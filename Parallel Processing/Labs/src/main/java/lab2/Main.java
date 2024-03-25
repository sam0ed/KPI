package lab2;

import lab2.Algorithms.Fox.FoxCalculator;
import lab2.Algorithms.Sequential.SequentialCalculator;
import lab2.Utils.MatrixClass;
import lab2.Utils.MatrixGenerator;
import lombok.var;

public class Main {
        public static void main(String[] args) {
                MatrixGenerator randomMatrixGenerator = new MatrixGenerator();

                var SIZE = 8;
                var matrixObj1 = new MatrixClass(new int[][] {
                                { 1, 8 },
                                { 2, 3 },
                                { 11, 4 }
                });

                var matrixObj2 = new MatrixClass(new int[][] {
                                { 1, 8, 1, 6 },
                                { 9, 2, 3, 1 }
                });

                var matrixObj3 = new MatrixClass(
                                randomMatrixGenerator
                                                .generateRandomMatrix(SIZE, SIZE)
                                                .getMatrix());

                var matrixObj4 = new MatrixClass(
                                randomMatrixGenerator
                                                .generateRandomMatrix(SIZE, SIZE)
                                                .getMatrix());

                calculate(matrixObj1, matrixObj2);
                // calculate(matrixObj3, matrixObj4);
        }

        public static void calculate(MatrixClass m1, MatrixClass m2) {
                System.out.println("Matrix 1:");
                m1.print2D();
                System.out.println("####");

                System.out.println("Matrix 2:");
                m2.print2D();
                System.out.println("####");

                var sequentialCalculator = new SequentialCalculator();
                var res = new MatrixClass(sequentialCalculator.multiplyMatrix(m1, m2).getMatrix());
                System.out.println("sequential result:");
                res.print2D();

                var parallelCalculator = new lab2.Algorithms.Parallel.ParallelCalculator();
                System.out.println("parallel result:");
                var res2 = new MatrixClass(parallelCalculator.multiplyMatrix(m1, m2, 4).getMatrix());
                res2.print2D();

                var foxCalculator = new FoxCalculator(m1, m2, 4);
                System.out.println("fox result:");
                var res3 = new MatrixClass(foxCalculator.multiplyMatrix().getMatrix());
                res3.print2D();
        }
}
