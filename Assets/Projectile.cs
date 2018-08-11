using UnityEngine;

public class Projectile : MonoBehaviour
{
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
}