using UnityEngine;

public class BuildingClass : MonoBehaviour
{
    [SerializeField]
    protected int maxHealth;
    protected int health;

    private void Awake()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
        else if(health > maxHealth)
        {
            health = maxHealth;
        }
    }
}