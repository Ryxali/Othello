using UnityEngine;
using System.Collections;

public class Point2D {

    

    public int x, y;
    public Point2D(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public Point2D()
    {
        x = 0;
        y = 0;
    }
    public Point2D(Point2D copy)
    {
        this.x = copy.x;
        this.y = copy.y;
    }

    public static Point2D operator+(Point2D a, Point2D b)
    {
        return new Point2D(a.x + b.x, a.y + b.y);
    }

    public static Point2D operator -(Point2D a, Point2D b)
    {
        return new Point2D(a.x - b.x, a.y - b.y);
    }

    public static bool operator ==(Point2D a, Point2D b)
    {
        return a.x == b.x && a.y == b.y;
    }
    public static bool operator !=(Point2D a, Point2D b)
    {
        return !(a == b);
    }
    public override bool Equals(object a)
    {
        if (a.GetType() == typeof(Point2D)) return this == (Point2D)a;
        return base.Equals(a);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return x + ", " + y;
    }
}
