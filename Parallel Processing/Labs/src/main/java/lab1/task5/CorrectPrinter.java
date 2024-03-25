package lab1.task5;

public class CorrectPrinter extends Thread {
    private final String character;
    private static final Object lock = new Object();

    public CorrectPrinter(String character) {
        this.character = character;
    }

    public void run() {
        for (int i = 0; i < 100; i++) {
            for (int j = 0; j < 10; j++) {
                synchronized (lock) {
                    lock.notifyAll();
                    System.out.print(this.character);
                    // // if (j == 9) {
                    // //     System.out.println();
                    // // }
                    try {
                        lock.wait();
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                }
            }
        }
    }
}
