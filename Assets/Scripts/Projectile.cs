using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject shooter;
    public float Damage = 1f;
    public float Life = 3f;
    public float Speed = 1f;

    private Rigidbody2D rigidBody;

    private Vector2 SetRotation()
    {
        return new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x,
                                 Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y);
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

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
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
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