using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float KB = 3;

    public float Speed = 3f;

    private Rigidbody2D rigidBody;

    public GameObject Projectile;

    public float FireRate = 1.0f;
    private float currentFireRate;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Movement()
    {
        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) > 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, PlayerController.Instance.transform.position,
                Speed * Time.fixedDeltaTime);
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
        Movement();
        SetRotation();
        Shoot(transform);

        if (KB <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Shoot(Transform trans)
    {
        if (currentFireRate > FireRate)
        {
            Instantiate(Projectile, trans.position, trans.rotation);
            currentFireRate = 0;
        }
        else
        {
            currentFireRate += 1;
        }
    }
}