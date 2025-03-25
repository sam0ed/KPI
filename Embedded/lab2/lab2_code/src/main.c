#include "stm32f103x6.h"
#include <stdio.h>
#include <string.h>

#define BUFFER_LENGTH 250

char receive_buffer[BUFFER_LENGTH];
uint8_t receive_buffer_ind = 0;
volatile uint8_t input_complete_flag = 0;

// Configures ADC1(Analog-to-Digital Converter peripheral) in continuous conversion mode
void setup_ADC() {
    ADC1->CR2 |= ADC_CR2_ADON | ADC_CR2_CONT; // Enables the ADC by setting the ADON (ADC ON) bit
    ADC1->SMPR2 |= ADC_SMPR2_SMP0; //Configures the sampling time for ADC channel 0. Sets maximum sampling time for more stable readings. SMPR2 is the Sample Time Register 2
    ADC1->CR2 |= ADC_CR2_SWSTART; //start in continuous mode
}

// Sets up USART1 (TX only -- microcontroller sends data) and USART2 (RX Receive with interrupts -- microcontroller is notified when data received) -- Universal Synchronous/Asynchronous Receiver/Transmitter
void setup_USART1n2() {
    USART1->BRR = 8000000 / 9600; // Set baud rate to 9600(UART is configured to transmit/receive 9600 bits per second)
    USART1->CR1 |= USART_CR1_TE | USART_CR1_UE; // Enable USART1 and Transmitter

    NVIC_EnableIRQ(USART2_IRQn);
    USART2->BRR = 8000000 / 9600;
    USART2->CR1 |= USART_CR1_RE | USART_CR1_UE | USART_CR1_RXNEIE;
}

// Configures TIM2 with an 8MHz/8000 = 1kHz tick rate
void setup_timer(int enabled) {
    NVIC_EnableIRQ(TIM2_IRQn); // Nested Vectored Interrupt Controller (NVIC)
    TIM2->PSC = 8000 - 1;
    TIM2->ARR = 1000 - 1;
    TIM2->DIER = TIM_DIER_UIE; // Generates an interrupt when the counter reaches the ARR value
    if (enabled) {
        TIM2->CR1 = TIM_CR1_CEN; // Conditionally enables the timer counter by setting the Counter Enable (CEN) bit
    }
}

// Sets up TIM1 for PWM output on PA8
void setup_PWM(int enabled) {
    GPIOA->CRH &= ~(GPIO_CRH_MODE8 | GPIO_CRH_CNF8);
    GPIOA->CRH |= GPIO_CRH_MODE8_0 | GPIO_CRH_CNF8_1;
    AFIO->MAPR |= AFIO_MAPR_TIM1_REMAP_0;
    TIM1->PSC = 8000 - 1;
    TIM1->ARR = 1000 - 1;
    TIM1->CCR1 = 250 - 1;
    TIM1->CCER |= TIM_CCER_CC1E | TIM_CCER_CC1P;
    TIM1->BDTR |= TIM_BDTR_MOE;
    TIM1->CR1 |= TIM_CR1_CEN;
    if (enabled) {
        TIM1->CCMR1 |= TIM_CCMR1_OC1M_1 | TIM_CCMR1_OC1M_2;
    }
}

// Sends a single character over USART1
void send_char(char s) {
    while (!(USART1->SR & USART_SR_TC));
    USART1->DR = s;
}

void send_string(const char *msg) {
    while (*msg) {
        send_char(*msg++);
    }
}

