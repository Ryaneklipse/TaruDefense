using Assets.Scripts;
using Assets.StatControllers;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Required fields")]
    [SerializeField] public int towerCost = 50;
    [SerializeField] int upgradeCost = 25;
    [SerializeField] int upgradeScaling = 2;
    public int towerRank = 1;
    Bank bank;

    public bool isMelee = false;
    public bool isRanged = false;
    public bool isCaster = false;
    public int currentHealth;

    public TpController tpController;
    public Waypoint waypoint;

    public readonly StatController statController = new StatController();

    public GameObject skillPrefab;
    public GameObject abilityPrefab;

    //stats
    [Space(20)]
    [Header("Stats")]
    [SerializeField] public float attackRate = 1f;
    [SerializeField] public int maxHealth = 15;
    [SerializeField] public int damage = 1;
    [SerializeField] public float castTime = 1f;
    [SerializeField] public float abilityCooldown = 1f;
    [SerializeField] public float speed = 1f;
    [SerializeField] public float tpRate = 20f;

    //passive modifiers
    [Space(20)]
    [Header("Stat Modifiers / aoe passives")]
    [SerializeField] public float attackRateModifier = 0f;
    [SerializeField] public int maxHealthModifier = 0;
    [SerializeField] public int damageModifier = 0;
    [SerializeField] public float castTimeModifier = 0;
    [SerializeField] public float abilityCooldownModifier = 0;
    [SerializeField] public float speedModifier = 0;
    [SerializeField] public float tpRateModifier = 0;

    public virtual void OnEnable()
    {
        tpController = GetComponent<TpController>();
        currentHealth = statController.GetMaxHealth(this, FindObjectsOfType<Tower>().ToList());
    }

    public void Update()
    {
        if (!isCaster)
        {
            if (tpController.currentTp >= tpController.tpRequiredForSkill)
            {
                tpController.DescreaseTp(tpController.tpRequiredForSkill);
                ExecuteSkill();
            }
        }
    }

    public void UpgradeTower(Passive passive)
    {
        bank = FindObjectOfType<Bank>();
        if (bank == null || bank.currentBalance < upgradeCost) return;
        bank.Withdraw(upgradeCost);
        upgradeCost *= upgradeScaling;
        towerRank++;
        switch (passive)
        {
            case Passive.PassiveOne:
                UpgradePassiveOne();
                break;
            case Passive.PassiveTwo:
                UpgradePassiveTwo();
                break;
            case Passive.PassiveThree:
                UpgradePassiveThree();
                break;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (isMelee)
        {
            ProcessHit();
        }
    }

    void ProcessHit()
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            transform.parent.gameObject.SetActive(false);
            waypoint.EnablePlacement();
        }
    }

    protected virtual void ExecuteSkill()
    {
        //Temp, to be replaced with overrides per job
        return;
    }

    public virtual void ExecuteAbility()
    {
        //Temp, to be replaced with overrides per job
        return;
    }

    protected virtual void UpgradePassiveOne()
    {
        //Temp, to be replaced with overrides per job
        return;
    }

    protected virtual void UpgradePassiveTwo()
    {
        //Temp, to be replaced with overrides per job
        return;
    }

    protected virtual void UpgradePassiveThree()
    {
        //Temp, to be replaced with overrides per job
        return;
    }

    #region Stat Modifiers

    public float GetAttackRateModifier()
    {
        return attackRateModifier;
    }

    public float GetCastTimeModifier()
    {
        return castTimeModifier;
    }

    public float GetDamageModifier()
    {
        return damageModifier;
    }

    public float GetMovementSpeedModifier()
    {
        return speedModifier;
    }

    public int GetHealthModifier()
    {
        return maxHealthModifier;
    }

    public float GetAbilityCooldownModifier()
    {
        return abilityCooldownModifier;
    }

    public float GetTpRateModifier()
    {
        return tpRateModifier;
    }

    #endregion
}
