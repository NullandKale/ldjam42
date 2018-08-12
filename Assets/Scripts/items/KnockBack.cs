using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KnockBack : CodeBlock
{
    public override string getName()
    {
        return "KnockBack();";
    }

    public override void OnHeal(params object[] args)
    {
        base.OnHeal(args);
    }

    public override int spawnChance()
    {
        return 10;
    }
}
