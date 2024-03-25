package kr1;

// Напишіть фрагмент коду програми, в якому N потоків виконують 1000 ітерацій, в кожній з яких створюють об'єкт String з випадковим набором символів та одразу додають його у  спільну для всіх потоків колекцією об'єктів  String.

import java.util.*;
import java.util.concurrent.*;

public class StringGenerator implements Runnable {
    private static final int ITERATIONS = 1000;
    private final List<String> sharedCollection;

    public StringGenerator(List<String> sharedCollection) {
        this.sharedCollection = sharedCollection;
    }

    private String generateRandomString() {
        String characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder stringBuilder = new StringBuilder();
        Random rnd = new Random();
        while (stringBuilder.length() < 10) { // length of the random string.
            int index = (int) (rnd.nextFloat() * characters.length());
            stringBuilder.append(characters.charAt(index));
        }
        return stringBuilder.toString();
    }

    @Override
    public void run() {
        for (int i = 0; i < ITERATIONS; i++) {
            synchronized (sharedCollection) {
                sharedCollection.add(generateRandomString());
            }
        }
    }

    public static void main(String[] args) throws InterruptedException {
        final int N = 5;
        List<String> sharedCollection = Collections.synchronizedList(new ArrayList<>());
        ExecutorService executor = Executors.newFixedThreadPool(N);

        for (int i = 0; i < N; i++) {
            executor.execute(new StringGenerator(sharedCollection));
        }

        executor.shutdown();
        executor.awaitTermination(Long.MAX_VALUE, TimeUnit.NANOSECONDS);

        System.out.println("Size of shared collection: " + sharedCollection.size());
    }
}
