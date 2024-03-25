package lab2.Algorithms.Fox;

import lab2.Algorithms.Sequential.SequentialCalculator;
import lab2.Utils.MatrixClass;
import lombok.var;

/**
 * Class for parallel matrix multiplication using the Fox algorithm
 */
public class FoxCalculatorThread extends Thread {
    private final MatrixClass matrixObj1;
    private final MatrixClass matrixObj2;
    private final int curRowShift;
    private final int curColShift;
    private final int blockSize;
    private final MatrixClass resultMatrix;

    /**
     * @param matrixEntity1 - matrix 1
     * @param matrixEntity2 - matrix 2
     * @param curRowShift  - starting row
     * @param curColShift - starting column
     * @param blockSize - block size of the matrix for the thread
     * @param resultMatrix - matrix for the result
     */
    public FoxCalculatorThread(MatrixClass matrixEntity1, MatrixClass matrixEntity2, int curRowShift,
                               int curColShift, int blockSize, MatrixClass resultMatrix) {
        this.resultMatrix = resultMatrix;
        this.matrixObj1 = matrixEntity1;
        this.matrixObj2 = matrixEntity2;
        this.curRowShift = curRowShift;
        this.curColShift = curColShift;
        this.blockSize = blockSize;
    }

    @Override
    public void run() {
        var m1RowSize = blockSize;
        var m2ColSize = blockSize;

        // this part ensures that no matter what block size was set in the caller, the last block will be of the correct size
        if (curRowShift + blockSize > matrixObj1.getRowsSize())
            m1RowSize = matrixObj1.getRowsSize() - curRowShift;

        if (curColShift + blockSize > matrixObj2.getColumnsSize())
            m2ColSize = matrixObj2.getColumnsSize() - curColShift;
        /////////////////////////
        // as you know when we multipy two matrices the amount of the columns of the first matrix should be equal to the amount of the rows of the second matrix
        // what we did in the code above ensures that if there is a mismatch in the amount of rows in the first matrix and the amount of columns in the second matrix it is not a big deal.
        // however the amount of columns in the first matrix should be equal to the amount of rows in the second matrix and that is the RULE.
        // we provide that in the cycle below
        
        for (int k = 0; k < matrixObj1.getRowsSize(); k += blockSize) {
            //we need to reset those sizes for every block in the loop so the declaration is inside the loop
            var m1ColSize = blockSize;
            var m2RowSize = blockSize;

            if (k + blockSize > matrixObj2.getRowsSize()) {
                m2RowSize = matrixObj2.getRowsSize() - k;
            }

            if (k + blockSize > matrixObj1.getColumnsSize()) {
                m1ColSize = matrixObj1.getColumnsSize() - k;
            }

            var blockFirst = copyBlock(matrixObj1, curRowShift, curRowShift + m1RowSize,
                    k, k + m1ColSize);
            var blockSecond = copyBlock(matrixObj2, k, k + m2RowSize,
                    curColShift, curColShift + m2ColSize);

            // so apparantly this algorithm doesn't do the whole broadcasting thing for some reason.
            // I presume this is because the dimensionality of the matrix would be all messed up in that case.
            // so instead we just multiply the blocks and add the result to the corresponding block in the result matrix
            var resBlock = new SequentialCalculator().multiplyMatrix(blockFirst, blockSecond);
            for (int i = 0; i < resBlock.getRowsSize(); i++) {
                for (int j = 0; j < resBlock.getColumnsSize(); j++) {
                    // add the elements of the resulting block to the corresponding row and column of the result matrix
                    resultMatrix.set(i + curRowShift, j + curColShift, resBlock.get(i, j)
                            + resultMatrix.get(i + curRowShift, j + curColShift));
                }
            }
        }
    }

    /**
     * @param src - matrix to copy from
     * @param rowStart - starting row
     * @param rowFinish - ending row
     * @param colStart - starting column
     * @param colFinish - ending column
     * @return a copy of the matrix block
     */
    private MatrixClass copyBlock(MatrixClass src, int rowStart, int rowFinish,
                              int colStart, int colFinish) {
        var copyMatrix = new MatrixClass(rowFinish - rowStart, colFinish - colStart);
        for (int i = 0; i < rowFinish - rowStart; i++) {
            for (int j = 0; j < colFinish - colStart; j++) {
                copyMatrix.set(i, j, src.get(i + rowStart, j + colStart));
            }
        }
        return copyMatrix;
    }
}
