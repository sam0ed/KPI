package kr1;

public class ThreadStateSwitcher {

    private volatile String state = "r";

    // Метод для запуску потоку А
    public void startThreadA() {
        new Thread(() -> {
            while (true) {
                try {
                    synchronized (this) {
                        if ("r".equals(state)) {
                            state = "w";
                        } else {
                            state = "r";
                        }
                        this.notifyAll();
                    }
                    Thread.sleep(100);
                    if ("w".equals(state)) {
                        System.out.print("|jfksjadjfkaj"); // Виведення символу на консоль
                        Thread.sleep(1);
                    }
                } catch (InterruptedException e) {
                    Thread.currentThread().interrupt();
                }
            }
        }).start();
    }

    // Метод для запуску потоку B
    public void startThreadB() {
        new Thread(() -> {
            while (true) {
                try {
                    synchronized (this) {
                        while (!"r".equals(state)) {
                            this.wait();
                        }
                    }
                    for (int i = 100; i >= 0; i--) {
                        if (!"r".equals(state)) {
                            break;
                        }
                        System.out.println(i + " milliseconds");
                        Thread.sleep(10);
                    }
                } catch (InterruptedException e) {
                    Thread.currentThread().interrupt();
                }
            }
        }).start();
    }

    public static void main(String[] args) {
        ThreadStateSwitcher switcher = new ThreadStateSwitcher();
        switcher.startThreadA();
        switcher.startThreadB();
    }
}
