using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class RangerTargeter : TargetLocator
{
    protected virtual IEnumerator Attack()
    {
        while (inCombat)
        {
            var towersInRangeOfPassive = FindObjectsOfType<Tower>().ToList();
            var attackRate = tower.statController.GetAttackRate(tower, towersInRangeOfPassive);
            if (tower.isRanged || tower.isCaster)
            {
                var projectile = Instantiate(projectilePrefab, launchOrigin.position, launchOrigin.rotation);
                projectile.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 700 * attackRate);
                yield return new WaitForSeconds(1 / attackRate);
                var distance = Vector3.Distance(transform.position, target.position);
                inCombat = distance <= range;
                if (tower.isRanged) tower.tpController.IncreaseTp(tower.statController.GetTpRate(tower, towersInRangeOfPassive));
            }
            else
            {
                tower.tpController.IncreaseTp(tower.statController.GetTpRate(tower, towersInRangeOfPassive));
                yield return new WaitForSeconds(1 / attackRate);
            }
        }
    }
}
