using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [SerializeField] private StatDefinition _health;
    [SerializeField] private StatDefinition _speed;
    [SerializeField] private StatDefinition _damage;

    public Stat Health { get; private set; }
    public Stat Speed { get; private set; }
    public Stat Damage { get; private set; }

    private void Awake()
    {
        Health = new Stat(_health);
        Speed = new Stat(_speed);
        Damage = new Stat(_damage);
    }
}
