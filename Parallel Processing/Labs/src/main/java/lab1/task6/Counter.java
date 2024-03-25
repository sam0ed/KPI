package lab1.task6;

public class Counter {
    private int count = 0;
    private final Object lock = new Object();

    public synchronized void incMethod() {
        count++;
    }

    public synchronized void decMethod() {
        count--;
    }

    public void incSyncBlock() {
        synchronized (lock) {
            count++;
        }
    }

    public void decSyncBlock() {
        synchronized (lock) {
            count--;
        }
    }

    public synchronized int getCount() {
        return count;
    }

    public void wrongInc() {
        count++;
    }

    public void wrongDec() {
        count--;
    }
}
