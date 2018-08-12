using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class PlayerController : MonoBehaviour
{
    public static RemoteEvent OnShoot = new RemoteEvent();
    public static RemoteEvent OnHit = new RemoteEvent();
    public static RemoteEvent OnHeal = new RemoteEvent();
    public static RemoteEvent OnEnemyHit = new RemoteEvent();
    public static RemoteEvent OnEnemyKilled = new RemoteEvent();
    public static RemoteEvent OnDie = new RemoteEvent();

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

    private readonly List<CodeBlock> CodeBlocks = new List<CodeBlock>();

    public void AddBlock(CodeBlock block)
    {
        block.Init();
        CodeBlocks.Add(block);
    }

    public Image KBImage;
    public Text KBText;

    public float MaxKB = 100;
    public float KB = 100;

    public float Speed = 3f;

    public GameObject Projectile;

    public float FireRate = 1.0f;
    private float currentFireRate;

    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        KBImage = GameObject.Find("KBImage").GetComponent<Image>();
        KBText = GameObject.Find("KBText").GetComponent<Text>();
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
            Shoot(transform);
        }

        if (KB <= 0)
        {
        }
    }

    public void Shoot(Transform trans)
    {
        if (currentFireRate > FireRate)
        {
            OnShoot.Invoke(Instantiate(Projectile, trans.position, trans.rotation));
            currentFireRate = 0;
        }
        else
        {
            currentFireRate += 1;
        }
    }

    public void Heal(int amount)
    {
        KB += amount;
        OnHeal.Invoke(amount);
    }
}

public sealed class RemoteEvent
{
    public delegate void OnEvent(params System.Object[] args);

    public event OnEvent Event;

    public void Invoke(params System.Object[] args)
    {
        if (Event != null)
        {
            Event.Invoke(args);
        }
    }

    public static RemoteEvent operator +(RemoteEvent remoteEvent, OnEvent onEvent)
    {
        remoteEvent.Event += onEvent;
        return remoteEvent;
    }

    public static RemoteEvent operator -(RemoteEvent remoteEvent, OnEvent onEvent)
    {
        remoteEvent.Event -= onEvent;
        return remoteEvent;
    }
}