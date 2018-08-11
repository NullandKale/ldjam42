using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level currentLevel;
    public int tileCount = 50;
    public int pixelsPerTile = 20;
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
                if(isColliding(i, j))
                {
                    int xPos = (int)utils.Remap(i, 0, gen.tiles.GetLength(0), r.bounds.min.x, r.bounds.max.x);
                    int yPos = (int)utils.Remap(j, 0, gen.tiles.GetLength(1), r.bounds.min.y, r.bounds.max.y);
                }
            }
        }
    }
}