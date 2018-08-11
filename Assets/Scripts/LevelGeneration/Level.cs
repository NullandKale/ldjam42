using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level currentLevel;
    public int tileCount = 50;
    private const int pixelsPerTile = 16;
    public float scale = 0.2f;
    public int roomCount = 10;
    public GenerationType type = GenerationType.Mix;
    public GameObject colliderPrefab;

    private int textureSize;
    private SpriteRenderer r;
    private LevelGenerator gen;

    private void Awake()
    {
        if(currentLevel == null)
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
    }

    public Tile getTile(Vector2 pos)
    {
        if(spriteRect.Contains(pos))
        {
            
            int xPos = (int)utils.Remap(pos.x, r.bounds.min.x, r.bounds.max.x, 0, gen.tiles.GetLength(0));
            int yPos = (int)utils.Remap(pos.y, r.bounds.min.y, r.bounds.max.y, 0, gen.tiles.GetLength(1));

            return gen.tiles[xPos, yPos];
        }

        return Tile.Hole;
    }

    public bool isColliding(int x, int y)
    {
        Tile t = gen.tiles[x,y];

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

    Rect spriteRect;

    private void addTexture()
    {
        Texture2D tex = gen.generateTexture(scale, type);
        spriteRect = new Rect(0, 0, textureSize, textureSize);
        r.sprite = Sprite.Create(tex, spriteRect, new Vector2(0.5f, 0.5f), 64, 0, SpriteMeshType.FullRect, Vector4.zero);

        for (int i = 0; i < gen.tiles.GetLength(0); i++)
        {
            for (int j = 0; j < gen.tiles.GetLength(1); j++)
            {
                if(isColliding(j, i))
                {
                    //float xPos = (utils.Remap(i, 0, gen.tiles.GetLength(0), r.bounds.min.x, r.bounds.max.x) * 1 / 2);
                    //float yPos = (utils.Remap(j, 0, gen.tiles.GetLength(1), r.bounds.min.y, r.bounds.max.y) * 1 / 2);
                    // 16           /   64               * 10 = 2.5
                    //pixelsPerTile / spritePixelDensity * transform.localScale.x;
                    
                    //float xPos = j * 2.5f - tileCount - tileCount / 4.1975f;
                    //float yPos = i * 2.5f - tileCount - tileCount / 4.1975f;
                    float xPos = j * 2.5f - tileCount - tileCount / 4.198f;
                    float yPos = i * 2.5f - tileCount - tileCount / 4.198f;

                    GameObject toAdd = Instantiate(colliderPrefab, new Vector3(xPos, yPos), new Quaternion(), transform);
                    toAdd.AddComponent<BoxCollider2D>().size = new Vector2(0.25f, 0.25f);
                }
            }
        }
    }
}