using System.Collections.Generic;
using UnityEngine;

public class HitZone : MonoBehaviour
{
    [SerializeField] float range = 3;
    public int destoryAtCount = 1;
    [SerializeField] bool canHitMultipleTimes;

    public float damage = 1f;

    private List<GameObject> hitObjects = new List<GameObject>();
    private int currentHitCount;

    private void Update()
    {
        var collider = Physics.OverlapSphere(gameObject.transform.position, range);

        foreach (var target in collider)
        {
            if (!canHitMultipleTimes && hitObjects.Contains(target.gameObject)) continue;
            hitObjects.Add(target.gameObject);
            var enemyHealth = target.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth == null) continue;
            currentHitCount++;
            enemyHealth.ProcessHit(damage);

            if(currentHitCount >= destoryAtCount)
            {
                Destroy(gameObject.transform.parent.gameObject);
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, range);
    }
}
