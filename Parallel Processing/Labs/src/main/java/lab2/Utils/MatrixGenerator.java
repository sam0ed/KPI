package lab2.Utils;

import lombok.Getter;
import lombok.Setter;

public class MatrixGenerator {
    @Getter
    @Setter
    MatrixClass matrixEntity;

    public MatrixClass generateRandomMatrix(int rows, int columns) {
        matrixEntity = new MatrixClass(rows, columns);
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < columns; j++) {
                matrixEntity.set(i, j, (int) (Math.random() * 10));
            }
        }
        return matrixEntity;
    }
}
