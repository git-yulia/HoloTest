using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities.Editor;

public class FireHandler : MonoBehaviour
{
    public PinchSlider slider { get; set; }

    #region Private Members
    private ParticleSystem fire;
    private ParticleSystem.Particle[] fire_particles;
    #endregion

    void Awake()
    {
        fire = GetComponent<ParticleSystem>();
        fire_particles = new ParticleSystem.Particle[fire.main.maxParticles];
        SetFlameHeight(0);

        // In the long run, this script should pull values 
        // from the particle system itself - like startx, starty, etc.
        // And then use those to set the scale for each function. 
    }

    public void SetFlameHeight(float goal_height)
    {
        if (goal_height < 0.001)
        {
            fire.Stop();
        }
        else if (goal_height > 0.001 && goal_height < 0.05)
        {
            // Here you might want to set special 
            // flame properties to smooth out the 
            // transition between off and on states
        }

        var main = fire.main;
        main.startLifetime = goal_height;

        if (fire.isStopped && goal_height > 0.001)
        {
            fire.Play(); 
        }
    }

    public void SetFlameHeightUsingSlider(float slider_value)
    {
        SetFlameHeight(slider_value * 0.2f);
    }

    public void ToggleBurner(bool toggle_value)
    {
        if (toggle_value == false)
        {
            fire.Stop();
        }
        else
        {
            fire.Play(); 
        }
    }
}
