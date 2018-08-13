using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullBackup : CodeBlock
{
    public override string GetName()
    {
        return "FullSystemBackup()";
    }

    public override OnX GetOnX()
    {
        return OnX.OnDie;
    }

    public override int SpawnChance()
    {
        return 1;
    }

    public override void OnDie(params object[] args)
    {
       if(PlayerController.Instance.KB <= 0)
        {
            PlayerController.Instance.Heal(PlayerController.Instance.MaxKB, true);
            PlayerController.Instance.CodeBlocks.Remove(this);
        }
    }
}
