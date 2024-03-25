package lab2.test;

import java.io.FileWriter;
import java.io.IOException;

import lab2.Utils.MatrixClass;
import lab2.Utils.MatrixGenerator;
import lombok.var;

public class Task4Test {
    public static void main(String[] args) {
        MatrixGenerator randomMatrixGenerator = new MatrixGenerator();

        var MATRIX_SIZE = 1500;
        var THREADS_COUNT = 28;

        var startTime = System.currentTimeMillis();
        var endTime = System.currentTimeMillis();

        var matrixEntity = new MatrixClass(
                randomMatrixGenerator
                        .generateRandomMatrix(MATRIX_SIZE, MATRIX_SIZE)
                        .getMatrix());

        var matrixEntity2 = new MatrixClass(
                randomMatrixGenerator
                        .generateRandomMatrix(MATRIX_SIZE, MATRIX_SIZE)
                        .getMatrix());

        var parallelCalculator = new lab2.Algorithms.Parallel.ParallelCalculator();
        var foxCalculator = new lab2.Algorithms.Fox.FoxCalculator(matrixEntity, matrixEntity2, THREADS_COUNT);

        // Parallel test
        startTime = System.currentTimeMillis();
        var parRes = new MatrixClass(
                parallelCalculator.multiplyMatrix(matrixEntity, matrixEntity2, THREADS_COUNT).getMatrix());
        endTime = System.currentTimeMillis();
        System.out.println("Parallel: " + (endTime - startTime) + " ms " + "with " +
                THREADS_COUNT + " threads");
        try (FileWriter csvWriter = new FileWriter("thread_results.csv", true)) { // 'true' for append mode
            csvWriter.append(THREADS_COUNT + "," + (endTime - startTime) + "," + "parallel" + "\n");
        } catch (IOException e) {
            e.printStackTrace();
        }

        // Fox test
        startTime = System.currentTimeMillis();
        var foxRes = new MatrixClass(foxCalculator.multiplyMatrix().getMatrix());
        endTime = System.currentTimeMillis();
        System.out.println("Fox: " + (endTime - startTime) + " ms " + "with " + THREADS_COUNT + " threads");
        try (FileWriter csvWriter = new FileWriter("thread_results.csv", true)) { // 'true' for append mode
            csvWriter.append(THREADS_COUNT + "," + (endTime - startTime) + "," + "fox" + "\n");
        } catch (IOException e) {
            e.printStackTrace();
        }

        // Check results
        for (int i = 0; i < MATRIX_SIZE; i++) {
            for (int j = 0; j < MATRIX_SIZE; j++) {
                if (parRes.get(i, j) != foxRes.get(i, j)) {
                    System.out.println("Error");
                    return;
                }
            }
        }
    }
}
