package lab1.task2;

import lombok.Getter;
import lombok.Setter;

import javax.swing.*;
import java.awt.*;
import java.util.ArrayList;

@Getter
@Setter
public class BallCanvas extends JPanel {
    private ArrayList<Ball> balls = new ArrayList<>();
    private ArrayList<Pocket> pockets;
    private int droppedCounter = 0;
    JLabel droppedCounterLabel = new JLabel();

    // public BallCanvas() {
    //     pockets = generatePockets();
    // }

    public void add(Ball b) {
        this.balls.add(b);
    }

    public ArrayList<Pocket> getPockets() {
        return pockets;
    }

    @Override
    public void paintComponent(Graphics g) {
        super.paintComponent(g);
        Graphics2D g2 = (Graphics2D) g;
        if (pockets == null){
            pockets = generatePockets();
        
        }
        // pockets = generatePockets();
        paintPockets(g2);

        for (Ball ball : balls) {
            if (ball.isBallInPocket()) {
                droppedCounter++;
                // can't remove from list while iterating
            }
        }
        droppedCounterLabel.setText("Dropped balls: " + droppedCounter);
        balls.removeIf(ball -> ball.isBallInPocket());
        for (Ball b : balls) {
            b.draw(g2);
        }
    }


    private ArrayList<Pocket> generatePockets() {
        ArrayList<Pocket> pockets = new ArrayList<>();
        pockets.add(new Pocket(0, 0, 10));
        pockets.add(new Pocket(0, this.getHeight() - 20, 10));
        pockets.add(new Pocket(this.getWidth() - 20, 0, 10));
        pockets.add(new Pocket(this.getWidth() - 20, this.getHeight() - 20, 10));
        return pockets;
    }

    private void paintPockets(Graphics2D g2) {
        for (Pocket p : pockets) {
            p.draw(g2);
        }
    }


}
