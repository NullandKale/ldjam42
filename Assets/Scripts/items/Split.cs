using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : CodeBlock
{
    public override string GetName()
    {
        return "Split()";
    }

    public override OnX GetOnX()
    {
        return OnX.OnEnemyHit;
    }

    public override int SpawnChance()
    {
        return 10;
    }

    public override void OnEnemyHit(params object[] args)
    {
        var proj = (GameObject)args[0];

        if (proj != null)
        {
            var proj2 = Object.Instantiate(proj, proj.transform.position, proj.transform.rotation * Quaternion.Euler(0, 0, -25));
            var proj3 = Object.Instantiate(proj, proj.transform.position, proj.transform.rotation * Quaternion.Euler(0, 0, 25));

            var proj2p = proj2.GetComponent<Projectile>();
            var proj3p = proj3.GetComponent<Projectile>();

            proj2p.noCollideTime = 0.4f;
            proj3p.noCollideTime = 0.4f;

            proj2p.c.enabled = false;
            proj3p.c.enabled = false;

            PlayerController.OnShoot.Invoke(proj2, false, false);
            PlayerController.OnShoot.Invoke(proj3, false, false);
        }
        else
        {
            Debug.Log("proj == null");
        }
    }
}
