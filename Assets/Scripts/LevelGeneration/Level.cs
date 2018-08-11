using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject playerPrefab;

    public GameObject enemyPrefab;

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

    private void Awake()
    {
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

    public Room SpawnPlayer()
    {
        var toSpawnIn = gen.rooms[utils.getIntInRange(0, gen.rooms.Count)];
        var playerPos = tileToWorldPos(toSpawnIn.center.x, toSpawnIn.center.y);
        Instantiate(playerPrefab, playerPos, Quaternion.identity);
        return toSpawnIn;
    }

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
            Instantiate(enemyPrefab, playerPos, Quaternion.identity);
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
        var tex = gen.generateTexture(scale, type);
        spriteRect = new Rect(0, 0, textureSize, textureSize);
        r.sprite = Sprite.Create(tex, spriteRect, new Vector2(0.5f, 0.5f), 64, 0, SpriteMeshType.FullRect, Vector4.zero);

        for (var i = 0; i < gen.tiles.GetLength(0); i++)
        {
            for (var j = 0; j < gen.tiles.GetLength(1); j++)
            {
                if (isColliding(j, i) && gen.countNeighbors(j, i, Tile.Hole) <= 6)
                {
                    //float xPos = (utils.Remap(i, 0, gen.tiles.GetLength(0), r.bounds.min.x, r.bounds.max.x) * 1 / 2);
                    //float yPos = (utils.Remap(j, 0, gen.tiles.GetLength(1), r.bounds.min.y, r.bounds.max.y) * 1 / 2);
                    // 16           /   64               * 10 = 2.5
                    //pixelsPerTile / spritePixelDensity * transform.localScale.x;

                    //float xPos = j * 2.5f - tileCount - tileCount / 4.1975f;
                    //float yPos = i * 2.5f - tileCount - tileCount / 4.1975f;
                    float xPos = j * 2.5f - tileCount - tileCount / 4.198f;
                    float yPos = i * 2.5f - tileCount - tileCount / 4.198f;

                    GameObject toAdd = Instantiate(prefab, new Vector3(xPos, yPos), new Quaternion(), transform);
                    toAdd.AddComponent<BoxCollider2D>().size = new Vector2(0.25f, 0.25f);
                }
            }
        }
    }

    public Vector3 tileToWorldPos(int x, int y)
    {
        float xPos = x * 2.5f - tileCount - tileCount / 4.198f;
        float yPos = y * 2.5f - tileCount - tileCount / 4.198f;

        return new Vector3(xPos, yPos);
    }
}