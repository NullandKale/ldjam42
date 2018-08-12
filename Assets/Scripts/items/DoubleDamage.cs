using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDamage : CodeBlock
{
    public override void OnShot(params System.Object[] args)
    {
        GameObject proj = (GameObject)args[0];
        proj.GetComponent<Projectile>().Damage *= 2;
    }
}
