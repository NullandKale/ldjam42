using System;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject explosionPrefab;
    public GameObject popupPrefab;

    public static Level currentLevel;

    [Header("Parameters")]
    public int tileCount = 50;

    private const int pixelsPerTile = 16;
    public float scale = 0.2f;
    public int roomCount = 10;
    public GenerationType type = GenerationType.Mix;
    public GameObject prefab;

    private int textureSize;
    private SpriteRenderer r;
    private LevelGenerator gen;

    public List<Sprite> EnergyTiles;
    public List<Sprite> WallTiles;
    public List<Sprite> FloorTiles;
    public List<Sprite> HoleTiles;
    public List<Sprite> DoorUpTiles;
    public List<Sprite> DoorDownTiles;

    private List<CodeBlock> codeBlocks;

    private void Awake()
    {
        utils.setSeed(UnityEngine.Random.Range(int.MinValue, int.MaxValue));
        if (currentLevel == null)
        {
            currentLevel = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        r = GetComponent<SpriteRenderer>();
        gen = new LevelGenerator(tileCount, tileCount, pixelsPerTile, roomCount);
        textureSize = tileCount * pixelsPerTile;
        addTexture();
        SpawnEnemies(SpawnPlayer());
    }

    private void initCodeBlocks()
    {
        codeBlocks = new List<CodeBlock>();
        codeBlocks.Add(new DoubleDamage());
        codeBlocks.Add(new EnemyProjectilesExplodeOnHit());
        codeBlocks.Add(new HealAgain());
        //codeBlocks.Add(new Homing());
        //codeBlocks.Add(new KnockBack());
        //codeBlocks.Add(new ReflectDamage());
        codeBlocks.Add(new TripleShot());
    }

    public CodeBlock getRandomCodeBlock()
    {
        if(codeBlocks == null)
        {
            initCodeBlocks();
        }

        if(codeBlocks.Count <= 0)
        {
            return null;
        }

        for (int i = 0; i < 100; i++)
        {
            CodeBlock temp = codeBlocks[utils.getIntInRange(0, codeBlocks.Count)];

            if(utils.getIntInRange(0,100) <= temp.spawnChance())
            {
                codeBlocks.Remove(temp);
                return temp;
            }
        }


        return null;
    }

    public Room SpawnPlayer()
    {
        var toSpawnIn = gen.rooms[utils.getIntInRange(0, gen.rooms.Count)];
        var playerPos = tileToWorldPos(toSpawnIn.center.x, toSpawnIn.center.y);
        Instantiate(playerPrefab, playerPos, Quaternion.identity);
        return toSpawnIn;
    }

    public GameObject getClosestEnemy(Vector3 pos)
    {
        int enemy = 0;
        float minDist = 1000000000;
        for(int i = 0; i < enemies.Count; i++)
        {
            float workingDist = Vector3.Distance(enemies[0].transform.position, pos);
            if(workingDist < minDist)
            {
                enemy = i;
                minDist = workingDist;
            }
        }

        return enemies[enemy];
    }

    public List<GameObject> enemies = new List<GameObject>();

    public void SpawnEnemies(Room exclude)
    {
        for (var i = 0; i < 100; i++)
        {
            var toSpawnIn = gen.rooms[utils.getIntInRange(0, gen.rooms.Count)];
            while (toSpawnIn.center.x == exclude.center.x && toSpawnIn.center.y == exclude.center.y)
            {
                toSpawnIn = gen.rooms[utils.getIntInRange(0, gen.rooms.Count)];
            }

            var playerPos = tileToWorldPos(toSpawnIn.center.x, toSpawnIn.center.y);
            enemies.Add(Instantiate(enemyPrefab, playerPos, Quaternion.identity));
        }
    }

    public Tile getTile(Vector2 pos)
    {
        if (spriteRect.Contains(pos))
        {
            int xPos = (int)utils.Remap(pos.x, r.bounds.min.x, r.bounds.max.x, 0, gen.tiles.GetLength(0));
            int yPos = (int)utils.Remap(pos.y, r.bounds.min.y, r.bounds.max.y, 0, gen.tiles.GetLength(1));

            return gen.tiles[xPos, yPos];
        }

        return Tile.Hole;
    }

    public bool isColliding(int x, int y)
    {
        Tile t = gen.tiles[x, y];

        switch (t)
        {
            case Tile.Energy:
                return true;

            case Tile.Wall:
                return true;

            case Tile.Floor:
                return false;

            case Tile.Hole:
                return true;

            default:
                return true;
        }
    }

    private Rect spriteRect;
    private void addTexture()
    {
        float multA = (float)pixelsPerTile / 64f * transform.localScale.x;
        var tex = gen.generateTexture(scale, type);
        spriteRect = new Rect(0, 0, textureSize, textureSize);
        r.sprite = Sprite.Create(tex, spriteRect, new Vector2(0.5f, 0.5f), 64, 0, SpriteMeshType.FullRect, Vector4.zero);

        for (var i = 0; i < gen.tiles.GetLength(0); i++)
        {
            for (var j = 0; j < gen.tiles.GetLength(1); j++)
            {
                if (isColliding(j, i) && gen.countNeighbors(j, i, Tile.Hole) <= 6)
                {
                    float xPos = j * multA - transform.localScale.x * 10 - transform.localScale.x * 10 / 4.2f;
                    float yPos = i * multA - transform.localScale.x * 10 - transform.localScale.x * 10 / 4.2f;
                    xPos = ((float)Math.Round(xPos * 4, MidpointRounding.ToEven)) / 4f;
                    yPos = ((float)Math.Round(yPos * 4, MidpointRounding.ToEven)) / 4f;

                    GameObject toAdd = Instantiate(prefab, new Vector3(xPos, yPos), new Quaternion(), transform);
                    toAdd.AddComponent<BoxCollider2D>().size = new Vector2(0.25f, 0.25f);
                }
            }
        }
    }

    public Vector3 tileToWorldPos(int x, int y)
    {
        float multA = pixelsPerTile / 64f * transform.localScale.x;
        float xPos = x * multA - transform.localScale.x * 10 - transform.localScale.x * 10 / 4.2f;
        float yPos = y * multA - transform.localScale.x * 10 - transform.localScale.x * 10 / 4.2f;
        xPos = ((float)Math.Round(xPos * 4, MidpointRounding.ToEven)) / 4f;
        yPos = ((float)Math.Round(yPos * 4, MidpointRounding.ToEven)) / 4f;

        return new Vector3(xPos, yPos);
    }
}