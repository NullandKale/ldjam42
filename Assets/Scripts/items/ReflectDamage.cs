[System.Serializable]
public class ReflectDamage : CodeBlock
{
    public override string GetName()
    {
        return "Reflect()";
    }

    public override void OnEnemyHit(params object[] args)
    {
        var proj = (Projectile)args[0];

        if (proj.shooter != null)
        {
            if (proj.shooter != PlayerController.Instance.gameObject)
            {
                proj.shooter.GetComponent<Enemy>().Damage(null, proj.Damage * 0.1f);
            }
        }
    }

    public override OnX GetOnX()
    {
        return OnX.OnEnemyHit;
    }

    public override int SpawnChance()
    {
        return 25;
    }
}