void process_input(const char *input) {
    char tmp[50];
    if (strcmp(input, "ADC") == 0) {
        snprintf(tmp, sizeof(tmp), "ADC = %d \r\n", (int)ADC1->DR);
        send_string(tmp);
    } else if (strcmp(input, "ELED") == 0) {
        send_string("> Enabled LED!\r\n");
        GPIOA->BSRR = GPIO_BSRR_BS1;
    } else if (strcmp(input, "DLED") == 0) {
        send_string("> Disabled LED!\r\n");
        GPIOA->BSRR = GPIO_BSRR_BR1;
    } else if (strcmp(input, "ETimer") == 0) {
        send_string("> Enabled timer!\r\n");
        TIM2->CR1 = TIM_CR1_CEN;
    } else if (strcmp(input, "DTimer") == 0) {
        send_string("> Disabled timer!\r\n");
        TIM2->CR1 = 0;
    } else if (strcmp(input, "EPWM") == 0) {
        send_string("> Enabled PWM!\r\n");
        TIM1->CCMR1 |= TIM_CCMR1_OC1M_1 | TIM_CCMR1_OC1M_2;
    } else if (strcmp(input, "DPWM") == 0) {
        send_string("> Disabled PWM!\r\n");
        TIM1->CCMR1 &= ~(TIM_CCMR1_OC1M_1 | TIM_CCMR1_OC1M_2);
    } else if (input[0] >= '1' && input[0] <= '5' && input[1] == '\0') {
        snprintf(tmp, sizeof(tmp), "> Set timer's delay to %d sec!\r\n", input[0] - '0');
        send_string(tmp);
        TIM2->ARR = (input[0] - '0') * 1000 - 1;
    } else if (input[0] >= '6' && input[0] <= '9' && input[1] == '\0') {
        snprintf(tmp, sizeof(tmp), "> Set PWM's pulse length to %d ms!\r\n", 1000 - 250 * (input[0] - '6'));
        send_string(tmp);
        TIM1->CCR1 = (1000 - 250 * (input[0] - '6')) - 1;
    }
}

// receives interrupts for USART2, collecting incoming characters and assembling them into complete command strings.
void USART2_IRQHandler() {
    if (USART2->SR & USART_SR_RXNE) { //Ensures an interrupt was triggered because data was actually received
        char received = USART2->DR; // read data register

        if (received == '\r' || received == '\n') {
            receive_buffer[receive_buffer_ind] = '\0';
            input_complete_flag = 1;
            receive_buffer_ind = 0;
        } else if (receive_buffer_ind < BUFFER_LENGTH - 1) { // If the character isn't a line ending and there's space in the buffer
            receive_buffer[receive_buffer_ind++] = received;
        }
    }
}


void TIM2_IRQHandler() {
    send_string("Timer is up!\r\n");
    TIM2->SR &= ~TIM_SR_UIF;
}

int main() {
    RCC->APB1ENR |= RCC_APB1ENR_USART2EN | RCC_APB1ENR_TIM2EN; // Enables the peripheral clocks for USART2 and TIM2 on the APB1 bus, without enabling these clocks, the peripherals remain inactive
    RCC->APB2ENR |= RCC_APB2ENR_USART1EN | RCC_APB2ENR_IOPAEN | RCC_APB2ENR_ADC1EN | RCC_APB2ENR_TIM1EN; // Enables peripheral clocks for USART1, GPIO Port A, ADC1, and TIM1 on the APB2 bus

    setup_ADC();
    setup_USART1n2();
    GPIOA->CRL |= GPIO_CRL_MODE1_0; // Configures GPIO pin PA1 as an output pin (for the LED)
    // init without enabling
    setup_timer(0); 
    setup_PWM(0);

    send_string("Information.\r\n==========================\r\n");
    send_string("ADC - Show ADC\r\nELED - Enable LED\r\nDLED - Disable LED\r\nETimer - Enable Timer Display\r\nDTimer - Disable Timer Display\r\nEPWM - Enable PWM\r\nDPWM - Disable PWM\r\n==========================\r\n");
    send_string("1-5 - Set timer's delay to X seconds\r\n6-9 - Set PWM's pulse length to 1000-250*(X-1) ms.\r\n==========================\r\n");

    // The main function implements a simple command-parsing event loop that waits for complete commands to arrive via UART, processes them, and then waits for more commands.
    while (1) {
            if (input_complete_flag) { // Checks if a complete command has been received by the interrupt handler
                process_input(receive_buffer);
                input_complete_flag = 0;
            }
        }
    return 0;
}
