using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    public Text fpsText;
    float time;
    int frameCount;

    void Update()
    {
        time += Time.unscaledDeltaTime;
        frameCount++;
        if (time >= 1 && frameCount >= 1)
        {
            float fps = frameCount / time;
            time = 0;
            frameCount = 0;
            fpsText.text = fps.ToString("f2");
        }
    }
}
