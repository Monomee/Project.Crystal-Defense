using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI timerText;

    private float _elapsedSeconds = 0f;
    private Tween _timerTween;

    void Start()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        _elapsedSeconds = 0f;

        _timerTween = DOTween.To(() => _elapsedSeconds, x => _elapsedSeconds = x, 3600f, 3600f)
            .SetEase(Ease.Linear) 
            .OnUpdate(() => {
                UpdateTimerUI(_elapsedSeconds);
            });
    }

    void UpdateTimerUI(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void PauseTimer() => _timerTween?.Pause();
    public void ResumeTimer() => _timerTween?.Play();

    private void OnDestroy()
    {
        _timerTween?.Kill();
    }
}
