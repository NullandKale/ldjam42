using System.Collections;
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
            vector2 size = utils.getCenteredVector2(new vector2(3, 3), 2);

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
                        noise[x, y] = 0;
                    }
                }
            }
        }

        return noisesToTiles(noise);
    }

    private void doFloodCheck()
    {
        //find all areas
        //connect closest areas together
        //
    }

    private vector2 findFirstClearArea(float[,] map)
    {
        for (int x = 0; x < sizeTiles.x; x++)
        {
            for (int y = 0; y < sizeTiles.y; y++)
            {
                if(map[x,y] == 0)
                {
                    return new vector2(x, y);
                }
            }
        }

        return new vector2(-1, -1);
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
                return Color.gray;
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
