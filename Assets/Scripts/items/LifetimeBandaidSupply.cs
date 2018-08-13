using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifetimeBandaidSupply : CodeBlock
{
    public override string GetName()
    {
        return "LifetimeBandaidSupply()";
    }

    public override void OnHit(params object[] args)
    {
        PlayerController.Instance.Heal(0.5f, true);
    }

    public override OnX GetOnX()
    {
        return OnX.OnHit;
    }

    public override int SpawnChance()
    {
        return 10;
    }
}
