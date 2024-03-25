package lab1.task2;

import lombok.Getter;
import lombok.Setter;

import java.awt.*;
import java.awt.geom.Ellipse2D;

@Getter
@Setter
public class Pocket {
    private final int x;
    private final int y;
    public int radius;

    public Pocket(int x, int y, int radius) {
        this.x = x;
        this.y = y;
        this.radius = radius;
    }

    public void draw(Graphics2D graphics2d) {
        graphics2d.setColor(Color.BLUE);
        graphics2d.fill(new Ellipse2D.Double(x, y, radius*2, radius*2));
    }


    public boolean isBallInPocket(int x, int y) {
        double collideRadius = this.getRadius()*2;
        double distance = Math.sqrt(Math.pow(this.getX() - x, 2) + Math.pow(this.getY() - y, 2));
        return distance <= collideRadius;
    }
}
