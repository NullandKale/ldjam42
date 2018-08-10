using System;
using System.Collections.Generic;
//using System.Drawing;
using System.IO;

public class utils
{
    private static int RandomSeed = 0;
    private static Random rng;

    public static T RandomEnumValue<T>()
    {
        var v = Enum.GetValues(typeof(T));
        return (T)v.GetValue(rng.Next(v.Length));
    }

    public static void clearInput()
    {
        int counter = 0;
        while (Console.Read() != 0)
        {
            if (counter > 1000)
            {
                Log.e("Took too long to remove chars from console buffer");
                break;
            }

            counter++;
        }
    }

    public static vector2 directionToVector2(direction d)
    {
        switch (d)
        {
            case direction.up:
                return vector2.up;

            case direction.down:
                return vector2.down;

            case direction.left:
                return vector2.left;

            case direction.right:
                return vector2.right;
        }

        return new vector2();
    }

    internal static float stringToFloat(string input)
    {
        float x;

        if (!float.TryParse(input, out x))
        {
            x = -1.0f;
        }

        return x;
    }

    public static string getTypeString(object o)
    {
        return o.GetType().ToString() + "|";
    }

    public static void setSeed(int seed)
    {
        RandomSeed = seed;
        Simplex.Noise.Seed = seed;
    }

    public static int stringToInt(string input)
    {
        int x;

        if (!int.TryParse(input, out x))
        {
            x = -1;
        }

        return x;
    }

    public static bool stringToBool(string input)
    {
        bool x;

        if (!bool.TryParse(input, out x))
        {
            x = false;
            Log.e("Bool failed to parse!");
        }

        return x;
    }

    public static int getIntInRange(int min, int max)
    {
        if (rng == null)
        {
            rng = new Random(RandomSeed);
        }

        return rng.Next(min, max);
    }

    public static int getCenteredInt(int change, int center)
    {
        if (rng == null)
        {
            rng = new Random(RandomSeed);
        }

        return rng.Next(center - change, center + change);
    }

    public static int map2Dto1D(int x, int y, int width)
    {
        return y * width + x;
    }

    public static vector2 map1Dto2D(int val, int width)
    {
        return new vector2(val % width, val / width);
    }

    public static string ConsoleColorToString(ConsoleColor c)
    {
        return ColorCodeToInt(c).ToString();
    }

