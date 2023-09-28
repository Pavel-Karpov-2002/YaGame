using System;
using TMPro;
using UnityEngine;

public class ResetUpgradesPointsController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private int _resetCost;

    public static event Action ResetEvent;

    private int _minGolds = 0;
    private int _minPoints = 0;

    void Start()
    {
        _costText.text = _resetCost.ToString();
    }

    public void ResetPoints(int deafultUpgradeLevel)
    {
        if (GameInformation.Instance.Information.Golds - _resetCost >= _minGolds)
        {
            int countPoints = 0;

            for (int i = 0; i < GameInformation.Instance.Information.UpgradesLevel.Count; i++)
            {
                countPoints += GameInformation.Instance.Information.UpgradesLevel[i];
                GameInformation.Instance.Information.UpgradesLevel[i] = deafultUpgradeLevel;
            }

            if (countPoints > _minPoints)
            {
                GameInformation.Instance.Information.Golds -= _resetCost;
                GameInformation.Instance.Information.UpgradePoints += countPoints;
                GameInformation.OnInformationChange?.Invoke();
                ResetEvent?.Invoke();
            }
        }
    }
}
