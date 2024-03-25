package kr1;

import java.util.concurrent.BlockingQueue;
import java.util.concurrent.LinkedBlockingQueue;

// Напишіть фрагмент коду, в якому створюються та запускаються на виконання потоки А,В,С. Потоки А і В виконують створення та додавання об’єктів в буфер обмеженої довжини, а потік С – їх вилучення. Використовуйте тип Object для об'єктів, які створюються, додаються та вилучаються. Для написання коду використовувати Java.


public class Buffer {
    private BlockingQueue<Object> buffer;

    public Buffer(int maxLength) {
        this.buffer = new LinkedBlockingQueue<>(maxLength);
    }

    public void add(Object obj) throws InterruptedException {
        buffer.put(obj);
    }

    public Object remove() throws InterruptedException {
        return buffer.take();
    }

    public static void main(String[] args) {
        final int maxLength = 10;
        final Buffer buffer = new Buffer(maxLength);

        Thread threadA = new Thread(() -> {
            for (int i = 0; i < maxLength; i++) {
                try {
                    buffer.add(new Object());
                    System.out.println("Thread A added object to buffer.");
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        });

        Thread threadB = new Thread(() -> {
            for (int i = 0; i < maxLength; i++) {
                try {
                    buffer.add(new Object());
                    System.out.println("Thread B added object to buffer.");
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        });

        Thread threadC = new Thread(() -> {
            for (int i = 0; i < maxLength; i++) {
                try {
                    buffer.remove();
                    System.out.println("Thread C removed object from buffer.");
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        });

        threadA.start();
        threadB.start();
        threadC.start();
    }
}

