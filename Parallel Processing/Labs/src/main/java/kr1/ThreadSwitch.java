package kr1;

public class ThreadSwitch {
    private volatile String threadAState = "r";

    public static void main(String[] args) {
        ThreadSwitch switcher = new ThreadSwitch();

        Thread threadA = new Thread(() -> {
            try {
                while (!Thread.currentThread().isInterrupted()) {
                    synchronized (switcher) {
                        switcher.threadAState = switcher.threadAState.equals("r") ? "w" : "r";
                        switcher.notifyAll();
                    }
                    Thread.sleep(100);
                }
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            }
        });

        Thread threadB = new Thread(() -> {
            try {
                while (!Thread.currentThread().isInterrupted()) {
                    synchronized (switcher) {
                        while (!switcher.threadAState.equals("r")) {
                            switcher.wait();
                        }

                        for (int i = 100; i >= 0; i -= 10) {
                            System.out.println(i + " milliseconds");
                            Thread.sleep(10);

                            if (switcher.threadAState.equals("w")) {
                                break;
                            }
                        }
                    }
                }
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            }
        });

        threadA.start();
        threadB.start();
    }
}


