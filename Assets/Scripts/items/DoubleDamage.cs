using UnityEngine;

[System.Serializable]
public class DoubleDamage : CodeBlock
{
    public override string GetName()
    {
        return "DoubleDamage()";
    }

    public override void OnShot(params System.Object[] args)
    {
        var proj = (GameObject)args[0];
        proj.GetComponent<Projectile>().Damage *= 2;
    }

    public override OnX GetOnX()
    {
        return OnX.OnShoot;
    }

    public override int SpawnChance()
    {
        return 50;
    }
}