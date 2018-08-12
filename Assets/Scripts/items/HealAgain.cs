using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAgain : CodeBlock
{
    public override void OnHeal(params object[] args)
    {
            PlayerController.Instance.KB += (float)args[0];
    }
}
