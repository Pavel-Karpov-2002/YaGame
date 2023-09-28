using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressUI : Singleton<LevelProgressUI>
{
    private const int DeafultMinProgress = 0;

    [SerializeField] private Image _progressIndicator;
    [SerializeField] private TextMeshProUGUI _levelNumText;

    public void UpdateProgressIndicator(float value)
    {
        _progressIndicator.fillAmount = value;
    }

    public void UpdateLevelNumText(int num)
    {
        _levelNumText.text = num.ToString();
        UpdateProgressIndicator(DeafultMinProgress);
    }
}
