using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesExplodeOnDeath : CodeBlock
{
    public override string GetName()
    {
        return "EnemiesExplode()";
    }

    public override OnX GetOnX()
    {
        return OnX.OnEnemyKilled;
    }

    public override int SpawnChance()
    {
        return 10;
    }

    public override void OnEnemyKilled(params object[] args)
    {
        var e = (Enemy)args[0];
        MonoBehaviour.Instantiate(Level.currentLevel.explosionPrefab, e.transform.position, Quaternion.identity).GetComponent<CircleCollider2D>().isTrigger = true;
    }
}
