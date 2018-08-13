[System.Serializable]
public class KnockBack : CodeBlock
{
    public override string GetName()
    {
        return "KnockBack()";
    }

    public override void OnHeal(params object[] args)
    {
        base.OnHeal(args);
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