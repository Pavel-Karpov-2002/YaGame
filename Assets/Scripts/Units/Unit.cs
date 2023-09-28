using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitParameters _unitParameters;
    [SerializeField] private int _minHealth = 0;

    private int _staticHealthParameter;
    private int _health;
    private float _speed;

    protected int MinHealth => _minHealth;
    public int StaticHealthParameter => _staticHealthParameter;
    public UnitParameters UnitParameters => _unitParameters;

    public int Health
    {
        get => _health;
        private set
        {
            _health = value;
            OnHealthChange?.Invoke(_health);
        }
    }

    public float Speed
    {

        get => _speed;
        private set
        {
            _speed = value;
            OnSpeedSet?.Invoke(_speed);
        }
    }

    public event Action<int> OnHealthChange;
    public event Action<int> OnHealthSet;
    public event Action<float> OnSpeedSet;
    public Action OnDeath;

    protected virtual void Awake()
    {
        OnHealthChange += (h) => Die();
    }

    public virtual void TakeDamage(int amount)
    {
        if (_health - amount < _minHealth)
            return;

        Health -= amount;
    }

    public virtual void AddHealth(int amount)
    {
        Health += amount;
    }

    public virtual void SetStaticHealth(int amount)
    {
        _staticHealthParameter = amount;
        Health = _staticHealthParameter;
        OnHealthSet?.Invoke(amount);
    }

    public virtual void SetStaticSpeed(float amount)
    {
        Speed = amount;
        OnSpeedSet?.Invoke(amount);
    }

    public virtual void Die()
    {
        if (Health > _minHealth)
            return;

        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        OnDeath = null;
    }
}
