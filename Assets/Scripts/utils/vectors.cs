using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class vector4
    {
        public vector2 i;
        public vector2 j;

        public vector4()
        {
            i = new vector2();
            j = new vector2();
        }

        public vector4(vector2 i, vector2 j)
        {
            this.i = i;
            this.j = j;
        }

        public vector4(int i, int j, int x, int y)
        {
            this.i = new vector2(i, j);
            this.j = new vector2(x, y);
        }

        public static vector4 getSplit(vector2 pos, int chunkSize)
        {
            int chunkX = pos.x / chunkSize;
            int chunkY = pos.y / chunkSize;

            int tileX = pos.x % chunkSize;
            int tileY = pos.y % chunkSize;

            return new vector4(chunkX, chunkY, tileX, tileY);
        }

    public override string ToString()
    {
        return i.toString() + ", " + j.toString();
    }
}

public struct vector2
{
    public static readonly vector2 zero = new vector2();
    public static readonly vector2 up = new vector2(0, -1);
    public static readonly vector2 down = new vector2(0, 1);
    public static readonly vector2 left = new vector2(-1, 0);
    public static readonly vector2 right = new vector2(1, 0);

    public int x;
    public int y;

    public vector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public string toString()
    {
        return x + "," + y;
    }

    public static vector2 fromString(string a, string b)
    {
        int x;
        int y;

        if (!int.TryParse(a, out x))
        {
            x = -1;
        }

        if (!int.TryParse(b, out y))
        {
            y = -1;
        }

        return new vector2(x, y);
    }

    public static vector2 fromString(string line)
    {
        string[] lines = line.Split(',');

        int x;
        int y;

        if (!int.TryParse(lines[0], out x))
        {
            x = -1;
        }

        if (!int.TryParse(lines[1], out y))
        {
            y = -1;
        }

        return new vector2(x, y);
    }

    public int toInt(int width)
    {
        return y * width + x;
    }

    public float dist(vector2 other)
    {
        return (float)Math.Sqrt(Math.Pow(x - other.x, 2) + Math.Pow(y - other.y, 2));
    }

    public bool Equals(vector2 X, vector2 Y)
    {
        if (X.x == Y.x && X.y == Y.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override int GetHashCode()
    {
        return this.x << 16 + (short)this.y;
    }

    public static vector2 add(vector2 a, vector2 b)
    {
        return new vector2(a.x + b.x, a.y + b.y);
    }

    public static vector2 sub(vector2 a, vector2 b)
    {
        return new vector2(a.x - b.x, a.y - b.y);
    }

    public static vector2 negate(vector2 a)
    {
        return new vector2(-a.x, -a.y);
    }
}

public class vector2HashCode : IEqualityComparer<vector2>
{
    public bool Equals(vector2 X, vector2 Y)
    {
        if (X.x == Y.x && X.y == Y.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetHashCode(vector2 obj)
    {
        return obj.x << 16 + (short)obj.y;
    }
}

public class vector4HashCode : IEqualityComparer<vector4>
{
    public bool Equals(vector4 X, vector4 Y)
    {
        if (X.i.Equals(Y.i) && X.j.Equals(Y.j))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetHashCode(vector4 obj)
    {
        return obj.i.GetHashCode() ^ obj.j.GetHashCode();
    }
}