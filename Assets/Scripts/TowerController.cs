using UnityEngine;

public class TowerController : MonoBehaviour
{
    private Bank bank;

    private void Awake()
    {
        bank = GetComponent<Bank>();   
    }

    public Tower CreateTower(TowerBase towerPrefab, Vector3 position)
    {
        if (bank == null || bank.currentBalance < towerPrefab.TowerCost) return null;
        var newTower = Instantiate(towerPrefab, position, Quaternion.identity);
        bank.Withdraw(towerPrefab.TowerCost);
        return newTower.GetComponentInChildren<Tower>();
    }
}
