using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeTargeter : TargetLocator
{
    [SerializeField] float hitRange = 3f;
    [SerializeField] bool canHitMultipleTimes;

    private List<GameObject> hitObjects = new List<GameObject>();

    protected override IEnumerator Attack()
    {
        while (inCombat)
        {
            var towersInRangeOfPassive = FindObjectsOfType<Tower>().ToList();
            var attackRate = tower.statController.GetAttackRate(tower, towersInRangeOfPassive);
            var collider = Physics.OverlapSphere(gameObject.transform.position, hitRange);

            foreach (var target in collider)
            {
                if (!canHitMultipleTimes && hitObjects.Contains(target.gameObject)) continue;
                hitObjects.Add(target.gameObject);
                var enemyHealth = target.gameObject.GetComponent<EnemyHealth>();
                if (enemyHealth == null) continue;
                enemyHealth.ProcessHit(tower.statController.GetDamage(tower, towersInRangeOfPassive));
            }
            yield return new WaitForSeconds(attackRate / tower.towerRank);
            hitObjects.Clear();
            if (target == null)
            {
                inCombat = false;
                continue;
            }
            var distance = Vector3.Distance(transform.position, target.position);
            inCombat = distance <= hitRange;
            tower.tpController.IncreaseTp(tower.statController.GetTpRate(tower, towersInRangeOfPassive));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, hitRange);
    }
}
