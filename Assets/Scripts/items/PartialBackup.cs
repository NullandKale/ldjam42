using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartialBackup : CodeBlock
{
    public override string GetName()
    {
        return "PartialSystemBackup()";
    }

    public override OnX GetOnX()
    {
        return OnX.OnDie;
    }

    public override int SpawnChance()
    {
        return 3;
    }

    public override void OnDie(params object[] args)
    {
        if (PlayerController.Instance.KB <= 0)
        {
            PlayerController.Instance.Heal((int)(PlayerController.Instance.MaxKB * 0.3f), true);
            PlayerController.Instance.CodeBlocks.Remove(this);
        }
    }
}
