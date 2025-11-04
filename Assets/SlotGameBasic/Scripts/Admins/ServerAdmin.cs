using System;
using UnityEngine;

public class ServerAdmin : BaseAdmin
{
    public event Action<SpinResult> onSpinResult;

    public async void Spin()
    {
        var request = new SpinRequest()
        {
            path = "spin_result",
            bet = 5,
            lines = 3,
            userID = "myUserID"
        };
        var result = await game.serverAPI.GetSpinResult(request);

        if (result == null)
        {
            Debug.LogError("SpinRequest failed or server not available!");
        }
        
        onSpinResult?.Invoke(result);
    }
}