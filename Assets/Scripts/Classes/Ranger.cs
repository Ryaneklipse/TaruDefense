public class Ranger : Tower
{
    private RangerTargeter targeter;

    public override void OnEnable()
    {
        base.OnEnable();
        targeter = GetComponent<RangerTargeter>();
    }

    protected override void ExecuteSkill()
    {
        targeter.FireAtTarget(skillPrefab, 3000);
        return;
    }

    public override void ExecuteAbility()
    {
        targeter.FireAtTarget(abilityPrefab, 3000);
        return;
    }

    protected override void UpgradePassiveOne()
    {
        attackRate += 2;
        return;
    }

    protected override void UpgradePassiveTwo()
    {
        attackRateModifier++;
        return;
    }

    protected override void UpgradePassiveThree()
    {
        tpRate += 20;
        return;
    }
}
