﻿using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float KB = 3;

    public float Speed = 3f;

    public GameObject BulletSpawn;

    private Rigidbody2D rigidBody;

    public GameObject itemPrefab;
    public GameObject Projectile;
    public AudioSource AudioSource;

    public float FireRate = 1.0f;
    private float currentFireRate;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        AudioSource = GetComponent<AudioSource>();
        FireRate += Random.Range(-1f, 1f);
    }

    private void Movement()
    {
        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) > 4)
        {
            rigidBody.MovePosition(Vector3.MoveTowards(transform.position, PlayerController.Instance.transform.position,
                Speed * Time.fixedDeltaTime));
        }
        else
        {
            rigidBody.velocity = Vector2.zero;
        }
    }

    private void SetRotation()
    {
        rigidBody.rotation = Mathf.Rad2Deg *
                             Mathf.Atan2(PlayerController.Instance.transform.position.y - transform.position.y,
                                        PlayerController.Instance.transform.position.x - transform.position.x);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < 8)
        {
            Movement();
            SetRotation();
            Shoot(transform);
        }
        else
        {
            rigidBody.velocity = Vector2.zero;
        }

        if (KB <= 0)
        {
            die();
        }
    }

    private void die()
    {
        spawnItem();
        PlayerController.OnEnemyKilled.Invoke();
        Level.currentLevel.enemies.Remove(gameObject);
        Destroy(gameObject);
    }

    public int spawnChance = 20;

    private void spawnItem()
    {
        if (utils.getIntInRange(1, 101) < spawnChance)
        {
            Instantiate(itemPrefab, transform.position, Quaternion.identity);
        }
    }

    public void Damage(Projectile proj, float amount)
    {
        var textobj = Instantiate(Level.currentLevel.popupPrefab, transform.position, Quaternion.identity);
        var text = textobj.GetComponent<DestroyAfterDelay>().text;
        text.color = Color.blue;
        text.text = "-" + amount + "kB";

        KB -= amount;

        if (proj != null)
        {
            PlayerController.OnEnemyHit.Invoke(proj);
        }
    }

    public void Shoot(Transform trans)
    {
        if (currentFireRate > FireRate)
        {
            Instantiate(Projectile, BulletSpawn.transform.position, trans.rotation).
                GetComponent<Projectile>().shooter = gameObject;
            currentFireRate = 0;
            AudioSource.pitch = Random.Range(0.4f, 0.8f);
            AudioSource.Play();
        }
        else
        {
            currentFireRate += 1;
        }
    }
}