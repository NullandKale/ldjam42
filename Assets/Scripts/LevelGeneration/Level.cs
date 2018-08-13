using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public SpriteRenderer spriteRenderer;
    private LevelGenerator gen;

    public List<Sprite> EnergyTiles;
    public List<Sprite> WallTiles;
    public List<Sprite> FloorTiles;
    public WallSplice HoleTiles;
    public List<Sprite> DoorUpTiles;
    public List<Sprite> DoorDownTiles;

    private List<CodeBlock> codeBlocks;

    private void Awake()
    {
        utils.setSeed(Random.Range(int.MinValue, int.MaxValue));
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        gen = new LevelGenerator(tileCount, tileCount, pixelsPerTile, roomCount);
        textureSize = tileCount * pixelsPerTile;
        addTexture();
        SpawnEnemies(SpawnPlayer());
    }

    private void initCodeBlocks()
    {
        codeBlocks = new List<CodeBlock>
        {
            new DoubleDamage(),
            new EnemyProjectilesExplodeOnHit(),
            new HealAgain(),
            new TripleShot(),
            new ReflectDamage(),
            //new KnockBack(),
        };
    }

    public CodeBlock GetRandomCodeBlock()
    {
        if (codeBlocks == null)
        {
            initCodeBlocks();
        }

        if (codeBlocks.Count <= 0)
        {
            return null;
        }

        for (var i = 0; i < 100; i++)
        {
            var temp = codeBlocks[utils.getIntInRange(0, codeBlocks.Count)];

            if (utils.getIntInRange(0, 100) <= temp.SpawnChance())
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

    public GameObject GetClosestEnemy(Vector3 pos)
    {
        var enemy = 0;
        var minDist = float.MaxValue;

        for (var i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                var workingDist = Vector3.Distance(enemies[i].transform.position, pos);
                if (workingDist < minDist)
                {
                    enemy = i;
                    minDist = workingDist;
                }
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

            var randomVector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            var playerPos = tileToWorldPos(toSpawnIn.center.x, toSpawnIn.center.y);
            enemies.Add(Instantiate(enemyPrefab, playerPos + randomVector, Quaternion.identity));
        }
    }

    public Tile GetTile(Vector2 pos)
    {
        if (spriteRect.Contains(pos))
        {
            var xPos = (int)utils.Remap(pos.x, spriteRenderer.bounds.min.x, spriteRenderer.bounds.max.x, 0, gen.tiles.GetLength(0));
            var yPos = (int)utils.Remap(pos.y, spriteRenderer.bounds.min.y, spriteRenderer.bounds.max.y, 0, gen.tiles.GetLength(1));

            return gen.tiles[xPos, yPos];
        }

        return Tile.Hole;
    }

    public bool isColliding(int x, int y)
    {
        var t = gen.tiles[x, y];

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
        var multA = (float)pixelsPerTile / 64f * transform.localScale.x;
        var tex = gen.generateTexture(scale, type);
        spriteRect = new Rect(0, 0, textureSize, textureSize);
        spriteRenderer.sprite = Sprite.Create(tex, spriteRect, new Vector2(0.5f, 0.5f), 64, 0, SpriteMeshType.FullRect, Vector4.zero);

        for (var i = 0; i < gen.tiles.GetLength(0); i++)
        {
            for (var j = 0; j < gen.tiles.GetLength(1); j++)
            {
                if (isColliding(j, i) && gen.countNeighbors(j, i, Tile.Hole) <= 6)
                {
                    var xPos = j * multA - transform.localScale.x * 10 - transform.localScale.x * 10 / 4.2f;
                    var yPos = i * multA - transform.localScale.x * 10 - transform.localScale.x * 10 / 4.2f;
                    xPos = ((float)Math.Round(xPos * 4, MidpointRounding.ToEven)) / 4f;
                    yPos = ((float)Math.Round(yPos * 4, MidpointRounding.ToEven)) / 4f;

                    var toAdd = Instantiate(prefab, new Vector3(xPos, yPos), new Quaternion(), transform);
                    toAdd.AddComponent<BoxCollider2D>().size = new Vector2(0.25f, 0.25f);
                }
            }
        }
    }

    public Vector3 tileToWorldPos(int x, int y)
    {
        var multA = pixelsPerTile / 64f * transform.localScale.x;
        var xPos = x * multA - transform.localScale.x * 10 - transform.localScale.x * 10 / 4.2f;
        var yPos = y * multA - transform.localScale.x * 10 - transform.localScale.x * 10 / 4.2f;
        xPos = ((float)Math.Round(xPos * 4, MidpointRounding.ToEven)) / 4f;
        yPos = ((float)Math.Round(yPos * 4, MidpointRounding.ToEven)) / 4f;

        return new Vector3(xPos, yPos);
    }

    private float h;
    private float s;
    private float v;

    public void Update()
    {
        Color.RGBToHSV(spriteRenderer.color, out h, out s, out v);

        s = 0.2f;
        h += 0.00005f;
        if (h > 1f)
        {
            h = 0f;
        }

        spriteRenderer.color = Color.HSVToRGB(h, s, v);
    }
}