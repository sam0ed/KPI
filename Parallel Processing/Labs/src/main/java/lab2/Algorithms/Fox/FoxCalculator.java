package lab2.Algorithms.Fox;

import lab2.Utils.MatrixClass;
import lombok.Getter;
import lombok.RequiredArgsConstructor;
import lombok.Setter;
import lombok.var;

/**
 * The FoxCalculator class is used for parallel matrix multiplication using the Fox algorithm.
 * The algorithm divides the matrix into parts that are executed in separate threads.
 */
@Getter
@Setter
@RequiredArgsConstructor
public class FoxCalculator {
    private MatrixClass matrixObj1;
    private MatrixClass matrixObj2;
    private int threadsCount;
    private MatrixClass resultMatrix;

    /**
     * Constructor of the FoxCalculator class, which takes the matrices to be multiplied and the number of threads
     * in which the algorithm will be executed.
     * @param matrixObj1 the first matrix
     * @param matrixObj2 the second matrix
     * @param threadsCount the number of threads
     */
    public FoxCalculator(MatrixClass matrixObj1, MatrixClass matrixObj2, int threadsCount) {
        this.matrixObj1 = matrixObj1;
        this.matrixObj2 = matrixObj2;
        this.resultMatrix = new MatrixClass(matrixObj1.getRowsSize(), matrixObj2.getColumnsSize());

        //  we ensure not less then 4 matrix entries per thread !!
        if (threadsCount > matrixObj1.getRowsSize() * matrixObj2.getColumnsSize() / 4) {
            this.threadsCount = matrixObj1.getRowsSize() * matrixObj2.getColumnsSize() / 4;
        } else this.threadsCount = Math.max(threadsCount, 1);
    }


    // this method is basically creating the threads needed for the Fox algorithm, all the calculations happen in the FoxCalculatorThread class
    public MatrixClass multiplyMatrix() {
        var step = (int) Math.ceil(1.0 * matrixObj1.getRowsSize() / (int) Math.sqrt(threadsCount));

        FoxCalculatorThread[] threads = new FoxCalculatorThread[threadsCount];
        var idx = 0;

        for (int i = 0; i < matrixObj1.getRowsSize(); i += step) {
            for (int j = 0; j < matrixObj2.getColumnsSize(); j += step) {
                threads[idx] = new FoxCalculatorThread(matrixObj1, matrixObj2, i, j, step, resultMatrix);
                idx++;
            }
        }

        for (int i = 0; i < idx; i++) {
            threads[i].start();
        }

        for (int i = 0; i < idx; i++) {
            try {
                threads[i].join();
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }

        return resultMatrix;
    }

}
