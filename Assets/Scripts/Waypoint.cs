using Assets.Scripts;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] public bool isPlaceable;

    [SerializeField] private TowerBase leftClickTower;
    [SerializeField] private TowerBase rightClickTower;
    [SerializeField] private TowerBase middleClickTower;

    private Tower createdTower;
    private TowerController controller;

    private void Awake()
    {
        controller = FindObjectOfType<TowerController>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick(leftClickTower, Passive.PassiveOne);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleClick(rightClickTower, Passive.PassiveTwo);
        }
        else if (Input.GetMouseButtonDown(2))
        {
            HandleClick(middleClickTower, Passive.PassiveThree);
        }
    }

    private void HandleClick(TowerBase tower, Passive passive)
    {
        if (isPlaceable)
        {
            createdTower = controller.CreateTower(tower, transform.position);
            createdTower.waypoint = this;
            isPlaceable = createdTower == null;
        }
        else if(createdTower != null)
        {
            createdTower.UpgradeTower(passive);
        }
    }

    public void EnablePlacement()
    {
        isPlaceable = true;
        createdTower = null;
    }
}
