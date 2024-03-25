package kr1;

import java.util.concurrent.*;

import lombok.var;

public class ArraySummer {
    private static final int ARRAY_SIZE = 1000000;
    private static final int TASK_SIZE = 100;
    private static final int POOL_SIZE = 8;

    public static void main(String[] args) throws InterruptedException, ExecutionException {
        var array = new double[ARRAY_SIZE];
        for (int i = 0; i < ARRAY_SIZE; i++) {
            array[i] = 1; 
            // array[i] = Math.random();
        }

        var threadPool = Executors.newFixedThreadPool(POOL_SIZE);
        double totalSum = 0;

        for (int i = 0; i < ARRAY_SIZE; i += TASK_SIZE) {
            int endLimit = Math.min(i + TASK_SIZE, ARRAY_SIZE);
            Callable<Double> partSumTask = new PartialSummer(array, i, endLimit);
            var resultFuture = threadPool.submit(partSumTask);
            totalSum += resultFuture.get();
        }

        threadPool.shutdown();
        System.out.println("Total sum of Array Elements: " + totalSum);
    }
}

class PartialSummer implements Callable<Double> {
    private final double[] array;
    private final int startLimit;
    private final int endLimit;

    public PartialSummer(double[] segmentData, int startLimit, int endLimit) {
        this.array = segmentData;
        this.startLimit = startLimit;
        this.endLimit = endLimit;
    }

    @Override
    public Double call() {
        double partSum = 0;
        for (int i = startLimit; i < endLimit; i++) {
            partSum += array[i];
        }
        return partSum;
    }
}
