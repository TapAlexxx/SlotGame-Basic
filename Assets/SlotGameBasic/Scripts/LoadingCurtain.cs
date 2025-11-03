using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public sealed class LoadingCurtain : UIBase
{
    [SerializeField] private Slider slider;

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
        slider.DOValue(progress, 0.1f).SetEase(Ease.OutBounce);
    }
}