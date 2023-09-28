using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class AdvertisementController : Singleton<AdvertisementController>
{
    private const float StopGameTimeScale = 0;
    private const float DefaulttGameTimeScale = 1;

    [SerializeField] private Button _buttonRewardVideo;
    [SerializeField] private Button _buttonInternalVideo;
    [SerializeField] private int _frequencyInternalAdvertisement = 3;
    [SerializeField] private int _frequencyRewardedAdvertisement = 2;

    [DllImport("__Internal")]
    public static extern void ShowInternal();
    [DllImport("__Internal")]
    public static extern void ShowRewardedVideo();

    public Button ButtonReward => _buttonRewardVideo;
    public int FrequencyInternalAdvertisement => _frequencyInternalAdvertisement;
    public int FrequencyRewardedAdvertisement => _frequencyRewardedAdvertisement;

    private bool _isViewed;
    private bool _isSetAudioListener;
    private int _equalizer = 0;

    protected override void Awake()
    {
        base.Awake();
        _buttonRewardVideo.onClick.AddListener(() => RewardedVideo());
        _buttonInternalVideo.onClick.AddListener(() =>
        {

            if (GameInformation.Instance.Information.PassedLevel % _frequencyInternalAdvertisement == _equalizer && !_isViewed)
            {
                _isViewed = true;
                Internal();
            }
        });
    }

    private void Start()
    {
        StartGameController.Instance.OnStartGame += () => _isViewed = false;
    }

    public void Internal()
    {
        StopLevel();
        ShowInternal();
    }

    public void RewardedVideo()
    {
        StopLevel();
        _buttonRewardVideo.transform.localScale = Vector3.zero;
        ShowRewardedVideo();
    }

    private void StopLevel()
    {
        Time.timeScale = StopGameTimeScale;

        if (AudioListener.volume > AudioListenerController.Instance.MinVolume)
        {
            _isSetAudioListener = true;
            AudioListenerController.Instance.SetAudioListerner(AudioListenerController.Instance.MinVolume);
        }
    }

    public void CloseAdvertisement()
    {
        Time.timeScale = DefaulttGameTimeScale;

        if (_isSetAudioListener)
        {
            AudioListenerController.Instance.SetAudioListerner(AudioListenerController.Instance.MaxVolume);
            _isSetAudioListener = false;
        }
    }
 
    public void AddGems(int value)
    {
        GameInformation.Instance.Information.Gems += value;
        GameInformation.OnInformationChange?.Invoke();
    }

    private void OnDestroy()
    {
        StartGameController.Instance.OnStartGame -= () => _isViewed = false;
    }
}
