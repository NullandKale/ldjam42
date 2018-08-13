using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour
{
    public List<Sprite> explosions;
    public float Damage;
    public float lifeTime;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = explosions[utils.getIntInRange(0, explosions.Count)];
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.gameObject.GetComponent<Enemy>().Damage(null, Damage);
        }
    }
}