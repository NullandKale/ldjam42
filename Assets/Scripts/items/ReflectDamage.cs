using UnityEngine;

[System.Serializable]
public class ReflectDamage : CodeBlock
{
    public override string GetName()
    {
        return "Reflect()";
    }

    public override void OnEnemyHit(params object[] args)
    {
        var proj = (GameObject)args[1];

        if (proj != null)
        {
            if (proj != PlayerController.Instance.gameObject)
            {
                proj.GetComponent<Enemy>().Damage(null, 1);
            }
        }
    }

    public override OnX GetOnX()
    {
        return OnX.OnEnemyHit;
    }

    public override int SpawnChance()
    {
        return 25;
    }
}