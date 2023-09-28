using System;
using System.Collections;
using UnityEngine;

public class PointsTimer : Singleton<PointsTimer>
{
    private const int DeafaultOneTick = 1;

    private float _time;
    private float _startTime;
    private Coroutine _timerCoroutine;

    public float RemainingTimeInProcent { get; private set; }

    public event Action<float> OnTimerUpdate;

    public void StartTimer(float time)
    {
        _time = time;
        _startTime = time;
        _timerCoroutine = StartCoroutine(TimerUpdate());
        LevelProgress.Instance.OnCompletedLevel += StopTimer;
        LevelProgress.Instance.OnCompletedLevel += TakeUpgradePoints.Instance.TakeGems;
    }

    public void StopTimer()
    {
        StopCoroutine(_timerCoroutine);
        RemainingTimeInProcent = GetPercentOfTotalTime();
        LevelProgress.Instance.OnCompletedLevel -= TakeUpgradePoints.Instance.TakeGems;
        LevelProgress.Instance.OnCompletedLevel -= StopTimer;
    }

    private IEnumerator TimerUpdate()
    {
        yield return new WaitForSeconds(DeafaultOneTick);
        _time -= DeafaultOneTick;

        if (_time <= 0)
        {
            OnTimerUpdate?.Invoke(0);
            yield break;
        }

        OnTimerUpdate?.Invoke(GetPercentOfTotalTime());
        _timerCoroutine = StartCoroutine(TimerUpdate());
    }

    private float GetPercentOfTotalTime()
    {
        return 1 - ((_startTime - _time) / _startTime);
    }

    private void OnDestroy()
    {
        LevelProgress.Instance.OnCompletedLevel -= StopTimer;
    }
}
