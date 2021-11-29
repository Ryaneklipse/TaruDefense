using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
public class EnemyTargeter : MonoBehaviour
{
    private EnemyMover mover;
    [SerializeField] float range = 8f;
    [SerializeField] ParticleSystem particle;

    // Start is called before the first frame update
    void OnEnable()
    {
        mover = GetComponent<EnemyMover>();
        var emission = particle.emission;
        emission.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        AimWeapon();
    }

    private void AimWeapon()
    {
        if (mover.inCombat && mover.target != null)
        {
            var distance = Vector3.Distance(transform.position, mover.target.position);
            transform.LookAt(mover.target);
            var inAttackRange = distance <= range;
            Attack(inAttackRange);
        }
        else
        {
            Attack(false);
        }
    }

    private void Attack(bool isActive)
    {
        var emission = particle.emission;
        //emission.rateOverTime = tower.towerRank; Add for difficulty ramping later
        emission.enabled = isActive;
    }
}
