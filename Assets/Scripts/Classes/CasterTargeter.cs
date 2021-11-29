using System.Collections;
using System.Linq;
using UnityEngine;

public class CasterTargeter : TargetLocator
{
    [SerializeField] public float castTime;
    [SerializeField] public bool isCasting;
    public float currentCastTime;
    public int additionalHits;

    protected override IEnumerator Attack()
    {
        while (inCombat)
        {
            var towersInRangeOfPassive = FindObjectsOfType<Tower>().ToList();
            var attackRate = tower.statController.GetAttackRate(tower, towersInRangeOfPassive);
            isCasting = true;
            castTime = tower.statController.GetCastTime(tower, towersInRangeOfPassive);
            yield return new WaitForSeconds(castTime);
            currentCastTime = 0;
            isCasting = false;
            if(target == null)
            {
                inCombat = false;
                break;
            }
            var distance = Vector3.Distance(transform.position, target.position);
            inCombat = distance <= range;
            if (!inCombat) break;
            var projectile = Instantiate(projectilePrefab, launchOrigin.position, launchOrigin.rotation);
            projectile.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 700 * attackRate * tower.towerRank);
            var hitZone = projectile.GetComponentInChildren<HitZone>();
            hitZone.damage = tower.statController.GetDamage(tower, towersInRangeOfPassive);
            hitZone.destoryAtCount += additionalHits;
        }
    }

    private void FixedUpdate()
    {
        //Display cast bar
    }
}
