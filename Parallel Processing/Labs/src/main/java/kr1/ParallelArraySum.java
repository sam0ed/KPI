package kr1;

import java.util.concurrent.*;

public class ParallelArraySum {
    private static final int N_THREADS = 8;
    private static final int TOTAL_NUMBERS = 1000000;
    private static final int TASKS = 100;
    
    public static void main(String[] args) throws InterruptedException, ExecutionException {
        ExecutorService executor = Executors.newFixedThreadPool(N_THREADS);
        double[] numbers = new double[TOTAL_NUMBERS]; // Array initialization is omitted for brevity
        
        // Fill the array with some numbers. For example, fill with 1.0 for simplicity
        for (int i = 0; i < numbers.length; i++) {
            numbers[i] = 1.0; // Example value
        }
        
        Future<Double>[] results = new Future[TASKS];
        int segmentSize = TOTAL_NUMBERS / TASKS;
        
        for (int i = 0; i < TASKS; i++) {
            final int start = segmentSize * i;
            final int end = (i == TASKS - 1) ? numbers.length : segmentSize * (i + 1);
            results[i] = executor.submit(() -> {
                double sum = 0;
                for (int j = start; j < end; j++) {
                    sum += numbers[j];
                }
                return sum;
            });
        }
        
        double totalSum = 0;
        for (Future<Double> result : results) {
            totalSum += result.get(); // This call blocks until the computation is complete
        }
        
        System.out.println("Total sum: " + totalSum);
        
        executor.shutdown();
        executor.awaitTermination(Long.MAX_VALUE, TimeUnit.MILLISECONDS);
    }
}
