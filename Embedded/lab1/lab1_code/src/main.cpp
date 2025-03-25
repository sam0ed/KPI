#include "stm32f103x6.h"

void setup_rcc(){
    RCC->CR |= RCC_CR_HSION; // enable internal high‑speed oscillator
    while (!(RCC->CR & RCC_CR_HSIRDY));

    RCC->CR |= RCC_CR_PLLON; // enable PLL
    while (!(RCC->CR & RCC_CR_PLLRDY));

    RCC->CFGR |= RCC_CFGR_SW_PLL; // set PLL as system clock
    while (!(RCC->CFGR & RCC_CFGR_SWS_PLL));

    // HPRE is responsible for prescaling of AHB bus, scaling it down by 4 means that APB bus will be N MHz
    // PPRE1 is responsible for prescaling of APB1 bus
    // the scaling chaing goes like: HSI(8MHz) -> PLLSRC(4MHz) -> After PLLMUL = 4*N  -> After AHB prescaler = N -> After APB = N/2 -> After TIM2 = N
    //                                                                                                           |-> APB2=1 -> TIM1 = N
    RCC->CFGR = (RCC->CFGR & ~(RCC_CFGR_PLLSRC | RCC_CFGR_PLLMULL)) ; //set PLL multiplier to N(1 in our case), but if we don't apply PLLMUL the default multiplier is 2
    RCC->CFGR = (RCC->CFGR & ~(RCC_CFGR_HPRE | RCC_CFGR_PPRE1 | RCC_CFGR_PPRE2)) | RCC_CFGR_PPRE1_DIV2 | RCC_CFGR_HPRE_DIV4;
    FLASH->ACR = (FLASH->ACR & ~FLASH_ACR_LATENCY) | FLASH_ACR_LATENCY_0;
    
    // After the system clock is set up, the code enables clocks for various peripherals:
    RCC->APB2ENR |= RCC_APB2ENR_IOPAEN
                 |  RCC_APB2ENR_AFIOEN
                 |  RCC_APB2ENR_TIM1EN;
    RCC->APB1ENR |= RCC_APB1ENR_TIM2EN;
}

void init_leds() {
    GPIOA->CRL |= GPIO_CRL_MODE0_0;
    GPIOA->CRL |= GPIO_CRL_MODE1_0;
    GPIOA->CRL |= GPIO_CRL_MODE2_0;
    GPIOA->CRL |= GPIO_CRL_MODE3_0;
    GPIOA->CRL |= GPIO_CRL_MODE4_0;
    GPIOA->CRL |= GPIO_CRL_MODE5_0;
    GPIOA->CRL |= GPIO_CRL_MODE6_0;
    GPIOA->CRH |= GPIO_CRH_MODE9_0;
    GPIOA->CRH |= GPIO_CRH_MODE10_0;
    GPIOA->CRH |= GPIO_CRH_MODE11_0;
}

void set_first() {
    GPIOA->BSRR = GPIO_BSRR_BS0;
    GPIOA->BSRR = GPIO_BSRR_BS1;
    GPIOA->BSRR = GPIO_BSRR_BS2;
    GPIOA->BSRR = GPIO_BSRR_BS3;
    GPIOA->BSRR = GPIO_BSRR_BS4;
}

void reset_first() {
    GPIOA->BSRR = GPIO_BSRR_BR0;
    GPIOA->BSRR = GPIO_BSRR_BR1;
    GPIOA->BSRR = GPIO_BSRR_BR2;
    GPIOA->BSRR = GPIO_BSRR_BR3;
    GPIOA->BSRR = GPIO_BSRR_BR4;
}

void set_second() {
    GPIOA->BSRR = GPIO_BSRR_BS5;
    GPIOA->BSRR = GPIO_BSRR_BS6;
    GPIOA->BSRR = GPIO_BSRR_BS9;
    GPIOA->BSRR = GPIO_BSRR_BS10;
    GPIOA->BSRR = GPIO_BSRR_BS11;
}

void reset_second() {
    GPIOA->BSRR = GPIO_BSRR_BR5;
    GPIOA->BSRR = GPIO_BSRR_BR6;
    GPIOA->BSRR = GPIO_BSRR_BR9;
    GPIOA->BSRR = GPIO_BSRR_BR10;
    GPIOA->BSRR = GPIO_BSRR_BR11;
}

extern "C" void TIM2_IRQHandler() {
    // choose which group of registers activate
    if (GPIOA->IDR & GPIO_IDR_IDR12) {
        reset_second();
        
        // Toggle LEDs 1-5 (if ON→OFF, if OFF→ON)
        if (GPIOA->ODR & GPIO_ODR_ODR0) {
            reset_first();
        } else {
            set_first();
        }
    } else {
        reset_first();

        if (GPIOA->ODR & GPIO_ODR_ODR5) {
            reset_second();
        } else {
            set_second();
        }
    }
    
    TIM2->SR &= ~TIM_SR_UIF; // reset the update event flag
}

void init_tim2() {
    TIM2->PSC = 2000-1; //scaling down the default 2 MHz to 1 kHz
    TIM2->ARR = 10000-1;
    TIM2->DIER = TIM_DIER_UIE;
    NVIC_EnableIRQ(TIM2_IRQn);
    TIM2->CR1 = TIM_CR1_CEN;
}

void init_pwm() {
    //Configures PA7 and PA8 pins for alternate function mode
    GPIOA->CRL &= ~(GPIO_CRL_MODE7 | GPIO_CRL_CNF7);
    GPIOA->CRL |= GPIO_CRL_MODE7_0 | GPIO_CRL_CNF7_1;
    GPIOA->CRH &= ~(GPIO_CRH_MODE8 | GPIO_CRH_CNF8);
    GPIOA->CRH |= GPIO_CRH_MODE8_0 | GPIO_CRH_CNF8_1;
    AFIO->MAPR |= AFIO_MAPR_TIM1_REMAP_0;

    TIM1->PSC = 2000-1; // prescale clock to 1khz from 2Mhz
    TIM1->ARR = 1000-1; // Auto-reload value (determines PWM period)
    TIM1->CCR1 = 500-1; // Compare value (determines duty cycle - 50% here)
    TIM1->CCMR1 |= TIM_CCMR1_OC1M_1 | TIM_CCMR1_OC1M_2; // Configures channel 1 for PWM mode 1 (output high when counter < compare value)

    TIM1->CCER |= TIM_CCER_CC1E | TIM_CCER_CC1P | TIM_CCER_CC1NE; //Enables both regular output (CC1E) and complementary output (CC1NE), Inverts polarity of regular output (CC1P)
    TIM1->BDTR |= TIM_BDTR_MOE;
    TIM1->CR1 |= TIM_CR1_CEN; //Starts the timer counter
}

int main() {
    setup_rcc();
    init_leds();
    init_tim2();
    init_pwm();

    while (true) {}

    return 0;
}