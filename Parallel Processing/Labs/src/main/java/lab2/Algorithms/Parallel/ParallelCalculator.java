package lab2.Algorithms.Parallel;

import lombok.var;

import java.util.ArrayList;

import lab2.Utils.MatrixClass;


/**
 * The ParallelCalculator class is used for parallel matrix multiplication.
 * Inside the class, a naive matrix multiplication algorithm is implemented.
 */
public class ParallelCalculator {

    /**
     * Method for matrix multiplication using the naive algorithm.
     * @param matrixObj1 the first matrix
     * @param matrixObj2 the second matrix
     * @param threadsAmount the number of threads
     * @return the result of matrix multiplication as a MatrixClass object
     */
    public MatrixClass multiplyMatrix(MatrixClass matrixObj1, MatrixClass matrixObj2, int threadsAmount) {

        // Check if the matrices can be multiplied (the number of columns of matrix A
        // must be equal to the number of rows of matrix B)
        if (matrixObj1.getColumnsSize() != matrixObj2.getRowsSize()) {
            throw new IllegalArgumentException("matrices cannot be multiplied because the " +
                    "number of columns of matrix A is not equal to the number of rows of matrix B.");
        }

        var height = matrixObj1.getRowsSize();
        var width = matrixObj2.getColumnsSize();
        var resultMatrix = new MatrixClass(height, width);


        var rowsPerThread = height / threadsAmount;
        var threads = new ArrayList<Thread>();
        for (int i = 0; i < threadsAmount; i++) {
            // set the lower boundary of the range of rows that the thread will process
            var threadLowerBound = i * rowsPerThread;

            // set the upper boundary of the range of rows that the thread will process
            int threadUpperBound;
            if (i == threadsAmount - 1) {
                // this is needed so that the last thread processes any rows that are left 
                // if the height is not divisible by the number of threads
                threadUpperBound = height; 
            } else {
                threadUpperBound = (i + 1) * rowsPerThread;
            }

             // Create a thread that will calculate the result within the specified range
            threads.add(new Thread(() -> {

                // the part below is the same as the sequential algorithm but for a part of the matrix bounded by the range of the thread
                for (int row = threadLowerBound; row < threadUpperBound; row++) { 
                    for (int col = 0; col < width; col++) {
                        for (int k = 0; k < matrixObj2.getRowsSize(); k++) { 
                            resultMatrix.set(row, col, resultMatrix.get(row, col)
                                    + matrixObj1.get(row, k) * matrixObj2.get(k, col));
                        }
                    }
                }
            }));
        }

        for (Thread thread : threads) {
            thread.start();
        }

        try { // Wait for all threads to finish their work
            for (Thread thread : threads) {
                thread.join();
            }
        } catch (InterruptedException e) {
            e.printStackTrace();
        }

        return resultMatrix;
    }

}
