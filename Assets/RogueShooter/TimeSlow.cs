using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlow : MonoBehaviour
{
    public void StartSlowMotion()
    {
        SetTimeScale(0.3f);
    }
    public void StopSlowMotion()
    {
        SetTimeScale(1f);
    }
    public void StopTime()
    {
        Time.timeScale = 0;
    }

    private void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}
