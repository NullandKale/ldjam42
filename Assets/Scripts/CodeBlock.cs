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

    public virtual void OnShot(params System.Object[] args)
    {
    }

    public virtual void OnHit(params System.Object[] args)
    {
    }

    public virtual void OnHeal(params System.Object[] args)
    {
    }

    public virtual void OnEnemyHit(params System.Object[] args)
    {
    }

    public virtual void OnEnemyKilled(params System.Object[] args)
    {
    }

    public virtual void OnDie(params System.Object[] args)
    {
    }
}