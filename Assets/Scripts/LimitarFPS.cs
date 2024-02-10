using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitarFPS : MonoBehaviour
{
    public int targetFrameRate;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }
}
