using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealAgain : CodeBlock
{
    public override string getName()
    {
        return "HealAgain();";
    }

    public override void OnHeal(params object[] args)
    {
            PlayerController.Instance.KB += (float)args[0];
    }

    public override int spawnChance()
    {
        return 10;
    }
}
