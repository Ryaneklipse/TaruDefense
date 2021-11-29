using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 5;

    [Tooltip("Adds amount to maxHealth when enemy dies.")]
    [SerializeField] int difficultyRamp = 1;
    [SerializeField] private float currentHealth = 0;

    Enemy enemy;

    void OnEnable()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    public void ProcessHit(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            gameObject.SetActive(false);
            enemy.RewardGold();
            maxHealth += difficultyRamp;
        }
    }
}
