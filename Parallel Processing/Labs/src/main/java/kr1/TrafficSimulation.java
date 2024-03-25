package kr1;

import java.util.concurrent.atomic.AtomicInteger;

public class TrafficSimulation {
    private static final AtomicInteger carsPassed = new AtomicInteger(0);
    private static final Object lock = new Object();
    private static volatile String trafficLight = "GREEN";

    public static void main(String[] args) throws InterruptedException {
        Thread trafficLightThread = new Thread(() -> {
            try {
                while (true) {
                    synchronized (lock) {
                        trafficLight = "GREEN";
                    }
                    Thread.sleep(70);
                    synchronized (lock) {
                        trafficLight = "YELLOW";
                    }
                    Thread.sleep(10);
                    synchronized (lock) {
                        trafficLight = "RED";
                    }
                    Thread.sleep(40);
                    synchronized (lock) {
                        trafficLight = "YELLOW";
                    }
                    Thread.sleep(10);
                }
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        });

        trafficLightThread.start();

        for (int i = 0; i < 100; i++) {
            new Thread(() -> {
                try {
                    while (carsPassed.get() < 10000) {
                        synchronized (lock) {
                            if ("GREEN".equals(trafficLight)) {
                                go();
                                carsPassed.incrementAndGet();
                            }
                        }
                        Thread.sleep(400);
                    }
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }).start();
        }
    }

    private static void go() throws InterruptedException {
        System.out.println("Car is passing, total passed: " + carsPassed);
        Thread.sleep(2); 
    }
}
