﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator
{
    private vector2 sizePixels;
    private vector2 sizeTiles;
    private int pixelPerTile;
    private int roomCount;

    public LevelGenerator(int heightTiles, int widthTiles, int pixelPerTile, int roomCount)
    {
        sizePixels = new vector2(widthTiles * pixelPerTile, heightTiles * pixelPerTile);
        sizeTiles = new vector2(widthTiles, heightTiles);
        this.pixelPerTile = pixelPerTile;
        this.roomCount = roomCount;
    }

    public Texture2D generateTexture(float scale, GenerationType genType)
    {
        Tile[,] tiles = null;

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
        return fillTexture(tiles);
    }

    private Tile[,] generateRooms(int roomCount)
    {
        float[,] noise = utils.CreateMap(1f, sizeTiles.x, sizeTiles.y);

        for (int i = 0; i < roomCount; i++)
        {
            vector2 center = utils.getCenteredVector2(new vector2(0, 0), sizeTiles.x);
            vector2 size = utils.getCenteredVector2(new vector2(4, 4), 2);

            for (int x = -size.x; x < size.x; x++)
            {
                for (int y = -size.y; y < size.y; y++)
                {
                    if(utils.isInRange(center.x - x, sizeTiles.x) && utils.isInRange(center.y - y, sizeTiles.y))
                    {
                        noise[center.x - x, center.y - y] = -1;
                    }
                }
            }
        }

        for (int x = 0; x < sizeTiles.x; x++)
        {
            for (int y = 0; y < sizeTiles.y; y++)
            {
                if (noise[x, y] > 0.01)
                {
                    if (countNeighbors(noise, x, y) < 3)
                    {
                        noise[x, y] = -1;
                    }
                }
            }
        }

        noise = utils.addBorder(noise);

        for(int i = 0; i < 5; i++)
        {
            noise = doFloodCheck(noise);
        }

        return noisesToTiles(noise);
    }

    private float[,] doFloodCheck(float[,] map)
    {
        //find all areas
        //connect closest areas together
        //

        List<List<vector2>> clearAreas = new List<List<vector2>>();

        vector2 firstClearArea = findFirstClearArea(map, clearAreas);

        int counter = 0;

        while(firstClearArea.x != -1)
        {
            clearAreas.Add(FloodFill(map, firstClearArea, sizeTiles.x));
            firstClearArea = findFirstClearArea(map, clearAreas);
            counter++;

            if(counter > 10000)
            {
                Debug.Log("Room Generation Error");
                break;
            }
        }

        List<vector4> pathsToDraw = new List<vector4>();

        for(int i = 0; i < clearAreas.Count; i++)
        {
            List<vector2> workingArea = clearAreas[i];
            vector4 toAdd = findClosestRelatives(clearAreas, workingArea, map);
            if(toAdd != null)
            {
                pathsToDraw.Add(toAdd);
            }
        }

        return drawPaths(map, pathsToDraw);

    }

    private float[,] drawPaths(float[,] map, List<vector4> pathsToDraw)
    {
        for(int i = 0; i < pathsToDraw.Count; i++)
        {
            vector2 start = pathsToDraw[i].i;
            vector2 end = pathsToDraw[i].j;
            vector2 current = start;

            if(start.x > sizeTiles.x / 2 || end.x > sizeTiles.x / 2)
            {
                Debug.Log("FUCKIGN WORK");
            }

            while((current.x != end.x || current.y != end.y))
            {
                map[current.x, current.y] = -1;
                if(current.x > end.x)
                {
                    current.x--;
                }
                else if(current.x < end.x)
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
    private vector4 findClosestRelatives(List<List<vector2>> wholeAreas, List<vector2> startingArea, float[,] map)
    {
        vector2 currentBestStart = new vector2(10000, 10000);
        vector2 currentBestEnd = new vector2(10000, 10000);
        float currentBestDist = float.MaxValue;

        for (int i = 0; i < startingArea.Count; i++)
        {
            map[startingArea[i].x, startingArea[i].y] = -3;
            //happens for each tile in starting area
            for (int k = 0; k < wholeAreas.Count; k++)
            {
                if (startingArea == wholeAreas[k])
                {
                    continue;
                }

                //if (startingArea[0].x == wholeAreas[k][0].x && startingArea[0].y == wholeAreas[k][0].y)
                //{
                //    return null;
                //}


                for (int l = 0; l < wholeAreas[k].Count; l++)
                {
                    bool test = false;

                    if(startingArea[i].x > sizeTiles.x / 2 && wholeAreas[k][l].x > sizeTiles.x / 2)
                    {
                        test = true;
                    }
                    if (Mathf.Abs(startingArea[i].dist(wholeAreas[k][l])) < currentBestDist)
                    {
                        if(test)
                        {
                            //Debug.Log("FUCK");
                        }
                        currentBestDist = Mathf.Abs(startingArea[i].dist(wholeAreas[k][l]));
                        currentBestEnd = wholeAreas[k][l];
                        currentBestStart = startingArea[i];
                    }
                }
            }
        }

        if (currentBestStart.x > sizeTiles.x / 2 || currentBestEnd.x > sizeTiles.x / 2)
        {
            Debug.Log("FUCK");
        }

        return new vector4(currentBestStart, currentBestEnd);
    }

    public static List<vector2> FloodFill(float[,] map, vector2 pt, int worldSize)
    {
        List<vector2> badvector2s = new List<vector2>();
        List<vector2> vector2s = new List<vector2>();
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
                        map[a.x, a.y] = -2;
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

        return vector2s;
    }

    private vector2 findFirstClearArea(float[,] map, List<List<vector2>> clearAreas)
    {
        for (int x = 0; x < sizeTiles.x; x++)
        {
            for (int y = 0; y < sizeTiles.y; y++)
            {
                if(map[x,y] <= 0)
                {
                    vector2 pos = new vector2(x, y);

                    if(!isAlreadyFound(pos, clearAreas))
                    {
                        return new vector2(x, y);
                    }
                }
            }
        }

        return new vector2(-1,-1);
    }

    private bool isAlreadyFound(vector2 toCheck, List<List<vector2>> clearAreas)
    {
        for(int i = 0; i < clearAreas.Count; i++)
        {
            if(clearAreas[i].Contains(toCheck))
            {
                return true;
            }
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
        if(noise > 0.5)
        {
            return Tile.Floor;
        }
        else if(noise < -1 && noise >= -2)
        {
            return Tile.Energy;
        }
        else if (noise < -2)
        {
            return Tile.Wall;
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

        for(int x = 0; x < sizePixels.x; x++)
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
                return Color.black;
            case Tile.Hole:
                return Color.clear;
            case Tile.Wall:
                return Color.red;
        }

        return Color.clear;
    }
}

public enum Tile
{
    Energy,
    Wall,
    Floor,
    Hole,
}

public enum GenerationType
{
    Room,
    Cave,
    Mix
}
