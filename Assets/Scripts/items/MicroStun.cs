using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroStun : CodeBlock
{
    public override string GetName()
    {
        return "MicroStun()";
    }

    public override OnX GetOnX()
    {
        return OnX.OnEnemyHit;
    }

    public override int SpawnChance()
    {
        return 10;
    }

    public override void OnEnemyHit(params object[] args)
    {
        var e = (Enemy)args[1];
        e.currentFireRate -= 0.25f;
    }
}