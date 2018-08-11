using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator
{
    private vector2 sizePixels;
    private vector2 sizeTiles;
    private int pixelPerTile;
    private int roomCount;

    public Tile[,] tiles = null;
    public List<Room> rooms = null;

    public LevelGenerator(int heightTiles, int widthTiles, int pixelPerTile, int roomCount)
    {
        utils.setSeed(Random.Range(int.MinValue, int.MaxValue));
        sizePixels = new vector2(widthTiles * pixelPerTile, heightTiles * pixelPerTile);
        sizeTiles = new vector2(widthTiles, heightTiles);
        this.pixelPerTile = pixelPerTile;
        this.roomCount = roomCount;
    }

    public Texture2D generateTexture(float scale, GenerationType genType)
    {
        tiles = null;

        switch (genType)
        {
            case GenerationType.Room:
                tiles = generateRooms(roomCount);
                break;
            case GenerationType.Cave:
                float[,] noise = utils.normalizeMap(Simplex.Noise.Calc2D(sizeTiles.x, sizeTiles.y, scale));
                tiles = noisesToTiles(noise);
                break;
            case GenerationType.Mix:
                noise = utils.normalizeMap(Simplex.Noise.Calc2D(sizeTiles.x, sizeTiles.y, scale));
                tiles = noisesToTiles(noise);
                break;
        }

        HallRemoval();

        return fillTexture(tiles);
    }

    private Tile[,] noisesToTiles(float[,] noise)
    {
        Tile[,] tiles = new Tile[sizeTiles.x, sizeTiles.y];

        for (int x = 0; x < sizeTiles.x; x++)
        {
            for (int y = 0; y < sizeTiles.y; y++)
            {
                tiles[x, y] = noiseToTile(noise[x, y]);
            }
        }

        return tiles;
    }

    private Tile noiseToTile(float noise)
    {
        if (noise <= 0)
        {
            return Tile.Floor;
        }
        else
        {
            return Tile.Hole;
        }
    }

    private Texture2D fillTexture(Tile[,] tiles)
    {
        Texture2D toReturn = new Texture2D(sizePixels.x, sizePixels.y);
        toReturn.filterMode = FilterMode.Point;

        for (int x = 0; x < sizePixels.x; x++)
        {
            for (int y = 0; y < sizePixels.y; y++)
            {
                vector4 workingPos = vector4.getSplit(new vector2(x, y), pixelPerTile);
                toReturn.SetPixel(x, y, getColorFromTile(tiles[workingPos.i.x, workingPos.i.y], workingPos.j));
            }
        }

        toReturn.Apply();

        return toReturn;
    }

    private Color getColorFromTile(Tile t, vector2 pos)
    {
        switch (t)
        {
            case Tile.Energy:
                return Color.cyan;
            case Tile.Floor:
                return Color.grey;
            case Tile.Hole:
                return Color.black;
            case Tile.Wall:
                return Color.red;
            case Tile.DoorUp:
                return Color.yellow;
            case Tile.DoorDown:
                return Color.white;
        }

        return Color.clear;
    }

    private Tile[,] generateRooms(int roomCount)
    {
        float[,] noise = utils.CreateMap(1f, sizeTiles.x, sizeTiles.y);

        rooms = new List<Room>();

        for (int i = 0; i < roomCount; i++)
        {
            Room working = Room.getValidRoom(sizeTiles.x, 4, 2);

            for (int x = -working.size.x; x < working.size.x; x++)
            {
                for (int y = -working.size.y; y < working.size.y; y++)
                {
                    if (utils.isInRange(working.center.x - x, sizeTiles.x) && utils.isInRange(working.center.y - y, sizeTiles.y))
                    {
                        noise[working.center.x - x, working.center.y - y] = -1;
                    }
                }
            }

            rooms.Add(working);
        }

        noise = utils.addBorder(noise);

        for (int i = 0; i < 5; i++)
        {
            noise = doFloodCheck(noise);
        }

        return noisesToTiles(noise);
    }

    private void HallRemoval()
    {
        bool run = true;
        while(run)
        {
            run = false;
            for (int x = 1; x < sizeTiles.x - 1; x++)
            {
                for (int y = 1; y < sizeTiles.y - 1; y++)
                {
                    if (tiles[x, y] == Tile.Hole)
                    {
                        if (countNeighbors(x, y, Tile.Hole) <= 2)
                        {
                            tiles[x, y] = Tile.Floor;
                            run = true;
                        }
                    }
                }
            }
        }

        run = true;
        while (run)
        {
            run = false;
            for (int x = 0; x < sizeTiles.x; x++)
            {
                for (int y = 0; y < sizeTiles.y; y++)
                {
                    if (tiles[x, y] == Tile.Floor)
                    {
                        if (countNeighbors(x, y, Tile.Floor) <= 1)
                        {
                            tiles[x, y] = Tile.Hole;
                            run = true;
                        }
                    }
                }
            }
        }

        for (int x = 0; x < sizeTiles.x; x++)
        {
            for (int y = 0; y < sizeTiles.y; y++)
            {
                if (tiles[x, y] == Tile.Wall)
                {
                    tiles[x, y] = Tile.Hole;
                }
            }
        }
    }

    private float[,] doFloodCheck(float[,] map)
    {
        //find all areas
        //connect closest areas together
        //

        List<List<vector2>> clearAreas = new List<List<vector2>>();
        HashSet<vector2> isChecked = new HashSet<vector2>();

        vector2 firstClearArea = findFirstClearArea(map, isChecked);

        int counter = 0;

        while (firstClearArea.x != -1)
        {
            List<vector2> toAdd = FloodFill(map, firstClearArea, sizeTiles.x);

            isChecked.UnionWith(toAdd);

            clearAreas.Add(toAdd);
            firstClearArea = findFirstClearArea(map, isChecked);
            counter++;

            if (counter > 10000)
            {
                break;
            }
        }

        List<vector4> pathsToDraw = new List<vector4>();

        for (int i = 0; i < clearAreas.Count; i++)
        {
            List<vector2> workingArea = clearAreas[i];
            vector4 toAdd = findClosestRelatives(clearAreas, workingArea);
            if (toAdd != null)
            {
                pathsToDraw.Add(toAdd);
            }
        }

        return drawPaths(map, pathsToDraw);

    }

    private float[,] drawPaths(float[,] map, List<vector4> pathsToDraw)
    {
        for (int i = 0; i < pathsToDraw.Count; i++)
        {
            vector2 start = pathsToDraw[i].i;
            vector2 end = pathsToDraw[i].j;
            vector2 current = start;

            while ((current.x != end.x || current.y != end.y))
            {
                map[current.x, current.y] = -1;
                if (current.x > end.x)
                {
                    current.x--;
                }
                else if (current.x < end.x)
                {
                    current.x++;
                }
                else
                {
                    if (current.y > end.y)
                    {
                        current.y--;
                    }
                    else if (current.y < end.y)
                    {
                        current.y++;
                    }
                }
            }
        }

        return map;
    }

    //THIS IS BAD AND I FEEL BAD
    private vector4 findClosestRelatives(List<List<vector2>> wholeAreas, List<vector2> startingArea)
    {
        vector2 currentBestStart = new vector2(10000, 10000);
        vector2 currentBestEnd = new vector2(10000, 10000);
        float currentBestDist = float.MaxValue;

        for (int i = 0; i < startingArea.Count; i++)
        {
            //happens for each tile in starting area
            for (int k = 0; k < wholeAreas.Count; k++)
            {
                if (startingArea == wholeAreas[k])
                {
                    continue;
                }

                for (int l = 0; l < wholeAreas[k].Count; l++)
                {
                    bool test = false;

                    if (startingArea[i].x > sizeTiles.x / 2 && wholeAreas[k][l].x > sizeTiles.x / 2)
                    {
                        test = true;
                    }

                    float workingDist = Mathf.Abs(startingArea[i].squareDist(wholeAreas[k][l]));

                    if (workingDist < currentBestDist)
                    {
                        if (test)
                        {
                            //Debug.Log("FUCK");
                        }
                        currentBestDist = workingDist;
                        currentBestEnd = wholeAreas[k][l];
                        currentBestStart = startingArea[i];
                    }
                }
            }
        }

        return new vector4(currentBestStart, currentBestEnd);
    }

    public static List<vector2> FloodFill(float[,] map, vector2 pt, int worldSize)
    {
        HashSet<vector2> badvector2s = new HashSet<vector2>();
        HashSet<vector2> vector2s = new HashSet<vector2>();
        Stack<vector2> pixels = new Stack<vector2>();
        pixels.Push(pt);

        while (pixels.Count > 0)
        {
            vector2 a = pixels.Pop();
            if (a.x < worldSize - 1 && a.x > 0 && a.y < worldSize - 1 && a.y > 0)
            {
                // make sure we stay within bounds
                if (map[a.x, a.y] <= 0)
                {
                    if (!vector2s.Contains(new vector2(a.x, a.y)))
                    {
                        vector2s.Add(new vector2(a.x, a.y));
                    }

                    if (!vector2s.Contains(new vector2(a.x - 1, a.y)) && map[a.x - 1, a.y] <= 0
                        && !pixels.Contains(new vector2(a.x - 1, a.y)) && !badvector2s.Contains(new vector2(a.x - 1, a.y)))
                    {
                        pixels.Push(new vector2(a.x - 1, a.y));
                        badvector2s.Add(new vector2(a.x - 1, a.y));
                    }

                    if (!vector2s.Contains(new vector2(a.x + 1, a.y)) && map[a.x + 1, a.y] <= 0
                        && !pixels.Contains(new vector2(a.x + 1, a.y)) && !badvector2s.Contains(new vector2(a.x + 1, a.y)))
                    {
                        pixels.Push(new vector2(a.x + 1, a.y));
                        badvector2s.Add(new vector2(a.x + 1, a.y));
                    }

                    if (!vector2s.Contains(new vector2(a.x, a.y - 1)) && map[a.x, a.y - 1] <= 0
                        && !pixels.Contains(new vector2(a.x, a.y - 1)) && !badvector2s.Contains(new vector2(a.x, a.y - 1)))
                    {
                        pixels.Push(new vector2(a.x, a.y - 1));
                        badvector2s.Add(new vector2(a.x, a.y - 1));
                    }

                    if (!vector2s.Contains(new vector2(a.x, a.y + 1)) && map[a.x, a.y + 1] <= 0
                        && !pixels.Contains(new vector2(a.x, a.y + 1)) && !badvector2s.Contains(new vector2(a.x, a.y + 1)))
                    {
                        pixels.Push(new vector2(a.x, a.y + 1));
                        badvector2s.Add(new vector2(a.x, a.y + 1));
                    }
                }
                else
                {
                    badvector2s.Add(a);
                }
            }
        }

        return vector2s.ToList();
    }

    private vector2 findFirstClearArea(float[,] map, HashSet<vector2> clearAreas)
    {
        for (int x = 0; x < sizeTiles.x; x++)
        {
            for (int y = 0; y < sizeTiles.y; y++)
            {
                if (map[x, y] <= 0)
                {
                    vector2 pos = new vector2(x, y);

                    if (!isAlreadyFound(pos, clearAreas))
                    {
                        return new vector2(x, y);
                    }
                }
            }
        }

        return new vector2(-1, -1);
    }

    private bool isAlreadyFound(vector2 toCheck, HashSet<vector2> clearAreas)
    {
        if (clearAreas.Contains(toCheck))
        {
            return true;
        }

        return false;
    }

    private int countNeighbors(float[,] map, int xPos, int yPos)
    {
        int toReturn = -1;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (utils.isInRange(xPos + x, sizeTiles.x) && utils.isInRange(yPos + y, sizeTiles.y))
                {
                    if(map[xPos + x, yPos + y] > 0)
                    {
                        toReturn++;
                    }
                }
            }
        }

        return toReturn;
    }

    public int countNeighbors(int xPos, int yPos, Tile toFind)
    {
        int toReturn = -1;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (utils.isInRange(xPos + x, sizeTiles.x) && utils.isInRange(yPos + y, sizeTiles.y))
                {
                    if (tiles[xPos + x, yPos + y] == toFind)
                    {
                        toReturn++;
                    }
                }
            }
        }

        return toReturn;
    }
}

