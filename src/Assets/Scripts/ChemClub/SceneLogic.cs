using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneLogic : MonoBehaviour
{
    private float average_framerate;

    void Start()
    {
        average_framerate = 0F;
    }

    public float GetAverageFramerate()
    {
        return (average_framerate = Time.frameCount / Time.time);
    }
}
