using System;

public class UpgradeAddHealthPoints : Upgrade
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        UnitUpgrade.OnHealthSet += LastValueUpdate;
        SetUpgradeLevel();
    }

    protected override void SetUpgradeLevel()
    {
        base.SetUpgradeLevel();
        UnitUpgrade.SetStaticHealth(UnitUpgrade.UnitParameters.MinHealth + Convert.ToInt32(Parameters) * Level);
        OnActivate?.Invoke();
        GameInformation.OnInformationChange?.Invoke();
    }

    public override void Activate()
    {
        if (UpLevel())
        {
            UnitUpgrade.SetStaticHealth(UnitUpgrade.Health + Convert.ToInt32(Parameters));
            OnActivate?.Invoke();
        }

        base.Activate();
    }

    public void LastValueUpdate(int amount)
    {
        LastValue = amount.ToString();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnitUpgrade.OnHealthSet -= LastValueUpdate;
    }
}
