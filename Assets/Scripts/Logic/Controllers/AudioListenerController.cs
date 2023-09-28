using UnityEngine;

public class AudioListenerController : Singleton<AudioListenerController>
{
    [SerializeField] private float _minVolume = 0;
    [SerializeField] private float _maxVolume = 1;

    public float MinVolume => _minVolume;
    public float MaxVolume => _maxVolume;

    public void ChangeAuidoListener()
    {
        if (AudioListener.volume > _minVolume)
            AudioListener.volume = _minVolume;
        else
            AudioListener.volume = _maxVolume;
    }

    public  void SetAudioListerner(float value)
    {
        AudioListener.volume = value;
    }
}
