using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectilesExplodeOnHit : CodeBlock
{
    public override void OnHit(params object[] args)
    {
        Projectile p = (Projectile)args[0];
        MonoBehaviour.Instantiate(Level.currentLevel.explosionPrefab, p.transform.position, Quaternion.identity);
    }
}