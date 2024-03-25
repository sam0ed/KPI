package lab2.test;

import java.io.FileWriter;
import java.io.IOException;

import lab2.Utils.MatrixClass;
import lab2.Utils.MatrixGenerator;
import lombok.var;

public class Task3Test {
    public static void main(String[] args) {
        MatrixGenerator randomMatrixGenerator = new MatrixGenerator();

        // if (args.length == 0) {
        // throw new IllegalArgumentException("Matrix size must be provided as an
        // argument");
        // }
        // var MATRIX_SIZE = Integer.parseInt(args[0]);
        var MATRIX_SIZE = 2000;
        var THREADS_COUNT = 4;

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

        var sequentialCalculator = new lab2.Algorithms.Sequential.SequentialCalculator();
        var parallelCalculator = new lab2.Algorithms.Parallel.ParallelCalculator();
        var foxCalculator = new lab2.Algorithms.Fox.FoxCalculator(matrixEntity, matrixEntity2, THREADS_COUNT);

        // Sequential test
        startTime = System.currentTimeMillis();
        var seqRes = new MatrixClass(sequentialCalculator.multiplyMatrix(matrixEntity, matrixEntity2).getMatrix());
        endTime = System.currentTimeMillis();
        System.out.println("Sequential: " + (endTime - startTime) + " ms " + "for " +
        MATRIX_SIZE + " matrix size" );
        // System.out.println(MATRIX_SIZE + "," + (endTime - startTime));
        try (FileWriter csvWriter = new FileWriter("results.csv", true)) { // 'true' for append mode
            csvWriter.append(MATRIX_SIZE + "," + (endTime - startTime) + "," +"sequential"+ "\n");
        } catch (IOException e) {
            e.printStackTrace();
        }

        // Parallel test
        startTime = System.currentTimeMillis();
        var parRes = new MatrixClass(
                parallelCalculator.multiplyMatrix(matrixEntity, matrixEntity2, THREADS_COUNT).getMatrix());
        endTime = System.currentTimeMillis();
        System.out.println("Parallel: " + (endTime - startTime) + " ms " + "for " +
        MATRIX_SIZE + " matrix size" );
        // System.out.println(MATRIX_SIZE + "," + (endTime - startTime));
        try (FileWriter csvWriter = new FileWriter("results.csv", true)) { // 'true' for append mode
            csvWriter.append(MATRIX_SIZE + "," + (endTime - startTime) + "," + "parallel"+ "\n");
        } catch (IOException e) {
            e.printStackTrace();
        }

        // Fox test
        startTime = System.currentTimeMillis();
        var foxRes = new MatrixClass(foxCalculator.multiplyMatrix().getMatrix());
        endTime = System.currentTimeMillis();
        System.out.println("Fox: " + (endTime - startTime) + " ms " + "for " +
        MATRIX_SIZE + " matrix size" );
        // System.out.println(MATRIX_SIZE + "," + (endTime - startTime));
        try (FileWriter csvWriter = new FileWriter("results.csv", true)) { // 'true' for append mode
            csvWriter.append(MATRIX_SIZE + "," + (endTime - startTime) + "," + "fox" + "\n");
        } catch (IOException e) {
            e.printStackTrace();
        }

        // Check results
        for (int i = 0; i < MATRIX_SIZE; i++) {
            for (int j = 0; j < MATRIX_SIZE; j++) {
                if (seqRes.get(i, j) != parRes.get(i, j) || seqRes.get(i, j) != foxRes.get(i, j)) {
                    System.out.println("Error");
                    return;
                }
            }
        }

    }
}
