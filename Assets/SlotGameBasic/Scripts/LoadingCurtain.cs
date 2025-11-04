using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public sealed class LoadingCurtain : UIBase
{
    [SerializeField] private Slider slider;
    private TweenerCore<float, float, FloatOptions> refreshTween;

    public void RefreshLoadingProgress(float progress, Action onComplete)
    {
        refreshTween?.Kill();
        refreshTween = slider.DOValue(progress, 0.5f).SetEase(Ease.OutBounce).OnComplete(() => onComplete?.Invoke());
    }

    protected override void OnInit()
    {
        game.gameLoader.onProgressChanged += RefreshLoadingProgress;
    }

    protected override void OnShutDown()
    {
        game.gameLoader.onProgressChanged -= RefreshLoadingProgress;
    }

    protected override void OnUIShow()
    {
        base.OnUIShow();
        
        slider.value = 0.0f;
    }

    private void RefreshLoadingProgress(float progress)
    {
        refreshTween?.Kill();
        refreshTween = slider.DOValue(progress, 0.2f).SetEase(Ease.Linear);
    }
}