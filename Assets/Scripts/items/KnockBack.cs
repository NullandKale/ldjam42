using UnityEngine;

[System.Serializable]
public class KnockBack : CodeBlock
{
    public override string GetName()
    {
        return "GiveMeSpace()";
    }

    public override void OnHeal(params object[] args)
    {
        GameObject g = MonoBehaviour.Instantiate(Level.currentLevel.explosionPrefab, PlayerController.Instance.gameObject.transform.position, Quaternion.identity);
        g.transform.localScale = new Vector3(2, 2, 2);
        g.GetComponent<SpriteRenderer>().color = Color.clear;
    }

    public override OnX GetOnX()
    {
        return OnX.OnHeal;
    }

    public override int SpawnChance()
    {
        return 10;
    }
}