using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReflectDamage : CodeBlock
{
    public override string getName()
    {
        return "Reflect();";
    }

    public override void OnEnemyHit(params object[] args)
    {
        Projectile proj = (Projectile)args[0];
        proj.shooter.GetComponent<Enemy>().KB -= (proj.Damage * 0.1f);
    }

    public override int spawnChance()
    {
        return 25;
    }
}