    public static float[,] normalizeMap(float[,] map)
    {
        float max = -100000;
        float min = +100000;

        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                if (map[x, y] > max)
                {
                    max = map[x, y];
                }

                if (map[x, y] < min)
                {
                    min = map[x, y];
                }
            }
        }

        float[,] reMapped = new float[map.GetLength(0), map.GetLength(1)];

        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                reMapped[x, y] = Remap(map[x, y], min, max, 0, 1);
            }
        }

        return reMapped;
    }

    public static float[,] invertMap(float[,] map)
    {
        float[,] reMapped = new float[map.GetLength(0), map.GetLength(1)];

        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                reMapped[x, y] = 1 - map[x, y];
            }
        }

        return reMapped;
    }

    public static float[,] addCircleMap(float[,] map, float percentage, float lowerAmount)
    {
        float[,] reMapped = new float[map.GetLength(0), map.GetLength(1)];

        vector2 zero = new vector2(map.GetLength(0) / 2, map.GetLength(1) / 2);
        vector2 currentPos = new vector2();
        vector2 max = new vector2(map.GetLength(0), map.GetLength(1));

        float range = Remap(percentage, 0, 1, 0, max.dist(zero));

        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                currentPos.x = x;
                currentPos.y = y;

                if (currentPos.dist(zero) > range)
                {
                    reMapped[x, y] = 1;
                }
                else
                {
                    reMapped[x, y] = map[x, y] - lowerAmount;

                    if (reMapped[x, y] < 0)
                    {
                        reMapped[x, y] = 0;
                    }
                }
            }
        }

        return reMapped;
    }

    public static float getMapMin(float[,] map)
    {
        float toReturn = float.MaxValue;

        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                if (map[x, y] < toReturn)
                {
                    toReturn = map[x, y];
                }
            }
        }

        return toReturn;
    }

    public static float getMapMax(float[,] map)
    {
        float toReturn = float.MinValue;

        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                if (map[x, y] > toReturn)
                {
                    toReturn = map[x, y];
                }
            }
        }

        return toReturn;
    }

    public static vector2 getCenteredVector2(vector2 center, int dist)
    {
        int x = getCenteredInt(dist, center.x);
        int y = getCenteredInt(dist, center.y);

        return new vector2(x, y);
    }

    /*public static Bitmap makeBitmap(float[,] array)
    {
        Bitmap bitmap = new Bitmap(array.GetLength(0), array.GetLength(1));

        for (int y = 0; y < array.GetLength(1); y++)
        {
            for (int x = 0; x < array.GetLength(0); x++)
            {
                int value = (int)Remap(array[x, y], 0, 1, 0, 255);
                if (value < 0)
                {
                    value = 0;
                }
                bitmap.SetPixel(x, y, Color.FromArgb(255, value, value, value));
            }
        }

        return bitmap;
    }*/

    /*public static Bitmap makeBitmap(float[,] r, float[,] b, float[,] g)
    {
        Bitmap bitmap = new Bitmap(r.GetLength(0), r.GetLength(1));

        for (int y = 0; y < r.GetLength(1); y++)
        {
            for (int x = 0; x < r.GetLength(0); x++)
            {
                int red = (int)Remap(r[x, y], 0, 1, 0, 255);
                int blue = (int)Remap(b[x, y], 0, 1, 0, 255);
                int green = (int)Remap(g[x, y], 0, 1, 0, 255);

                if (red < 0)
                {
                    red = 0;
                }

                if (blue < 0)
                {
                    blue = 0;
                }

                if (green < 0)
                {
                    green = 0;
                }

                bitmap.SetPixel(x, y, Color.FromArgb(255, red, green, blue));
            }
        }

        return bitmap;
    }*/

    public static ConsoleColor ConsoleColorFromString(string s)
    {
        int f = -1;
        if (int.TryParse(s, out f))
        {
            return ColorCodeFromInt(f);
        }
        else
        {
            return ConsoleColor.Black;
        }
    }

    /*public static Color ColorCodeToColor(ConsoleColor c)
    {
        switch (c)
        {
            case ConsoleColor.Black:
                return Color.Black;

            case ConsoleColor.Blue:
                return Color.Blue;

            case ConsoleColor.Cyan:
                return Color.Cyan;

            case ConsoleColor.DarkBlue:
                return Color.DarkBlue;

            case ConsoleColor.DarkCyan:
                return Color.DarkCyan;

            case ConsoleColor.DarkGray:
                return Color.DarkGray;

            case ConsoleColor.DarkGreen:
                return Color.DarkGreen;

            case ConsoleColor.DarkMagenta:
                return Color.DarkMagenta;

            case ConsoleColor.DarkRed:
                return Color.DarkRed;

            case ConsoleColor.DarkYellow:
                return Color.SandyBrown;

            case ConsoleColor.Gray:
                return Color.Gray;

            case ConsoleColor.Green:
                return Color.Green;

            case ConsoleColor.Magenta:
                return Color.Magenta;

            case ConsoleColor.Red:
                return Color.Red;

            case ConsoleColor.White:
                return Color.White;

            case ConsoleColor.Yellow:
                return Color.Yellow;

            default:
                return Color.MediumAquamarine;
        }
    }*/

    private static int ColorCodeToInt(ConsoleColor c)
    {
        switch (c)
        {
            case ConsoleColor.Black:
                return 0;

            case ConsoleColor.Blue:
                return 1;

            case ConsoleColor.Cyan:
                return 2;

            case ConsoleColor.DarkBlue:
                return 3;

            case ConsoleColor.DarkCyan:
                return 4;

            case ConsoleColor.DarkGray:
                return 5;

            case ConsoleColor.DarkGreen:
                return 6;

            case ConsoleColor.DarkMagenta:
                return 7;

            case ConsoleColor.DarkRed:
                return 8;

            case ConsoleColor.DarkYellow:
                return 9;

            case ConsoleColor.Gray:
                return 10;

            case ConsoleColor.Green:
                return 11;

            case ConsoleColor.Magenta:
                return 12;

            case ConsoleColor.Red:
                return 13;

            case ConsoleColor.White:
                return 14;

            case ConsoleColor.Yellow:
                return 15;

            default:
                return -1;
        }
    }

    private static ConsoleColor ColorCodeFromInt(int i)
    {
        switch (i)
        {
            case 0:
                return ConsoleColor.Black;

            case 1:
                return ConsoleColor.Blue;

            case 2:
                return ConsoleColor.Cyan;

            case 3:
                return ConsoleColor.DarkBlue;

            case 4:
                return ConsoleColor.DarkCyan;

            case 5:
                return ConsoleColor.DarkGray;

            case 6:
                return ConsoleColor.DarkGreen;

            case 7:
                return ConsoleColor.DarkMagenta;

            case 8:
                return ConsoleColor.DarkRed;

            case 9:
                return ConsoleColor.DarkYellow;

            case 10:
                return ConsoleColor.Gray;

            case 11:
                return ConsoleColor.Green;

            case 12:
                return ConsoleColor.Magenta;

            case 13:
                return ConsoleColor.Red;

            case 14:
                return ConsoleColor.White;

            case 15:
                return ConsoleColor.Yellow;

            default:
                return ConsoleColor.Black;
        }
    }

    public static string getString(string message)
    {
        Console.Write(message);
        return Console.ReadLine();
    }

    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static int getInt(string message)
    {
        Console.Write(message);
        string s = Console.ReadLine();
        int toReturn;

        while (!int.TryParse(s, out toReturn))
        {
            Console.WriteLine("Invalid Try Again: ");
            Console.Write(message);
            s = Console.ReadLine();
        }
        return toReturn;
    }
}

