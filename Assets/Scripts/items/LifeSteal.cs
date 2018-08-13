using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSteal : CodeBlock
{
    public override string GetName()
    {
        return "LifeSteal()";
    }

    public override void OnHit(params object[] args)
    {
        PlayerController.Instance.Heal(0.1f, true);
    }

    public override OnX GetOnX()
    {
        return OnX.OnHit;
    }

    public override int SpawnChance()
    {
        return 5;
    }
}