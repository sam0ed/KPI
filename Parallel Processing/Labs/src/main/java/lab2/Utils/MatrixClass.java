package lab2.Utils;

import java.util.Arrays;

/**
 * The MatrixClass class is used for working with and storing matrices.
 */
public class MatrixClass {
    private final int[][] matrix;

    /**
     * Constructor of the MatrixClass class, takes a matrix as a parameter and saves it.
     *
     * @param matrix the matrix to be saved, represented as a two-dimensional array.
     */
    public MatrixClass(int[][] matrix) {
        this.matrix = matrix;
    }


    /**
     * Constructor of the MatrixClass class, takes the dimensions of the matrix to be saved.
     *
     * @param rowsSize    the number of rows in the matrix.
     * @param columnsSize the number of columns in the matrix.
     */
    public MatrixClass(int rowsSize, int columnsSize) {
        matrix = new int[rowsSize][columnsSize];
    }

    public int[][] getMatrix() {
        return matrix;
    }

    public int getRowsSize() {
        return matrix.length;
    }

    public int getColumnsSize() {
        return matrix[0].length;
    }

    /**
     * Method that returns the value of a matrix element at the specified indices.
     *
     * @param i the row index.
     * @param j the column index.
     * @return the value of the matrix element at the specified indices.
     */
    public int get(int i, int j) {
        return matrix[i][j];
    }

    /**
     * Method that sets the value of a matrix element at the specified indices.
     *
     * @param i     the row index.
     * @param j     the column index.
     * @param value the value to be set for the matrix element at the specified indices.
     */
    public void set(int i, int j, int value) {
        matrix[i][j] = value;
    }


    /**
     * Method that prints the matrix to the console.
     */
    public void print2D() {
        Arrays.stream(matrix).map(Arrays::toString).forEach(System.out::println);
    }
}
