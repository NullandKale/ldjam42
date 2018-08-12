using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour {

    public List<Sprite> explosions;
    public float Damage;
    public float lifeTime;
	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().sprite = explosions[utils.getIntInRange(0, explosions.Count)];
        Destroy(gameObject, lifeTime);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.gameObject.GetComponent<Enemy>().KB -= Damage;
            PlayerController.OnEnemyHit.Invoke(this);
        }
    }
}
