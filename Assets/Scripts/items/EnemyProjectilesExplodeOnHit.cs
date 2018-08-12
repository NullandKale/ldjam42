using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyProjectilesExplodeOnHit : CodeBlock
{
    public override string getName()
    {
        return "Explode();";
    }

    public override void OnHit(params object[] args)
    {
        Projectile p = (Projectile)args[0];
        MonoBehaviour.Instantiate(Level.currentLevel.explosionPrefab, p.transform.position, Quaternion.identity);
    }

    public override int spawnChance()
    {
        return 5;
    }
}