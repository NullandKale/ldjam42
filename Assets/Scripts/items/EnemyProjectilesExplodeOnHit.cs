using UnityEngine;

[System.Serializable]
public class EnemyProjectilesExplodeOnHit : CodeBlock
{
    public override string GetName()
    {
        return "Explode()";
    }

    public override void OnHit(params object[] args)
    {
        var p = (Projectile)args[0];
        MonoBehaviour.Instantiate(Level.currentLevel.explosionPrefab, p.transform.position, Quaternion.identity);
    }

    public override OnX GetOnX()
    {
        return OnX.OnHit;
    }

    public override int SpawnChance()
    {
        return 5;
    }
}