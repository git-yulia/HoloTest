using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities.Editor;

namespace HoloTest_Namespace
{
    public class FireHandler : MonoBehaviour
    {
        public PinchSlider slider { get; set; }

        public GameObject firePrefabRed;
        public GameObject firePrefabBlue;
        public GameObject firePrefabMixedLow;

        #region Private Members
        private ParticleSystem fire;
        private ParticleSystem.Particle[] fireParticles;
        private GameObject activeFirePrefab; 
        #endregion

        private void Start()
        {
            // In the long run, this script should pull values 
            // from the particle system itself - like startx, starty, etc.
            // And then use those to set the scale for each function. 

            // Get current location of burner
            Vector3 burnerLocation = gameObject.transform.position;
            Vector3 burnerSize = gameObject.GetComponent<Collider>().bounds.size;
            Vector3 flamePlacement = burnerLocation;
            flamePlacement.y += 0.74f; 

            activeFirePrefab = Instantiate(firePrefabMixedLow, flamePlacement, Quaternion.identity);

            fire = activeFirePrefab.GetComponent<ParticleSystem>();
            fireParticles = new ParticleSystem.Particle[fire.main.maxParticles];

            SetFireAnimation(firePrefabBlue);

            SetFlameHeight(0);
        }

        public IEnumerator SetFireAnimation(GameObject firePrefab)
        {
            yield return new WaitForSeconds(3); 

            // Deactivate the old prefab if it exists
            if (activeFirePrefab != null)
            {
                activeFirePrefab.SetActive(false);
            }

            // Activate the instance if it is already in the scene
            if (GameObject.Find(firePrefab.name))
            {
                firePrefab.SetActive(true);
            }
            else
            {
                Vector3 burnerLocation = gameObject.transform.position;
                Vector3 flamePlacement = burnerLocation;
                activeFirePrefab = Instantiate(firePrefab, flamePlacement, Quaternion.identity);
            }
        }

        public float GetFlameHeight()
        {
            // For now, the property used to control height 
            // is startLifetime, so this is what is returned. 
            var main = fire.main;
            return main.startLifetime.constant;
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
            SetFlameHeight(sliderValue * 0.2f);
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
