using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour
{
    public List<Sprite> explosions;
    public AudioSource AudioSource;
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
            if (!Options.Effects)
            {
                AudioSource.pitch = Random.Range(0.4f, 0.8f);
                AudioSource.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && !ignore.Contains(other.gameObject))
        {
            ignore.Add(other.gameObject);
            other.gameObject.GetComponent<Enemy>().Damage(null, Damage);
            if (!Options.Effects)
            {
                AudioSource.pitch = Random.Range(0.4f, 0.8f);
                AudioSource.Play();
            }
        }
    }
}