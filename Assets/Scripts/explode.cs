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

    private List<GameObject> ignore = new List<GameObject>();

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 10 && !ignore.Contains(other.gameObject))
        {
            ignore.Add(other.gameObject);
            other.gameObject.GetComponent<Enemy>().Damage(null, Damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && !ignore.Contains(other.gameObject))
        {
            ignore.Add(other.gameObject);
            other.gameObject.GetComponent<Enemy>().Damage(null, Damage);
        }
    }
}