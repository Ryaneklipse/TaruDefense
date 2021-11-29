using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AbilityController : MonoBehaviour
{
    [SerializeField] private Image abilityBarFill;
    private RectTransform abilityTransform;
    private float defaultBarWidth;

    private Tower tower;
    private TargetLocator targetLocator;
    private MeleeTargeter melee;

    private float currentAbilityTime;
    private bool isCasting;
    private float abilityTime;


    private void OnEnable()
    {
        tower = GetComponent<Tower>();
        targetLocator = GetComponent<TargetLocator>();
        melee = GetComponent<MeleeTargeter>();
        abilityTransform = abilityBarFill.GetComponent<RectTransform>();
        defaultBarWidth = abilityTransform.sizeDelta.x;
        StartCoroutine(UseAbility());
    }

    private void Update()
    {
        if (isCasting)
        {
            currentAbilityTime += Time.deltaTime;
            abilityTransform.sizeDelta = new Vector2(Mathf.Min(currentAbilityTime / abilityTime, 1) * defaultBarWidth, abilityTransform.sizeDelta.y);
        }
        else
        {
            currentAbilityTime = 0;
            abilityTransform.sizeDelta = new Vector2(0, abilityTransform.sizeDelta.y);
        }
    }

    private IEnumerator UseAbility()
    {
        while (true)
        {
            var towersInRangeOfPassive = FindObjectsOfType<Tower>().ToList();
            if (tower.isMelee)
            {
                if (melee.inCombat)
                {
                    abilityTime = tower.statController.GetAbilityCooldown(tower, towersInRangeOfPassive);
                    isCasting = true;
                    yield return new WaitForSeconds(abilityTime);
                    isCasting = false;
                    currentAbilityTime = 0;
                    tower.ExecuteAbility();
                }
                yield return new WaitForEndOfFrame();
            }
            else
            {
                if (targetLocator.inCombat)
                {
                    abilityTime = tower.statController.GetAbilityCooldown(tower, towersInRangeOfPassive);
                    isCasting = true;
                    yield return new WaitForSeconds(abilityTime);
                    isCasting = false;
                    currentAbilityTime = 0;
                    tower.ExecuteAbility();
                }
                yield return new WaitForEndOfFrame();
            }
        }

    }
}
