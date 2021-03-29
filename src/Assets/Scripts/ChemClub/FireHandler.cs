using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities.Editor;

namespace HoloTest
{
    public class FireHandler : MonoBehaviour
    {
        public PinchSlider slider { get; set; }

        public GameObject firePrefabRed;
        public GameObject firePrefabBlue;
        public GameObject firePrefabMixedLow;

        public float sliderScaleFactor = 0.2f; 

        #region Private Members
        private ParticleSystem fire;
        private ParticleSystem.Particle[] fireParticles;
        private GameObject activeFirePrefab;
        private GameObject saved_firePrefabRed;
        private GameObject saved_firePrefabBlue;
        private GameObject saved_firePrefabMixedLow;
        #endregion

        private void Start()
        {
            // In the long run, this script should pull values 
            // from the particle system itself - like startx, starty, etc.
            // And then use those to set the scale for each function. 

            // Get current location of burner
            Vector3 burnerLocation = gameObject.transform.position;
            Vector3 flamePlacement = burnerLocation;
            flamePlacement.y += 0.74f;

            // Initiate all 3 animation types and save handles to them.
            saved_firePrefabRed = Instantiate(firePrefabRed, flamePlacement, Quaternion.identity);
            saved_firePrefabRed.SetActive(false); 
            saved_firePrefabBlue = Instantiate(firePrefabBlue, flamePlacement, Quaternion.identity);
            saved_firePrefabBlue.SetActive(false);
            saved_firePrefabMixedLow = Instantiate(firePrefabMixedLow, flamePlacement, Quaternion.identity);
            saved_firePrefabMixedLow.SetActive(false);

            // Set default fire prefab
            activeFirePrefab = firePrefabMixedLow;
            activeFirePrefab.SetActive(true);
            SetFireAnimation(saved_firePrefabMixedLow);
            fire = activeFirePrefab.GetComponent<ParticleSystem>();
            fireParticles = new ParticleSystem.Particle[fire.main.maxParticles];
            SetFlameHeight(0);
        }

        // spaghetti
        public int GetFireType()
        {
            int fireType = 0;

            if (activeFirePrefab == saved_firePrefabMixedLow)
            {
                fireType = 0;
            }
            else if (activeFirePrefab == saved_firePrefabRed)
            {
                fireType = 1;
            }
            else
            {
                fireType = 2;
            }

            return fireType; 
        }

        public void GetFireState(out int fireType, out float fireHeight)
        {
            fireType = 0;
            fireHeight = 0;

            fireHeight = GetFlameHeight(); 

            if (activeFirePrefab == saved_firePrefabMixedLow)
            {
                fireType = 0; 
            }
            else if (activeFirePrefab == saved_firePrefabRed)
            {
                fireType = 1;
            }
            else
            {
                fireType = 2; 
            }
        }

        public void SetFireAnimation(GameObject firePrefab)
        {
            // Deactivate the current fire animation
            activeFirePrefab.SetActive(false);

            firePrefab.SetActive(true);

            activeFirePrefab = firePrefab;
            activeFirePrefab.SetActive(true); 

            // Update active particle system
            fire = activeFirePrefab.GetComponent<ParticleSystem>();
            fireParticles = new ParticleSystem.Particle[fire.main.maxParticles];
        }

        public void PlayNextAnimation()
        {
            if (activeFirePrefab == saved_firePrefabMixedLow)
            {
                SetFireAnimation(saved_firePrefabRed);
            }
            else if (activeFirePrefab == saved_firePrefabRed)
            {
                SetFireAnimation(saved_firePrefabBlue);
            }
            else if (activeFirePrefab == saved_firePrefabBlue)
            {
                SetFireAnimation(saved_firePrefabMixedLow);
            }
        }

        public float GetFlameHeight()
        {
            // For now, the property used to control height 
            // is startLifetime, so this is what is returned. 
            var main = fire.main;
            return main.startLifetime.constant;
        }

        public bool isFireEmitting()
        {
            return fire.isEmitting; 
        }

        public void SetFlameHeight(float goalHeight)
        {
            if (goalHeight < 0.001)
            {
                fire.Stop();
            }
            else if (goalHeight > 0.001 && goalHeight < 0.05)
            {
                // Here you might want to set special 
                // flame properties to smooth out the 
                // transition between off and on states
            }

            var main = fire.main;
            main.startLifetime = goalHeight;

            if (fire.isStopped && goalHeight > 0.001)
            {
                fire.Play();
            }
        }

        public void SetFlameHeightUsingSlider(float sliderValue)
        {
            SetFlameHeight(sliderValue * sliderScaleFactor);
        }

        public void ToggleBurner(bool toggleValue)
        {
            if (toggleValue == false)
            {
                fire.Stop();
            }
            else
            {
                fire.Play();
            }
        }
    }
}
