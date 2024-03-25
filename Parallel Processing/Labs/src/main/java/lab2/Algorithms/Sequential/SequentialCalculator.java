package lab2.Algorithms.Sequential;

import lab2.Utils.MatrixClass;
import lombok.var;

public class SequentialCalculator {
    public MatrixClass multiplyMatrix(MatrixClass matrixEntity1, MatrixClass matrixEntity2) {
        if (matrixEntity1.getColumnsSize() != matrixEntity2.getRowsSize()) {
            throw new IllegalArgumentException(
                    "Error during multiplication occured. The number of columns of matrix A is not equal to the number of rows of matrix B.");
        }

        var resultMatrix = new MatrixClass(matrixEntity1.getRowsSize(), matrixEntity2.getColumnsSize());
        for (int i = 0; i < matrixEntity1.getRowsSize(); i++) {
            for (int j = 0; j < matrixEntity2.getColumnsSize(); j++) {
                for (int k = 0; k < matrixEntity1.getColumnsSize(); k++) {
                    resultMatrix.set(i, j, resultMatrix.get(i, j) + matrixEntity1.get(i, k) * matrixEntity2.get(k, j));
                }
            }
        }

        return resultMatrix;
    }

}
