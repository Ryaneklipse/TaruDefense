public class Melee : Tower
{
    private MeleeTargeter targeter;

    public override void OnEnable()
    {
        base.OnEnable();
        targeter = GetComponent<MeleeTargeter>();
    }

    protected override void ExecuteSkill()
    {
        targeter.FireAtTarget(skillPrefab, 3000);
    }

    public override void ExecuteAbility()
    {
        targeter.FireAtTarget(abilityPrefab, 3000);
    }

    protected override void UpgradePassiveOne()
    {
        damage++;
    }

    protected override void UpgradePassiveTwo()
    {
        speed++;
    }

    protected override void UpgradePassiveThree()
    {
        damageModifier++;
    }
}
