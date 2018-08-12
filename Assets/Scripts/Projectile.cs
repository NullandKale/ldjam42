using UnityEngine;
using UnityEngine.UI;

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
            GameObject textobj = Instantiate(Level.currentLevel.popupPrefab, transform.position, Quaternion.identity);
            Text text = textobj.GetComponent<DestroyAfterDelay>().text;
            text.color = Color.red;
            text.text = "-" + Damage + "kB";

            PlayerController.Instance.KB -= Damage;
            PlayerController.OnHit.Invoke(this);
        }
        else if (other.gameObject.layer == 10)
        {
            GameObject textobj = Instantiate(Level.currentLevel.popupPrefab, transform.position, Quaternion.identity);
            Text text = textobj.GetComponent<DestroyAfterDelay>().text;
            text.color = Color.red;
            text.text = "-" + Damage + "kB";

            other.gameObject.GetComponent<Enemy>().KB -= Damage;
            PlayerController.OnEnemyHit.Invoke(this);
        }

        Destroy(gameObject);
    }
}