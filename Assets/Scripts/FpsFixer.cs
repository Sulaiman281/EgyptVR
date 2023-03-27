using UnityEngine;

public class FpsFixer : MonoBehaviour
{
    private void Awake()
    {
        if (Unity.XR.Oculus.Performance.TryGetDisplayRefreshRate(out var rate))
        {
            var newRate = 90f;
            // if (Unity.XR.Oculus.Performance.TryGetAvailableDisplayRefreshRates(out var rates))
            // {
            //     newRate = rates.Max();
            // }

            if (rate < newRate)
            {
                if (Unity.XR.Oculus.Performance.TrySetDisplayRefreshRate(newRate))
                {
                    Time.fixedDeltaTime = 1f / newRate;
                    Time.maximumDeltaTime = 1f / newRate;
                }
                // else
                // {
                //     if (Unity.XR.Oculus.Performance.TrySetDisplayRefreshRate(90))
                //     {
                //         Time.fixedDeltaTime = 1f / 90;
                //         Time.maximumDeltaTime = 1f / 90;
                //     }
                // }
            }
        }
    }
}
