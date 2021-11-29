using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] List<Waypoint> path = new List<Waypoint>();
    [SerializeField] [Range(0f, 10f)] float speed = 1f;
    [SerializeField] [Range(0f, 10f)] float combatRange = 8f;

    Enemy enemy;
    public Transform target;

    public bool inCombat;

    private void OnEnable()
    {
        FindPath();
        ReturnToStart();
        StartCoroutine(FollowPath());
    }

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        FindClosestTarget();
    }

    private void FindPath()
    {
        path.Clear();
        var parent = GameObject.FindGameObjectWithTag("Path");

        foreach (Transform child in parent.transform)
        {
            path.Add(child.GetComponent<Waypoint>());
        }
    }

    private void ReturnToStart()
    {
        transform.position = path.First().transform.position;
    }

    private void FinishPath()
    {
        enemy.StealGold();
        gameObject.SetActive(false);
    }

    private IEnumerator FollowPath()
    {
        foreach(var waypoint in path)
        {
            var startPosition = transform.position;
            var endPosition = waypoint.transform.position;
            var travelPercent = 0f;

            while (travelPercent < 1f)
            {
                if(target != null)
                {
                    inCombat = true;
                    while (inCombat && target != null)
                    {
                        var distance = Vector3.Distance(transform.position, target.position);
                        inCombat = distance <= combatRange;
                        if(inCombat) yield return new WaitForEndOfFrame();
                    }
                    inCombat = false;
                }
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                if (!inCombat) transform.LookAt(endPosition);
                yield return new WaitForEndOfFrame();
            }
        }

        FinishPath();
    }

    private void FindClosestTarget()
    {
        var towers = FindObjectsOfType<Tower>().ToList().Where(tower => tower.isMelee);
        Transform closest = null;
        var maxDistance = Mathf.Infinity;

        foreach (var tower in towers)
        {
            var targetDistance = Vector3.Distance(transform.position, tower.transform.position);

            if (targetDistance < maxDistance)
            {
                closest = tower.transform;
                maxDistance = targetDistance;
            }
        }

        target = closest;
    }
}
