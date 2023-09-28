using System;
using System.Runtime.InteropServices;

public class LevelProgress : Singleton<LevelProgress>
{
    private const float CompletionThreshold = 1.0f;

    [DllImport("__Internal")]
    private static extern void SetToLeaderboard(int value);

    private int _countKillsOnLevel;

    public Action OnCompletedLevel;

    public int RequiredNumberOfKills { get; set; }

    public int CountKillsOnLevel 
    {
        get { return _countKillsOnLevel; }
        set
        {
            _countKillsOnLevel = value;
            float progress = (float)_countKillsOnLevel / RequiredNumberOfKills;
            LevelProgressUI.Instance.UpdateProgressIndicator(progress);

            if (progress >= CompletionThreshold)
            {
                OnCompletedLevel?.Invoke();

                if (GameInformation.Instance.Information.PassedLevel % AdvertisementController.Instance.FrequencyRewardedAdvertisement == 0)
                    AdvertisementController.Instance.ButtonReward.transform.localScale = UnityEngine.Vector3.one;
            }
        }
    }

    private void Start()
    {
        OnCompletedLevel += UpdateLevelParameters;
    }

    private void UpdateLevelParameters()
    {
        CountKillsOnLevel = 0;
        LevelProgressUI.Instance.UpdateLevelNumText(++GameInformation.Instance.Information.PassedLevel);
#if UNITY_WEBGL && !UNITY_EDITOR
        SetToLeaderboard(GameInformation.Instance.Information.PassedLevel);
#endif
        GameInformation.OnInformationChange?.Invoke();
    }

    private void OnDestroy()
    {
        OnCompletedLevel -= UpdateLevelParameters;
        OnCompletedLevel = null;
    }
}
