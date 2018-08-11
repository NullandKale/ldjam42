using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float KB = 3;

    public Weapon Weapon;

    public float Speed = 3f;

    private Rigidbody2D rigidBody;

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
        Weapon.Shoot(transform);

        if (KB <= 0)
        {
            Destroy(gameObject);
        }
    }
}