public static class Log
{
    public static LogMode currentMode = LogMode.ERROR;
    public static bool doLogFile = true;

    public static void d(string message)
    {
        if ((int)currentMode >= (int)LogMode.DEBUG)
        {
            //write(message);
        }
    }

    public static void v(string message)
    {
        if ((int)currentMode >= (int)LogMode.VERBOSE)
        {
            //write(message);
        }
    }

    public static void e(string message)
    {
        if ((int)currentMode >= (int)LogMode.ERROR)
        {
            //write(message);
        }
    }
    /*
    private static void write(string message)
    {
        if (doLogFile)
        {
            string path = world.Program.loadedWorldPath + "LOGFILE.txt";
            // This text is added only once to the file.
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(message);
                }
            }
        }
        else
        {
            Console.WriteLine(message);
        }
    }*/

    public static void setLogMode(LogMode l)
    {
        currentMode = l;
    }

    public enum LogMode
    {
        ERROR = 0,
        DEBUG = 1,
        VERBOSE = 2
    }
}

// SimplexNoise for C#
// Author: Benjamin Ward
// Originally authored by Heikki Törmälä

namespace Simplex
{
    /// <summary>
    /// Implementation of the Perlin simplex noise, an improved Perlin noise algorithm.
    /// Based loosely on SimplexNoise1234 by Stefan Gustavson <http://staffwww.itn.liu.se/~stegu/aqsis/aqsis-newnoise/>
    /// </summary>
    public class Noise
    {
        public static float[] Calc1D(int width, float scale)
        {
            float[] values = new float[width];
            for (int i = 0; i < width; i++)
                values[i] = Generate(i * scale) * 128 + 128;
            return values;
        }

