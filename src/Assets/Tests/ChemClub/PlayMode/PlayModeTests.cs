using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Tests; 
using UnityEditor.Build;

namespace HoloTest_Namespace
{
    public class PlayModeTests
    {
        const string testSceneName = "ChemClub";
        const string testScenePath = "Assets/Scenes/ChemClub.unity";

        private GameObject button;
        private PressableButtonHoloLens2 buttonComponent; 

        public IEnumerator LoadTestScene()
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(testSceneName);
            loadOp.allowSceneActivation = true;
            while (!loadOp.isDone)
            {
                yield return null;
            }
            yield return true; 
        }

        /// <summary>
        /// Developers can change the MR Toolkit profile by clicking on the 
        /// MixedRealityToolkit game object and setting the profile through 
        /// the inspector.
        /// </summary>
        [UnityTest]
        public IEnumerator CheckMixedRealityConfiguration()
        {
            yield return LoadTestScene();
            var MRTK_gameObject = GameObject.Find("MixedRealityToolkit");
            var configProfile = MRTK_gameObject.GetComponent<MixedRealityToolkit>();
            Debug.Log("Active profile used in this scene - " + configProfile.ActiveProfile.name);
            Assert.AreEqual(configProfile.ActiveProfile.name, "DefaultHololens2XRSDKConfigurationProfile");
        }

        [UnityTest]
        public IEnumerator CheckSliderExistsAtRuntime()
        {
            yield return LoadTestScene();
            GameObject slider = GameObject.Find("PinchSlider");
            Assert.IsNotNull(slider);
        }

        /// <summary>
        /// This test sets the slider to a random value and checks
        /// if the flame height changes in response. 
        /// </summary>
        [UnityTest]
        public IEnumerator CheckFireHeightMatchesSliderValue()
        {
            yield return LoadTestScene();
            GameObject pinchSlider = GameObject.Find("PinchSlider");
            Assert.IsNotNull(pinchSlider);
            GameObject burnerObject = GameObject.Find("bunsen_burner");
            Assert.IsNotNull(burnerObject);

            SliderLogic slider = pinchSlider.GetComponent<SliderLogic>();
            FireHandler fireHandler = burnerObject.GetComponent<FireHandler>();

            // Set the slider value to some arbitrary value between 0 and 1
            float randomHeight = Random.Range(0f, 1f); 
            slider.SetSliderValue(randomHeight);
            float sliderValue = slider.GetSliderValue();
            Assert.AreEqual(sliderValue, randomHeight);

            // Check if the flame is as tall as it should be using scaled slider value
            float currentHeight = (fireHandler.GetFlameHeight());
            float expected_height = (randomHeight * fireHandler.sliderScaleFactor);

            Assert.AreEqual(expected_height, currentHeight);
        }

        /// <summary>
        /// This test checks that adjustments to the fire animation
        /// do not cause unwanted changes to the particle system - 
        /// in this case, disabling the system entirely. 
        /// </summary>
        [UnityTest]
        public IEnumerator CanChangeFireAnimation()
        {
            yield return LoadTestScene();
            
            GameObject burnerObject = GameObject.Find("bunsen_burner");
            FireHandler fireHandler = burnerObject.GetComponent<FireHandler>();

            // Set the flame height to some arbitrary value
            float randomHeight = Random.Range(0.3f, 1f);
            fireHandler.SetFlameHeightUsingSlider(randomHeight);

            bool isEmitting = fireHandler.isFireEmitting();
            Assert.AreEqual(true, isEmitting);
            fireHandler.PlayNextAnimation();
            isEmitting = fireHandler.isFireEmitting();
            Assert.AreEqual(true, isEmitting);
        }
    }
}


