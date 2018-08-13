using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public readonly List<CodeBlock> CodeBlocks = new List<CodeBlock>();

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

    public AudioSource AudioSource;
    public GameObject BulletSpawn;

    public Image KBImage;
    public Text KBText;
    public Text UpgradeText;
    public Text EnemiesLeft;
    public Image Arrow;

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
        AudioSource = GetComponent<AudioSource>();
        KBImage = GameObject.Find("KBImage").GetComponent<Image>();
        KBText = GameObject.Find("KBText").GetComponent<Text>();
        UpgradeText = GameObject.Find("Blocks").GetComponent<Text>();
        EnemiesLeft = GameObject.Find("Enemies").GetComponent<Text>();
        Arrow = GameObject.Find("Arrow").GetComponent<Image>();
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

    private float SetArrowRotation()
    {
        if (Level.currentLevel.enemies.Count > 0)
        {
            return Mathf.Rad2Deg *
                   Mathf.Atan2(
                       Level.currentLevel.GetClosestEnemy(transform.position).transform.position.y - transform.position.y,
                       Level.currentLevel.GetClosestEnemy(transform.position).transform.position.x - transform.position.x);
        }
        else
        {
            return 0;
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

        EnemiesLeft.text = "Enemies Left: " + Level.currentLevel.enemies.Count;

        Arrow.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, SetArrowRotation());
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

    private static bool IsApproximately(float fl1, float fl2, float distance)
    {
        return Mathf.Abs(fl1 - fl2) < distance;
    }

    private void Update()
    {
        if (IsApproximately(Time.timeScale, 1f, 0.01f) && Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
        else
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale += Time.fixedDeltaTime;
            }

            if (Time.timeScale > 1)
            {
                Time.timeScale -= Time.fixedDeltaTime;
            }
        }

        Movement();
        SetRotation();
        RenderUI();

        if (Input.GetKeyUp(KeyCode.Q))
        {
            for (int i = 0; i < CodeBlocks.Count; i++)
            {
                createItemFromCodeBlock(CodeBlocks[i]);
            }

            CodeBlocks.Clear();
        }

        if (Input.GetMouseButton(0))
        {
            Shoot(transform);
        }

        if (Time.timeScale < 1)
        {
            Time.timeScale = 1;
        }

        if (KB <= 0)
        {
            SceneManager.LoadScene("Defeat");
        }
        else if (Level.currentLevel.enemies.Count <= 0)
        {
            SceneManager.LoadScene("Victory");
        }
    }

    private Item createItemFromCodeBlock(CodeBlock b)
    {
        GameObject item = Instantiate(Level.currentLevel.itemPrefab, transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)), Quaternion.identity);
        Item toReturn = item.GetComponent<Item>();
        toReturn.Block = b;
        return toReturn;
    }

    public void Shoot(Transform trans)
    {
        if (currentFireRate > FireRate)
        {
            var proj = Instantiate(Projectile, BulletSpawn.transform.position, trans.rotation);
            OnShoot.Invoke(proj, true);
            currentFireRate = 0;

            if (!Options.Effects)
            {
                AudioSource.Play();
            }
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

        if (KB < 0)
        {
            OnDie.Invoke();
        }

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

        if (KB + amount > MaxKB)
        {
            KB = MaxKB;
        }
        else
        {
            KB += amount;
        }

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