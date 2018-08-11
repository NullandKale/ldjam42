public class CodeBlock
{
    public void Init()
    {
        PlayerController.OnShoot += OnShot;
        PlayerController.OnHit += OnHit;
        PlayerController.OnHeal += OnHeal;
        PlayerController.OnEnemyHit += OnEnemyHit;
        PlayerController.OnEnemyKilled += OnEnemyKilled;
        PlayerController.OnDie += OnDie;
    }

    public virtual void OnShot()
    {
    }

    public virtual void OnHit()
    {
    }

    public virtual void OnHeal()
    {
    }

    public virtual void OnEnemyHit()
    {
    }

    public virtual void OnEnemyKilled()
    {
    }

    public virtual void OnDie()
    {
    }
}