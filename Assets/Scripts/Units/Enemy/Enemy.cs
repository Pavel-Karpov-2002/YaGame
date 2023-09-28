using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    [SerializeField] private List<DropItemParameters> _itemsParameters;
    [SerializeField] private ItemDropController _dropItem;
    [SerializeField] private LookAtController _lookAtController;
    [SerializeField] private AttackController _attack;
    [SerializeField] private int _deathLayer;
    [SerializeField] private float _timeOnDestroy = 3;
    
    private Skin _skin;

    private RagdollController _ragdollController;

    protected Skin Skin => _skin;
    protected AttackController Attack => _attack;

    protected override void Awake()
    {
        base.Awake();
        SetStaticHealth(UnitParameters.MinHealth);
        OnDeath += DisableScripts;

        _lookAtController.OnLook += _attack.Shoot;
        _skin = GetComponentInChildren<Skin>();
    }

    public override void Die()
    {
        if (Health > MinHealth)
            return;

        LevelProgress.Instance.CountKillsOnLevel += 1;
        DropItems();

        if (_ragdollController = _skin.RagdollController)
        {
            _ragdollController.EnablePhysics();
            SetDeathLayer();
        }

        _lookAtController.Tween.Kill();
        OnDeath?.Invoke();
        Destroy(gameObject, _timeOnDestroy);
    }

    private void DropItems()
    {
        foreach (var item in _itemsParameters)
            _dropItem.DropItems(item.ItemPrefab, item.MinDrops, item.MaxDrops);
    }

    private void SetDeathLayer()
    {
        gameObject.layer = _deathLayer;

        foreach (var rb in _ragdollController.RagdollElements)
            rb.gameObject.layer = _deathLayer;
    }

    private void DisableScripts()
    {
        _attack.enabled = false;
        _lookAtController.enabled = false;
        this.enabled = false;
    }

    protected override void OnDestroy()
    {
        OnDeath -= DisableScripts;
        _lookAtController.OnLook -= _attack.Shoot;
        base.OnDestroy();
    }
}
