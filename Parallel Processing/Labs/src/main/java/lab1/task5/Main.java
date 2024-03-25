package lab1.task5;

public class Main {
    public static void main(String[] args) {
        // CorrectPrinter thread1 = new CorrectPrinter("|");
        // CorrectPrinter thread2 = new CorrectPrinter("-");

       WrongPrinter thread1 = new WrongPrinter("|");
       WrongPrinter thread2 = new WrongPrinter("-");

        thread1.start();
        thread2.start();

    }
}
