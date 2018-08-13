using UnityEngine;

[System.Serializable]
public class TripleShot : CodeBlock
{
    public override string GetName()
    {
        return "TripleShot()";
    }

    public override void OnShot(params object[] args)
    {
        if ((bool)args[1] == false)
        {
            return;
        }

        var proj = (GameObject)args[0];
        var proj2 = Object.Instantiate(proj, proj.transform.position, proj.transform.rotation * Quaternion.Euler(0, 0, -25));
        var proj3 = Object.Instantiate(proj, proj.transform.position, proj.transform.rotation * Quaternion.Euler(0, 0, 25));

        PlayerController.OnShoot.Invoke(proj2, false);
        PlayerController.OnShoot.Invoke(proj3, false);
    }

    public override OnX GetOnX()
    {
        return OnX.OnShoot;
    }

    public override int SpawnChance()
    {
        return 15;
    }
}