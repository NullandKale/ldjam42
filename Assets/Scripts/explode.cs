using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            GameObject textobj = Instantiate(Level.currentLevel.popupPrefab, transform.position, Quaternion.identity);
            Text text = textobj.GetComponent<DestroyAfterDelay>().text;
            text.color = Color.red;
            text.text = "-" + Damage + "kB";

            other.gameObject.GetComponent<Enemy>().KB -= Damage;
            PlayerController.OnEnemyHit.Invoke(this);
        }
    }
}
