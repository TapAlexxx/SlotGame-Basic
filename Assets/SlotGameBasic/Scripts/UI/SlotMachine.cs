using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public sealed class SlotMachinePopup : UIBase
{
    [SerializeField, NotNull] private Button spinButton;
    [SerializeField] private List<Image> images;

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
        spinButton.interactable = true;
        ShowSpinResult(spinResult.finalGrid);
    }
    
    private void ShowSpinResult(string[][] grid)
    {
        for (int row = 0; row < grid.Length; row++)
        {
            for (int column = 0; column < grid[row].Length; column++)
            {
                string symbolName = grid[row][column];
                
                Sprite sprite = admin.game.configAdmin.symbolIcons.Get(symbolName);

                images[row * grid[row].Length + column].sprite = sprite;
            }
        }
    }
}

