using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class TargetLocator : MonoBehaviour
{
    [SerializeField] protected float range = 15f;
    [SerializeField] protected float chaseRange = 15f;
    [SerializeField] protected Transform weapon;
    [SerializeField] protected private GameObject projectilePrefab;
    [SerializeField] protected Transform launchOrigin;

    public bool inCombat;
    protected Transform target;
    protected Tower tower;
    protected Vector3 tetherPoint;


    private void Awake()
    {
        tower = GetComponent<Tower>();
    }

    private void OnEnable()
    {
        tetherPoint = transform.position;
        if(tower.isMelee) StartCoroutine(MoveToTarget());
    }

    private void Update()
    {
        FindClosestTarget();
        AimWeapon();
    }

    private void FindClosestTarget()
    {
        var enemies = FindObjectsOfType<Enemy>().ToList();
        Transform closest = null;
        var maxDistance = Mathf.Infinity;

        foreach(var enemy in enemies)
        {
            var targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if (targetDistance < maxDistance)
            {
                closest = enemy.transform;
                maxDistance = targetDistance;
            }
        }

        target = closest;
    }

    private void AimWeapon()
    {
        if (target == null) return;
        var distance = Vector3.Distance(transform.position, target.position);
        transform.LookAt(target);
        var inAttackRange = distance <= range;
        if (inAttackRange && !inCombat) {
            inCombat = true;
            StartCoroutine(Attack());
        }
    }

    //Should be replaced by class overrides
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

    private IEnumerator MoveToTarget()
    {
        while (true)
        {
            //Out of combat, move to tether
            var outOfRange = true;
            var startPosition = transform.parent.position;
            var endPosition = tetherPoint;
            var travelPercent = 0f;
            var movementSpeed = tower.statController.GetMovementSpeed(tower, FindObjectsOfType<Tower>().ToList());
            while (outOfRange)
            {
                travelPercent += Time.deltaTime * movementSpeed;
                transform.parent.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                if (target == null)
                {
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    var distanceToTether = Vector3.Distance(tetherPoint, target.position);
                    outOfRange = distanceToTether > chaseRange;
                    yield return new WaitForEndOfFrame();
                }
            }

            //in combat, move to enemy
            startPosition = transform.parent.position;
            endPosition = target.transform.position;
            travelPercent = 0f;

            var distanceToEnemy = Vector3.Distance(transform.parent.position, target.position);
            while (travelPercent < 1f && distanceToEnemy > range && target != null)
            {
                travelPercent += Time.deltaTime * movementSpeed;
                transform.parent.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                distanceToEnemy = Vector3.Distance(transform.parent.position, target.position);
                yield return new WaitForEndOfFrame();
            }
            while(distanceToEnemy <= range && target != null)
            {
                distanceToEnemy = Vector3.Distance(transform.parent.position, target.position);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void FireAtTarget(GameObject prefab, float velocityMultiplier)
    {
        var projectile = Instantiate(prefab, launchOrigin.position, launchOrigin.rotation);
        projectile.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * velocityMultiplier * tower.towerRank);
        projectile.GetComponentInChildren<HitZone>().damage = tower.statController.GetDamage(tower, FindObjectsOfType<Tower>().ToList());
    }
}
