using System;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    private const int DeafultUpgradeLevel = 0;
    private const int MinimumUpgradePointsRequired = 0;
    private const int UpgradeIncrement = 1;

    [SerializeField] private UpgradesParameters _upgradesParameters;
    [SerializeField] private Unit _unitUpgrade;

    public Unit UnitUpgrade => _unitUpgrade;
    public int CostUpgrade { get; private set; }
    public object Parameters { get; protected set; }
    public string LastValue { get; protected set; }
    public int UpgradeId { get; protected set; }
    public int Level { get; protected set; }

    public Action OnActivate;

    protected virtual void Awake()
    {
        CostUpgrade = _upgradesParameters.MinCost;
        Parameters = _upgradesParameters.Value;
        if (GameInformation.Instance.Information.UpgradesLevel.Count - 1 < UpgradeId)
        {
            for (int i = GameInformation.Instance.Information.UpgradesLevel.Count - 1; i < UpgradeId; i++)
            {
                GameInformation.Instance.Information.UpgradesLevel.Add(DeafultUpgradeLevel);
            }

            GameInformation.OnInformationChange?.Invoke();
        }
        ResetUpgradesPointsController.ResetEvent += ResetLevel;
        ResetUpgradesPointsController.ResetEvent += SetUpgradeLevel;
    }

    protected bool UpLevel()
    {
        bool isLiquid = GameInformation.Instance.Information.UpgradePoints - CostUpgrade >= MinimumUpgradePointsRequired;

        if (isLiquid)
        {
            GameInformation.Instance.Information.UpgradesLevel[UpgradeId] += UpgradeIncrement;
            GameInformation.Instance.Information.UpgradePoints -= CostUpgrade;
            GameInformation.OnInformationChange?.Invoke();
        }

        return isLiquid;
    }

    public virtual void Activate()
    {
        GameInformation.OnInformationChange?.Invoke();
    }

    protected virtual void SetUpgradeLevel()
    {
    }

    protected void ResetLevel()
    {
        Level = DeafultUpgradeLevel;
    }

    protected virtual void OnDestroy()
    {
        ResetUpgradesPointsController.ResetEvent -= ResetLevel;
        ResetUpgradesPointsController.ResetEvent -= SetUpgradeLevel;
    }
}
