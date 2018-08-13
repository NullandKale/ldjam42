using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adrenaline : CodeBlock
{
    public override string GetName()
    {
        return "Adrenaline()";
    }

    public override void OnEnemyKilled(params object[] args)
    {
        Time.timeScale += Time.timeScale * 0.03f;
    }

    public override OnX GetOnX()
    {
        return OnX.OnHeal;
    }

    public override int SpawnChance()
    {
        return 10;
    }
}
