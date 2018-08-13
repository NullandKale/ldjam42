using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float KB = 3;

    public float Speed = 3f;

    private Rigidbody2D rigidBody;

    public GameObject Projectile;

    public float FireRate = 1.0f;
    public float currentFireRate;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Movement()
    {
        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) > 2)
        {
            rigidBody.velocity = Vector3.MoveTowards(transform.position, PlayerController.Instance.transform.position,
                Speed * Time.fixedDeltaTime).normalized;
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

        if (KB <= 0)
        {
            die();
        }
    }

    private void die()
    {
        spawnItem();
        PlayerController.OnEnemyKilled.Invoke(this);
        Level.currentLevel.enemies.Remove(gameObject);
        Destroy(gameObject);
    }

    public int spawnChance = 20;

    private void spawnItem()
    {
        if (utils.getIntInRange(1, 101) < spawnChance)
        {
            Instantiate(Level.currentLevel.itemPrefab, transform.position, Quaternion.identity);
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
            PlayerController.OnEnemyHit.Invoke(proj.gameObject, this.gameObject);
        }
    }

    public void Shoot(Transform trans)
    {
        if (currentFireRate > FireRate)
        {
            Instantiate(Projectile, trans.position, trans.rotation).GetComponent<Projectile>().shooter = gameObject;
            currentFireRate = 0;
        }
        else
        {
            currentFireRate += 1;
        }
    }
}