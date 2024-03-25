package lab1.task5;

public class WrongPrinter extends Thread {
    private final String character;

    public WrongPrinter(String character) {
        this.character = character;
    }

    public void run() {
        for (int i = 0; i < 100; i++) {
            for (int j = 0; j < 10; j++) {
                System.out.print(this.character);
            }
            System.out.println();
        }
    }
}
