using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class WallSplice
{
    public Sprite WallTop;
    public Sprite WallBottom;
    public Sprite WallRight;
    public Sprite WallLeft;
    public Sprite WallTopRight;
    public Sprite WallTopLeft;
    public Sprite WallBottomRight;
    public Sprite WallBottomLeft;
    public Sprite WallInsetTopRight;
    public Sprite WallInsetTopLeft;
    public Sprite WallInsetBottomRight;
    public Sprite WallInsetBottomLeft;
    public Sprite WallCenter;
}

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
                var noise = utils.normalizeMap(Simplex.Noise.Calc2D(sizeTiles.x, sizeTiles.y, scale));
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
        var tiles = new Tile[sizeTiles.x, sizeTiles.y];

        for (var x = 0; x < sizeTiles.x; x++)
        {
            for (var y = 0; y < sizeTiles.y; y++)
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

    private Sprite Aligntiles(int x, int y, WallSplice Set)
    {
        if (utils.isInRange(x, 1, tiles.GetLength(0) - 1) && utils.isInRange(y, 1, tiles.GetLength(1) - 1))
        {
            if (tiles[x - 1, y] != Tile.Hole
                && tiles[x, y - 1] != Tile.Hole)
            {
                return Set.WallBottomLeft;
            }
            else if (tiles[x + 1, y] != Tile.Hole
                && tiles[x, y - 1] != Tile.Hole)
            {
                return Set.WallBottomRight;
            }
            else if (tiles[x - 1, y] != Tile.Hole
                && tiles[x, y + 1] != Tile.Hole)
            {
                return Set.WallTopLeft;
            }
            else if (tiles[x + 1, y] != Tile.Hole
                && tiles[x, y + 1] != Tile.Hole)
            {
                return Set.WallTopRight;
            }
            else if (tiles[x + 1, y] != Tile.Hole)
            {
                return Set.WallRight;
            }
            else if (tiles[x - 1, y] != Tile.Hole)
            {
                return Set.WallLeft;
            }
            else if (tiles[x, y - 1] != Tile.Hole)
            {
                return Set.WallBottom;
            }
            else if (tiles[x, y + 1] != Tile.Hole)
            {
                return Set.WallTop;
            }
            else if (tiles[x - 1, y - 1] != Tile.Hole)
            {
                return Set.WallInsetBottomLeft;
            }
            else if (tiles[x + 1, y - 1] != Tile.Hole)
            {
                return Set.WallInsetBottomRight;
            }
            else if (tiles[x - 1, y + 1] != Tile.Hole)
            {
                return Set.WallInsetTopLeft;
            }
            else if (tiles[x + 1, y + 1] != Tile.Hole)
            {
                return Set.WallInsetTopRight;
            }
        }

        return Set.WallCenter;
    }

    private Texture2D fillTexture(Tile[,] tiles)
    {
        var toReturn = new Texture2D(sizePixels.x, sizePixels.y) { filterMode = FilterMode.Point };

        for (var x = 0; x < sizePixels.x; x++)
        {
            for (var y = 0; y < sizePixels.y; y++)
            {
                toReturn.SetPixel(x, y, getColorFromTile(tiles[x / pixelPerTile, y / pixelPerTile], new vector2(x % pixelPerTile, y % pixelPerTile),
                    new vector2(x / pixelPerTile, y / pixelPerTile)));
            }
        }

        toReturn.Apply();

        return toReturn;
    }

    private static int Rand<T>(List<T> source)
    {
        return Random.Range(0, source.Count);
    }

    private Color getColorFromTile(Tile t, vector2 pos, vector2 tilePos)
    {
        Random.InitState((tilePos.x + tilePos.y + 1) * (tilePos.x + tilePos.y / 2) + tilePos.y);

        switch (t)
        {
            case Tile.Energy:
                var e = Level.currentLevel.EnergyTiles[Rand(Level.currentLevel.EnergyTiles)];
                return e.texture.GetPixel(pos.x + (int)(e.rect.x), pos.y + (int)(e.rect.y));

            case Tile.Floor:
                var f = Level.currentLevel.FloorTiles[Rand(Level.currentLevel.FloorTiles)];
                return f.texture.GetPixel(pos.x + (int)(f.rect.x), pos.y + (int)(f.rect.y));

            case Tile.Hole:
                var h = Aligntiles(tilePos.x, tilePos.y, Level.currentLevel.HoleTiles);
                return h.texture.GetPixel(pos.x + (int)(h.rect.x), pos.y + (int)(h.rect.y));

            case Tile.Wall:
                var w = Level.currentLevel.WallTiles[Rand(Level.currentLevel.WallTiles)];
                return w.texture.GetPixel(pos.x + (int)(w.rect.x), pos.y + (int)(w.rect.y));

            case Tile.DoorUp:
                var du = Level.currentLevel.DoorUpTiles[Rand(Level.currentLevel.DoorUpTiles)];
                return du.texture.GetPixel(pos.x + (int)(du.rect.x), pos.y + (int)(du.rect.y));

            case Tile.DoorDown:
                var dd = Level.currentLevel.DoorDownTiles[Rand(Level.currentLevel.DoorDownTiles)];
                return dd.texture.GetPixel(pos.x + (int)(dd.rect.x), pos.y + (int)(dd.rect.y));
        }

        return Color.clear;
    }

    private Tile[,] generateRooms(int roomCount)
    {
        var noise = utils.CreateMap(1f, sizeTiles.x, sizeTiles.y);

        rooms = new List<Room>();

        for (var i = 0; i < roomCount; i++)
        {
            var working = Room.getValidRoom(sizeTiles.x, 4, 2);

            for (var x = -working.size.x; x < working.size.x; x++)
            {
                for (var y = -working.size.y; y < working.size.y; y++)
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

        for (var i = 0; i < 5; i++)
        {
            noise = doFloodCheck(noise);
        }

        return noisesToTiles(noise);
    }

    private void HallRemoval()
    {
        var run = true;
        while (run)
        {
            run = false;
            for (var x = 1; x < sizeTiles.x - 1; x++)
            {
                for (var y = 1; y < sizeTiles.y - 1; y++)
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
            for (var x = 0; x < sizeTiles.x; x++)
            {
                for (var y = 0; y < sizeTiles.y; y++)
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

        for (var x = 0; x < sizeTiles.x; x++)
        {
            for (var y = 0; y < sizeTiles.y; y++)
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

        var clearAreas = new List<List<vector2>>();
        var isChecked = new HashSet<vector2>();

        var firstClearArea = findFirstClearArea(map, isChecked);

        var counter = 0;

        while (firstClearArea.x != -1)
        {
            var toAdd = FloodFill(map, firstClearArea, sizeTiles.x);

            isChecked.UnionWith(toAdd);

            clearAreas.Add(toAdd);
            firstClearArea = findFirstClearArea(map, isChecked);
            counter++;

            if (counter > 10000)
            {
                break;
            }
        }

        var pathsToDraw = new List<vector4>();

        for (var i = 0; i < clearAreas.Count; i++)
        {
            var workingArea = clearAreas[i];
            var toAdd = findClosestRelatives(clearAreas, workingArea);
            if (toAdd != null)
            {
                pathsToDraw.Add(toAdd);
            }
        }

        return drawPaths(map, pathsToDraw);
    }

    private float[,] drawPaths(float[,] map, List<vector4> pathsToDraw)
    {
        for (var i = 0; i < pathsToDraw.Count; i++)
        {
            var start = pathsToDraw[i].i;
            var end = pathsToDraw[i].j;
            var current = start;

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
        var currentBestStart = new vector2(10000, 10000);
        var currentBestEnd = new vector2(10000, 10000);
        var currentBestDist = float.MaxValue;

        for (var i = 0; i < startingArea.Count; i++)
        {
            //happens for each tile in starting area
            for (var k = 0; k < wholeAreas.Count; k++)
            {
                if (startingArea == wholeAreas[k])
                {
                    continue;
                }

                for (var l = 0; l < wholeAreas[k].Count; l++)
                {
                    var test = false;

                    if (startingArea[i].x > sizeTiles.x / 2 && wholeAreas[k][l].x > sizeTiles.x / 2)
                    {
                        test = true;
                    }

                    var workingDist = Mathf.Abs(startingArea[i].squareDist(wholeAreas[k][l]));

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
        var badvector2s = new HashSet<vector2>();
        var vector2s = new HashSet<vector2>();
        var pixels = new Stack<vector2>();
        pixels.Push(pt);

        while (pixels.Count > 0)
        {
            var a = pixels.Pop();
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
        for (var x = 0; x < sizeTiles.x; x++)
        {
            for (var y = 0; y < sizeTiles.y; y++)
            {
                if (map[x, y] <= 0)
                {
                    var pos = new vector2(x, y);

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
        var toReturn = -1;
        for (var x = -1; x <= 1; x++)
        {
            for (var y = -1; y <= 1; y++)
            {
                if (utils.isInRange(xPos + x, sizeTiles.x) && utils.isInRange(yPos + y, sizeTiles.y))
                {
                    if (map[xPos + x, yPos + y] > 0)
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
        var toReturn = -1;
        for (var x = -1; x <= 1; x++)
        {
            for (var y = -1; y <= 1; y++)
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
        center = new vector2(utils.getIntInRange(3, posMax - 3), utils.getIntInRange(3, posMax - 3));
        size = utils.getCenteredVector2(new vector2(roomSizeCenter, roomSizeCenter), roomSizeChange);
    }

    public static Room getValidRoom(int posMax, int roomSizeCenter, int roomSizeChange)
    {
        var working = new Room(posMax, roomSizeCenter, roomSizeChange);
        for (var i = 0; i < 100; i++)
        {
            if (getAspectRatio(working) >= AspectRatioMin)
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
        var working = new Room(posMax, roomSizeCenter, roomSizeChange);
        for (var i = 0; i < 10; i++)
        {
            if (getAspectRatio(working) >= AspectRatioMin)
            {
                for (var j = 0; j < rooms.Count; j++)
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