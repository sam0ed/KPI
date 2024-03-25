package lab1.task3;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

public class BounceFrame extends JFrame {
    private BallCanvas canvas;
    public static final int WIDTH = 450;
    public static final int HEIGHT = 350;

    private static final int BLUE_PRIORITY = 1;
    private static final int RED_PRIORITY = 5;

    private static final int RED_QUANTITY  = 1;
    private static final int BLUE_QUANTITY = 300;

    public BounceFrame() {
        this.setSize(WIDTH, HEIGHT);
        this.setTitle("Bounce programm");
        this.canvas = new BallCanvas();
        System.out.println("In Frame Thread name = "
                + Thread.currentThread().getName());
        Container content = this.getContentPane();
        content.add(this.canvas, BorderLayout.CENTER);
        JPanel buttonPanel = new JPanel();
        buttonPanel.setBackground(Color.lightGray);
        JButton startBlue = new JButton("Add Blue");
        JButton startRed = new JButton("Add Red");
        JButton buttonStop = new JButton("Stop");
        startBlue.addActionListener(new ActionListener() {

            @Override
            public void actionPerformed(ActionEvent e) {

                for (int i = 0; i < BLUE_QUANTITY; i++) {
                    Ball b = new Ball(canvas);
                    b.setColor(Color.blue);
                    canvas.add(b);

                    BallThread thread = new BallThread(b, BLUE_PRIORITY);
                    thread.start();
                    System.out.println("Thread name = " +
                            thread.getName());
                }

            }
        });
        startRed.addActionListener(new ActionListener() {

            @Override
            public void actionPerformed(ActionEvent e) {

                for (int i = 0; i < RED_QUANTITY; i++) {
                    Ball b = new Ball(canvas);
                    b.setColor(Color.red);
                    canvas.add(b);

                    BallThread thread = new BallThread(b, RED_PRIORITY);
                    thread.start();
                    System.out.println("Thread name = " +
                            thread.getName());
                }
            }
        });
        buttonStop.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {

                System.exit(0);
            }
        });
        buttonPanel.add(startBlue);
        buttonPanel.add(startRed);
        buttonPanel.add(buttonStop);

        content.add(buttonPanel, BorderLayout.SOUTH);
    }
}
