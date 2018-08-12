using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Homing : CodeBlock
{
    public override void OnShot(params object[] args)
    {
        GameObject proj = (GameObject)args[0];
        GameObject home = Level.currentLevel.getClosestEnemy(proj.transform.position);
        Projectile p = proj.GetComponent<Projectile>();
        //GameObject.Destroy(p.GetComponent<CircleCollider2D>());
        p.StartCoroutine(Home(proj.GetComponent<Rigidbody2D>(), home.transform.position, p.Speed));
    }

    public IEnumerator Home(Rigidbody2D self, Vector3 enemy, float Speed)
    {
        while (true)
        {
            Vector3 dir = enemy - self.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            self.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            self.AddForce(self.transform.right * 100 * Speed * Time.fixedDeltaTime, ForceMode2D.Force);
            yield return new WaitForFixedUpdate();
        }
    }

    public override int spawnChance()
    {
        return 0;
    }

    public override string getName()
    {
        return "Homing();";
    }
}
