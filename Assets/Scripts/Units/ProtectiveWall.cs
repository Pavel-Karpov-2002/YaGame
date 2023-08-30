﻿using UnityEngine;

public class ProtectiveWall : Unit
{
    [SerializeField] private UnitParameters _unitParameters;
    [SerializeField] private GameObject _playerStopObject;

    protected override void Awake()
    {
        base.Awake();
        SetStaticHealth(_unitParameters.MinHpWall);
        LevelProgress.Instance.OnCompletedLevel += ChangeStaticHealth;
        LevelProgress.Instance.OnCompletedLevel += ChangeGameObjectActive;
    }

    private void ChangeStaticHealth()
    {
        SetStaticHealth(StaticHealthParameter);
    }

    private void ChangeGameObjectActive()
    {
        gameObject.SetActive(true);
    }

    public override void Die()
    {
        if (Health <= 0)
            gameObject.SetActive(false);
    }

    protected override void OnDestroy()
    {
        LevelProgress.Instance.OnCompletedLevel -= ChangeStaticHealth;
        LevelProgress.Instance.OnCompletedLevel -= ChangeGameObjectActive;
        base.OnDestroy();
    }
}
