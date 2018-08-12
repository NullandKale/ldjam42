using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DoubleDamage : CodeBlock
{
    public override string getName()
    {
        return "DoubleDamage()";
    }

    public override void OnShot(params System.Object[] args)
    {
        GameObject proj = (GameObject)args[0];
        proj.GetComponent<Projectile>().Damage *= 2;
    }

    public override int spawnChance()
    {
        return 50;
    }
}
