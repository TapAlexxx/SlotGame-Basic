using UnityEngine;
using UnityEngine.UI;

public sealed class SlotMachinePopup : UIBase
{
    [SerializeField] private Button spinButton;

    public void SpinHandler()
    {
        spinButton.interactable = false;
        admin.game.serverAdmin.Spin();
    }

    protected override void OnUIShow()
    {
        base.OnUIShow();
        admin.game.serverAdmin.onSpinResult += OnSpinResult;
    }

    protected override void OnUIHide()
    {
        base.OnUIHide();
        admin.game.serverAdmin.onSpinResult -= OnSpinResult;
    }

    private void OnSpinResult(SpinResult spinResult)
    {
        
    }
}