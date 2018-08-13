using UnityEngine.UI;

[System.Serializable]
public abstract class CodeBlock
{
    public abstract OnX GetOnX();

    public abstract int SpawnChance();

    public abstract string GetName();

    public void Init()
    {
        PlayerController.OnShoot += OnShot;
        PlayerController.OnHit += OnHit;
        PlayerController.OnHeal += OnHeal;
        PlayerController.OnEnemyHit += OnEnemyHit;
        PlayerController.OnEnemyKilled += OnEnemyKilled;
        PlayerController.OnDie += OnDie;
    }

    public virtual void OnShot(params object[] args)
    {
    }

    public virtual void OnHit(params object[] args)
    {
    }

    public virtual void OnHeal(params object[] args)
    {
    }

    public virtual void OnEnemyHit(params object[] args)
    {
    }

    public virtual void OnEnemyKilled(params object[] args)
    {
    }

    public virtual void OnDie(params object[] args)
    {
    }
}