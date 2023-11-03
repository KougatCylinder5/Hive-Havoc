using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static PathingManager;

public class BuildingClass : MonoBehaviour, IHealth
{
    [SerializeField]
    protected int buildingSizeX = 2;
    [SerializeField]
    protected int buildingSizeY = 2;
    protected int buildingX, buildingY;
    [SerializeField]
    protected int[] buildingCost;

    protected int _health, _maxHealth, _regeneration;
    private bool _isDead = false;
    public bool IsDead { get => _isDead; protected set => _isDead = value; }
    protected float _resistance;

    public int Health {
        get => _health;
        protected set
        {
            if (_health < 0)
            {
                _health = Mathf.RoundToInt(_maxHealth * 0.3f);
                IsDead = true;
            }
            else
            {
                _health = value;
            }
        }
    }
    public int MaxHealth { get => _maxHealth; }
    public int HealthRegen { get => _regeneration; }
    public float Resistance { get => _resistance; }

    // Start is called before the first frame update
    void Start()
    {
        buildingX = Mathf.CeilToInt(gameObject.transform.position.x);
        buildingY = Mathf.CeilToInt(gameObject.transform.position.z);

    }

    public Vector2Int GetBuildingSize()
    {
        return new Vector2Int(buildingSizeX, buildingSizeY);
    }

    public void SetBuildingSize()
    {

    }

    public bool CheckPlacementArea(int x, int y)
    {
        for(int i = 0; i < buildingSizeX * buildingSizeY; i++)
        {
            if (!IsOpen(new(x - i % buildingSizeX, y - i / buildingSizeX)))
            {
                return false;
            }
        }
        return true;
    }

    public int[] GetCost()
    {
        return buildingCost;
    }

    public void DealDamage(int damage)
    {
        Health -= Mathf.RoundToInt(damage * (1-_resistance));
    }

    public void Regenerate()
    {
        Health += _regeneration;
    }
}
