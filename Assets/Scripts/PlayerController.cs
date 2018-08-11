using UnityEngine;
using UnityEngine.UI;

public sealed class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    public static PlayerController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerController>();
            }

            return instance;
        }
    }

    public Image KBImage;
    public Text KBText;

    public Weapon Weapon;

    public float MaxKB = 100;
    public float KB = 100;

    public float Speed = 3f;

    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private Vector2 DirectionInput(KeyCode key, Vector2 vel)
    {
        return Input.GetKey(key) ? vel : Vector2.zero;
    }

    private void Movement()
    {
        var direction = Vector2.zero;
        direction += DirectionInput(KeyCode.W, Vector2.up);
        direction += DirectionInput(KeyCode.S, Vector2.down);
        direction += DirectionInput(KeyCode.A, Vector2.left);
        direction += DirectionInput(KeyCode.D, Vector2.right);

        if (direction != Vector2.zero)
        {
            rigidBody.velocity = Speed * direction.normalized;
        }
        else
        {
            rigidBody.velocity = Vector2.zero;
        }
    }

    private void SetRotation()
    {
        rigidBody.rotation = Mathf.Rad2Deg *
                              Mathf.Atan2(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y,
                                  Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x);
    }

    private void Update()
    {
        Movement();
        SetRotation();

        KBImage.fillAmount = KB / MaxKB;
        KBText.text = KB + " / " + MaxKB + "KB";
        if (Input.GetMouseButton(0))
        {
            Weapon.Shoot(transform);
        }
    }
}