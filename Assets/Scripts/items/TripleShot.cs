using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TripleShot : CodeBlock
{
    public override string getName()
    {
        return "TripleShot();";
    }

    public override void OnShot(params object[] args)
    {
        GameObject proj = (GameObject)args[0];
        MonoBehaviour.Instantiate(proj, proj.transform.position, proj.transform.rotation * Quaternion.Euler(0, 0, -30));
        MonoBehaviour.Instantiate(proj, proj.transform.position, proj.transform.rotation * Quaternion.Euler(0, 0, 30));
    }

    public override int spawnChance()
    {
        return 15;
    }
}