public enum Tile
{
    Energy,
    Wall,
    Floor,
    Hole,
    DoorUp,
    DoorDown,
}

public enum GenerationType
{
    Room,
    Cave,
    Mix
}

public struct Room
{
    public const float AspectRatioMin = 0.5f;
    public vector2 center;
    public vector2 size;

    public Room(vector2 center, vector2 size)
    {
        this.center = center;
        this.size = size;
    }

    public Room(int posMax, int roomSizeCenter, int roomSizeChange)
    {
        center = utils.getCenteredVector2(new vector2(0, 0), posMax - 2);
        if(center.x <= 2)
        {
            center.x++;
        }
        if (center.y <= 2)
        {
            center.y++;
        }

        size = utils.getCenteredVector2(new vector2(roomSizeCenter, roomSizeCenter), roomSizeChange);
    }

    public static Room getValidRoom(int posMax, int roomSizeCenter, int roomSizeChange)
    {
        Room working = new Room(posMax, roomSizeCenter, roomSizeChange);
        for(int i = 0; i < 100; i++)
        {
            if(getAspectRatio(working) >= AspectRatioMin)
            {
                return working;
            }
            else
            {
                working = new Room(posMax, roomSizeCenter, roomSizeChange);
            }
        }

        return working;
    }

    public static Room getValidRoomMinDist(int posMax, int roomSizeCenter, int roomSizeChange, float minSquareDist, List<Room> rooms)
    {
        Room working = new Room(posMax, roomSizeCenter, roomSizeChange);
        for (int i = 0; i < 10; i++)
        {
            if (getAspectRatio(working) >= AspectRatioMin)
            {
                for(int j = 0; j < rooms.Count; j++)
                {
                    if (squareDist(rooms[i], working) > minSquareDist)
                    {

                    }
                    else
                    {
                        break;
                    }
                }
                return working;
            }
            else
            {
                working = new Room(posMax, roomSizeCenter, roomSizeChange);
            }
        }

        return working;
    }

    public static float squareDist(Room a, Room b)
    {
        return a.center.squareDist(b.center);
    }

    public static float getAspectRatio(Room a)
    {
        return a.size.x / a.size.y;
    }
}