        public static float[,] Calc2D(int width, int height, float scale)
        {
            float[,] values = new float[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    values[i, j] = Generate(i * scale, j * scale) * 128 + 128;
            return values;
        }

        public static float[,,] Calc3D(int width, int height, int length, float scale)
        {
            float[,,] values = new float[width, height, length];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    for (int k = 0; k < length; k++)
                        values[i, j, k] = Generate(i * scale, j * scale, k * scale) * 128 + 128;
            return values;
        }

        public static float CalcPixel1D(int x, float scale)
        {
            return Generate(x * scale) * 128 + 128;
        }

        public static float CalcPixel2D(int x, int y, float scale)
        {
            return Generate(x * scale, y * scale) * 128 + 128;
        }

        public static float CalcPixel3D(int x, int y, int z, float scale)
        {
            return Generate(x * scale, y * scale, z * scale) * 128 + 128;
        }

        static Noise()
        {
            perm = new byte[permOriginal.Length];
            Simplex.Noise.permOriginal.CopyTo(perm, 0);
        }

        public static int Seed
        {
            get { return seed; }
            set
            {
                if (value == 0)
                {
                    perm = new byte[permOriginal.Length];
                    Simplex.Noise.permOriginal.CopyTo(perm, 0);
                }
                else
                {
                    perm = new byte[512];
                    Random random = new Random(value);
                    random.NextBytes(perm);
                }
            }
        }
        private static int seed = 0;

        /// <summary>
        /// 1D simplex noise
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        internal static float Generate(float x)
        {
            int i0 = FastFloor(x);
            int i1 = i0 + 1;
            float x0 = x - i0;
            float x1 = x0 - 1.0f;

            float n0, n1;

            float t0 = 1.0f - x0 * x0;
            t0 *= t0;
            n0 = t0 * t0 * grad(perm[i0 & 0xff], x0);

            float t1 = 1.0f - x1 * x1;
            t1 *= t1;
            n1 = t1 * t1 * grad(perm[i1 & 0xff], x1);
            // The maximum value of this noise is 8*(3/4)^4 = 2.53125
            // A factor of 0.395 scales to fit exactly within [-1,1]
            return 0.395f * (n0 + n1);
        }

        /// <summary>
        /// 2D simplex noise
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        internal static float Generate(float x, float y)
        {
            const float F2 = 0.366025403f; // F2 = 0.5*(sqrt(3.0)-1.0)
            const float G2 = 0.211324865f; // G2 = (3.0-Math.sqrt(3.0))/6.0

            float n0, n1, n2; // Noise contributions from the three corners

            // Skew the input space to determine which simplex cell we're in
            float s = (x + y) * F2; // Hairy factor for 2D
            float xs = x + s;
            float ys = y + s;
            int i = FastFloor(xs);
            int j = FastFloor(ys);

            float t = (float)(i + j) * G2;
            float X0 = i - t; // Unskew the cell origin back to (x,y) space
            float Y0 = j - t;
            float x0 = x - X0; // The x,y distances from the cell origin
            float y0 = y - Y0;

            // For the 2D case, the simplex shape is an equilateral triangle.
            // Determine which simplex we are in.
            int i1, j1; // Offsets for second (middle) corner of simplex in (i,j) coords
            if (x0 > y0) { i1 = 1; j1 = 0; } // lower triangle, XY order: (0,0)->(1,0)->(1,1)
            else { i1 = 0; j1 = 1; }      // upper triangle, YX order: (0,0)->(0,1)->(1,1)

            // A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
            // a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
            // c = (3-sqrt(3))/6

            float x1 = x0 - i1 + G2; // Offsets for middle corner in (x,y) unskewed coords
            float y1 = y0 - j1 + G2;
            float x2 = x0 - 1.0f + 2.0f * G2; // Offsets for last corner in (x,y) unskewed coords
            float y2 = y0 - 1.0f + 2.0f * G2;

            // Wrap the integer indices at 256, to avoid indexing perm[] out of bounds
            int ii = Mod(i, 256);
            int jj = Mod(j, 256);

            // Calculate the contribution from the three corners
            float t0 = 0.5f - x0 * x0 - y0 * y0;
            if (t0 < 0.0f) n0 = 0.0f;
            else
            {
                t0 *= t0;
                n0 = t0 * t0 * grad(perm[ii + perm[jj]], x0, y0);
            }

            float t1 = 0.5f - x1 * x1 - y1 * y1;
            if (t1 < 0.0f) n1 = 0.0f;
            else
            {
                t1 *= t1;
                n1 = t1 * t1 * grad(perm[ii + i1 + perm[jj + j1]], x1, y1);
            }

            float t2 = 0.5f - x2 * x2 - y2 * y2;
            if (t2 < 0.0f) n2 = 0.0f;
            else
            {
                t2 *= t2;
                n2 = t2 * t2 * grad(perm[ii + 1 + perm[jj + 1]], x2, y2);
            }

            // Add contributions from each corner to get the final noise value.
            // The result is scaled to return values in the interval [-1,1].
            return 40.0f * (n0 + n1 + n2); // TODO: The scale factor is preliminary!
        }

        internal static float Generate(float x, float y, float z)
        {
            // Simple skewing factors for the 3D case
            const float F3 = 0.333333333f;
            const float G3 = 0.166666667f;

            float n0, n1, n2, n3; // Noise contributions from the four corners

            // Skew the input space to determine which simplex cell we're in
            float s = (x + y + z) * F3; // Very nice and simple skew factor for 3D
            float xs = x + s;
            float ys = y + s;
            float zs = z + s;
            int i = FastFloor(xs);
            int j = FastFloor(ys);
            int k = FastFloor(zs);

            float t = (float)(i + j + k) * G3;
            float X0 = i - t; // Unskew the cell origin back to (x,y,z) space
            float Y0 = j - t;
            float Z0 = k - t;
            float x0 = x - X0; // The x,y,z distances from the cell origin
            float y0 = y - Y0;
            float z0 = z - Z0;

            // For the 3D case, the simplex shape is a slightly irregular tetrahedron.
            // Determine which simplex we are in.
            int i1, j1, k1; // Offsets for second corner of simplex in (i,j,k) coords
            int i2, j2, k2; // Offsets for third corner of simplex in (i,j,k) coords

            /* This code would benefit from a backport from the GLSL version! */
            if (x0 >= y0)
            {
                if (y0 >= z0)
                { i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 1; k2 = 0; } // X Y Z order
                else if (x0 >= z0) { i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 0; k2 = 1; } // X Z Y order
                else { i1 = 0; j1 = 0; k1 = 1; i2 = 1; j2 = 0; k2 = 1; } // Z X Y order
            }
            else
            { // x0<y0
                if (y0 < z0) { i1 = 0; j1 = 0; k1 = 1; i2 = 0; j2 = 1; k2 = 1; } // Z Y X order
                else if (x0 < z0) { i1 = 0; j1 = 1; k1 = 0; i2 = 0; j2 = 1; k2 = 1; } // Y Z X order
                else { i1 = 0; j1 = 1; k1 = 0; i2 = 1; j2 = 1; k2 = 0; } // Y X Z order
            }

            // A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z),
            // a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z), and
            // a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z), where
            // c = 1/6.

            float x1 = x0 - i1 + G3; // Offsets for second corner in (x,y,z) coords
            float y1 = y0 - j1 + G3;
            float z1 = z0 - k1 + G3;
            float x2 = x0 - i2 + 2.0f * G3; // Offsets for third corner in (x,y,z) coords
            float y2 = y0 - j2 + 2.0f * G3;
            float z2 = z0 - k2 + 2.0f * G3;
            float x3 = x0 - 1.0f + 3.0f * G3; // Offsets for last corner in (x,y,z) coords
            float y3 = y0 - 1.0f + 3.0f * G3;
            float z3 = z0 - 1.0f + 3.0f * G3;

            // Wrap the integer indices at 256, to avoid indexing perm[] out of bounds
            int ii = Mod(i, 256);
            int jj = Mod(j, 256);
            int kk = Mod(k, 256);

            // Calculate the contribution from the four corners
            float t0 = 0.6f - x0 * x0 - y0 * y0 - z0 * z0;
            if (t0 < 0.0f) n0 = 0.0f;
            else
            {
                t0 *= t0;
                n0 = t0 * t0 * grad(perm[ii + perm[jj + perm[kk]]], x0, y0, z0);
            }

            float t1 = 0.6f - x1 * x1 - y1 * y1 - z1 * z1;
            if (t1 < 0.0f) n1 = 0.0f;
            else
            {
                t1 *= t1;
                n1 = t1 * t1 * grad(perm[ii + i1 + perm[jj + j1 + perm[kk + k1]]], x1, y1, z1);
            }

            float t2 = 0.6f - x2 * x2 - y2 * y2 - z2 * z2;
            if (t2 < 0.0f) n2 = 0.0f;
            else
            {
                t2 *= t2;
                n2 = t2 * t2 * grad(perm[ii + i2 + perm[jj + j2 + perm[kk + k2]]], x2, y2, z2);
            }

            float t3 = 0.6f - x3 * x3 - y3 * y3 - z3 * z3;
            if (t3 < 0.0f) n3 = 0.0f;
            else
            {
                t3 *= t3;
                n3 = t3 * t3 * grad(perm[ii + 1 + perm[jj + 1 + perm[kk + 1]]], x3, y3, z3);
            }

            // Add contributions from each corner to get the final noise value.
            // The result is scaled to stay just inside [-1,1]
            return 32.0f * (n0 + n1 + n2 + n3); // TODO: The scale factor is preliminary!
        }

        private static byte[] perm;

        private static readonly byte[] permOriginal = new byte[]
        {
            151,160,137,91,90,15,
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
            190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
            129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
            49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,
            151,160,137,91,90,15,
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
            190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
            129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
            49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
        };

        private static int FastFloor(float x)
        {
            return (x > 0) ? ((int)x) : (((int)x) - 1);
        }

        private static int Mod(int x, int m)
        {
            int a = x % m;
            return a < 0 ? a + m : a;
        }

        private static float grad(int hash, float x)
        {
            int h = hash & 15;
            float grad = 1.0f + (h & 7);   // Gradient value 1.0, 2.0, ..., 8.0
            if ((h & 8) != 0) grad = -grad;         // Set a random sign for the gradient
            return (grad * x);           // Multiply the gradient with the distance
        }

        private static float grad(int hash, float x, float y)
        {
            int h = hash & 7;      // Convert low 3 bits of hash code
            float u = h < 4 ? x : y;  // into 8 simple gradient directions,
            float v = h < 4 ? y : x;  // and compute the dot product with (x,y).
            return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -2.0f * v : 2.0f * v);
        }

        private static float grad(int hash, float x, float y, float z)
        {
            int h = hash & 15;     // Convert low 4 bits of hash code into 12 simple
            float u = h < 8 ? x : y; // gradient directions, and compute dot product.
            float v = h < 4 ? y : h == 12 || h == 14 ? x : z; // Fix repeats at h = 12 to 15
            return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -v : v);
        }

        private static float grad(int hash, float x, float y, float z, float t)
        {
            int h = hash & 31;      // Convert low 5 bits of hash code into 32 simple
            float u = h < 24 ? x : y; // gradient directions, and compute dot product.
            float v = h < 16 ? y : z;
            float w = h < 8 ? z : t;
            return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -v : v) + ((h & 4) != 0 ? -w : w);
        }
    }
}

public enum direction
{
    up,
    down,
    left,
    right
}