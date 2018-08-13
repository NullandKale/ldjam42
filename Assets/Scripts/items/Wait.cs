using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : CodeBlock
{
    public override string GetName()
    {
        return "Wait()";
    }

    public override void OnHeal(params object[] args)
    {
        Time.timeScale -= Time.timeScale * 0.05f;
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