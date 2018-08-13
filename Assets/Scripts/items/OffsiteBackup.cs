using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsiteBackup : CodeBlock
{
    public override string GetName()
    {
        return "OffsiteBackup()";
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
        if (PlayerController.Instance.KB <= 0)
        {
            PlayerController.Instance.Heal((int)(PlayerController.Instance.MaxKB * 0.3f), true);
            PlayerController.Instance.transform.position = Level.currentLevel.getRandomRoomCenter();
            PlayerController.Instance.CodeBlocks.Remove(this);
        }
    }
}
