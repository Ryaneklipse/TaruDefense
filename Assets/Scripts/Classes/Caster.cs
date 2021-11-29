using UnityEngine;

public class Caster : Tower
{
    private CasterTargeter targeter;

    public override void OnEnable()
    {
        base.OnEnable();
        targeter = GetComponent<CasterTargeter>();
    }

    protected override void ExecuteSkill()
    {
        return;
    }

    public override void ExecuteAbility()
    {
        targeter.FireAtTarget(abilityPrefab, 3000);
    }

    protected override void UpgradePassiveOne()
    {
        castTime -= castTime / 2;
    }

    protected override void UpgradePassiveTwo()
    {
        castTimeModifier += Mathf.Max(castTimeModifier / 2, 0.5f);
    }

    protected override void UpgradePassiveThree()
    {
        targeter.additionalHits += 5;
    }
}
