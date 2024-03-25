package lab1.task2;

public class BallThread extends Thread {
    private Ball b;

    public BallThread(Ball ball) {
        b = ball;
    }

    @Override
    public void run() {
        try {
            for (int i = 1; i < 10000; i++) {
                if (b.isBallInPocket()) {
                    //finish the cycle with return without interrupting the thread
                    System.out.println("Ball in pocket");
                    return;
                }
                b.move();
                System.out.println("Thread name = "
                        + Thread.currentThread().getName());
                Thread.sleep(5);

            }
        } catch (InterruptedException ex) {
        }
    }
}
