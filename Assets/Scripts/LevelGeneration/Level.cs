using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int tileCount = 50;
    public int pixelsPerTile = 20;
    public float scale = 0.2f;
    public int roomCount = 10;
    public GenerationType type = GenerationType.Mix;


    private int textureSize;
    SpriteRenderer r;
    LevelGenerator gen;

    // Use this for initialization
    void Start()
    {
        r = GetComponent<SpriteRenderer>();
        gen = new LevelGenerator(tileCount, tileCount, pixelsPerTile, roomCount);
        textureSize = tileCount * pixelsPerTile;
        addTexture();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void addTexture()
    {
        Texture2D tex = gen.generateTexture(scale, type);
        Rect re = new Rect(0, 0, textureSize, textureSize);
        r.sprite = Sprite.Create(tex, re, new Vector2(0.5f, 0.5f));
        //gameObject.AddComponent<PolygonCollider2D>();
    }
}
