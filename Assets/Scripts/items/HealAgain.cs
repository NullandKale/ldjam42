[System.Serializable]
public class HealAgain : CodeBlock
{
    public override string GetName()
    {
        return "HealAgain()";
    }

    public override void OnHeal(params object[] args)
    {
        PlayerController.Instance.Heal((float)args[0], false);
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