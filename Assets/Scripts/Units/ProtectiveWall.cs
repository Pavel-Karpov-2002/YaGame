using UnityEngine;

public class ProtectiveWall : Unit
{
    [SerializeField] private GameObject _playerStopObject;

    protected override void Awake()
    {
        base.Awake();
        SetStaticHealth(UnitParameters.MinHealth);
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
        if (Health <= MinHealth)
            gameObject.SetActive(false);
    }

    protected override void OnDestroy()
    {
        LevelProgress.Instance.OnCompletedLevel -= ChangeStaticHealth;
        LevelProgress.Instance.OnCompletedLevel -= ChangeGameObjectActive;
        base.OnDestroy();
    }
}
