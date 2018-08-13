using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveRegen : CodeBlock
{
    public override string GetName()
    {
        return "ActiveRegen()";
    }

    public override OnX GetOnX()
    {
        return OnX.OnEnemyKilled;
    }

    public override int SpawnChance()
    {
        return 25;
    }

    public override void OnEnemyKilled(params object[] args)
    {
        PlayerController.Instance.Heal(2, true);
    }
}
