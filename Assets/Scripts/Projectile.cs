using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject shooter;
    public float Damage = 1f;
    public float Life = 3f;
    public float Speed = 1f;

    public float noCollideTime = 0;

    private Rigidbody2D rigidBody;
    public CircleCollider2D c;

    private Vector2 SetRotation()
    {
        return new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x,
                                 Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y);
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        c = GetComponent<CircleCollider2D>();

        transform.Rotate(transform.right);
        rigidBody.AddForce(transform.right * 1000 * Speed * Time.fixedDeltaTime, ForceMode2D.Force);
    }

    private void Update()
    {
        Life -= Time.fixedDeltaTime;
        if (Life <= 0f)
        {
            Destroy(gameObject);
        }

        if(noCollideTime > 0)
        {
            noCollideTime -= Time.deltaTime;
        }
        else
        {
            c.enabled = true;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(noCollideTime > 0)
        {
            return;
        }

        if (other.gameObject.layer == 8)
        {
            PlayerController.Instance.Damage(this, Damage);
        }
        else if (other.gameObject.layer == 10)
        {
            other.gameObject.GetComponent<Enemy>().Damage(this, Damage);
        }

        if (other.gameObject.layer != 9)
        {
            Destroy(gameObject);
        }
    }
}