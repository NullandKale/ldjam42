using System;
using System.Collections.Generic;
using System.Linq;
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
        if (block != null)
        {
            block.Init();
            var textobj = Instantiate(Level.currentLevel.popupPrefab, transform.position, Quaternion.identity);
            var text = textobj.GetComponent<DestroyAfterDelay>().text;
            text.color = Color.blue;
            text.text = "+" + block.GetName();
            CodeBlocks.Add(block);
        }
    }

    public GameObject BulletSpawn;

    public Image KBImage;
    public Text KBText;
    public Text UpgradeText;

    private float MaxKB = 100;
    private float KB = 100;

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
        UpgradeText = GameObject.Find("Blocks").GetComponent<Text>();
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

    private string[] codeDefault =
    {
        "null",
        "null",
        "null"
    };

    private void RenderUI()
    {
        KBImage.fillAmount = KB / MaxKB;
        KBText.text = KB + " / " + MaxKB + "KB";
        var Shot = new string[3];
        Array.Copy(codeDefault, Shot, 3);
        var Hit = new string[3];
        Array.Copy(codeDefault, Hit, 3);
        var Heal = new string[3];
        Array.Copy(codeDefault, Heal, 3);
        var EnemyHit = new string[3];
        Array.Copy(codeDefault, EnemyHit, 3);
        var EnemyKilled = new string[3];
        Array.Copy(codeDefault, EnemyKilled, 3);
        var Die = new string[3];
        Array.Copy(codeDefault, Die, 3);

        for (var i = 0; i < CodeBlocks.Count; i++)
        {
            switch (CodeBlocks[i].GetOnX())
            {
                case OnX.OnShoot:
                    for (var j = 0; j < 3; j++)
                    {
                        if (Shot[j] == "null")
                        {
                            Shot[j] = CodeBlocks[i].GetName();
                            break;
                        }
                    }

                    break;

                case OnX.OnHit:
                    for (var j = 0; j < 3; j++)
                    {
                        if (Hit[j] == "null")
                        {
                            Hit[j] = CodeBlocks[i].GetName();
                            break;
                        }
                    }
                    break;

                case OnX.OnHeal:
                    for (var j = 0; j < 3; j++)
                    {
                        if (Heal[j] == "null")
                        {
                            Heal[j] = CodeBlocks[i].GetName();
                            break;
                        }
                    }
                    break;

                case OnX.OnEnemyHit:
                    for (var j = 0; j < 3; j++)
                    {
                        if (EnemyHit[j] == "null")
                        {
                            EnemyHit[j] = CodeBlocks[i].GetName();
                            break;
                        }
                    }
                    break;

                case OnX.OnEnemyKilled:
                    for (var j = 0; j < 3; j++)
                    {
                        if (EnemyKilled[j] == "null")
                        {
                            EnemyKilled[j] = CodeBlocks[i].GetName();
                            break;
                        }
                    }
                    break;

                case OnX.OnDie:
                    for (var j = 0; j < 3; j++)
                    {
                        if (Die[j] == "null")
                        {
                            Die[j] = CodeBlocks[i].GetName();
                            break;
                        }
                    }
                    break;
            }
        }

        UpgradeText.text = "OnShot( " + Shot[0] + ", " + Shot[1] + ", " + Shot[2] + " )\n"
                            + "OnHit( " + Hit[0] + ", " + Hit[1] + ", " + Hit[2] + " )\n"
                            + "OnHeal( " + Heal[0] + ", " + Heal[1] + ", " + Heal[2] + " )\n"
                            + "OnEnemyHit( " + EnemyHit[0] + ", " + EnemyHit[1] + ", " + EnemyHit[2] + " )\n"
                            + "OnEnemyKilled( " + EnemyKilled[0] + ", " + EnemyKilled[1] + ", " + EnemyKilled[2] + " )\n"
                            + "OnDie( " + Die[0] + ", " + Die[1] + ", " + Die[2] + " )";
    }

    public bool CanPickUp(OnX x)
    {
        var sum = 0;
        for (var i = 0; i < CodeBlocks.Count; i++)
        {
            if (CodeBlocks[i].GetOnX() == x)
            {
                sum++;
                if (sum >= 3)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void Update()
    {
        Movement();
        SetRotation();
        RenderUI();

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
            var proj = Instantiate(Projectile, BulletSpawn.transform.position, trans.rotation);
            proj.GetComponent<Projectile>().shooter = gameObject;
            OnShoot.Invoke(proj, true);
            currentFireRate = 0;
        }
        else
        {
            currentFireRate += 1;
        }
    }

    public void Damage(Projectile proj, float amount)
    {
        var textobj = Instantiate(Level.currentLevel.popupPrefab, transform.position, Quaternion.identity);
        var text = textobj.GetComponent<DestroyAfterDelay>().text;
        text.color = Color.red;
        text.text = "-" + amount + "kB";

        KB -= amount;

        if (proj != null)
        {
            OnHit.Invoke(proj);
        }
    }

    public void Heal(float amount, bool callEvent = true)
    {
        var textobj = Instantiate(Level.currentLevel.popupPrefab, transform.position, Quaternion.identity);
        var text = textobj.GetComponent<DestroyAfterDelay>().text;
        text.color = Color.green;
        text.text = "+" + amount + "kB";

        KB += amount;

        if (callEvent)
        {
            OnHeal.Invoke(amount);
        }
    }
}

public sealed class RemoteEvent
{
    public delegate void OnEvent(params object[] args);

    public event OnEvent Event;

    public void Invoke(params object[] args)
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