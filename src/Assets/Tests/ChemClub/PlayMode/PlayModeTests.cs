using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using UnityEditor.Build;
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

namespace HoloTest_Namespace
{
    public class PlayModeTests
    {
        private const string testSceneName = "ChemClub";
        private const string configProfilePath = "Assets/MixedRealityToolkit.Generated/CustomProfiles";
        private const string configProfileName = "/HoloTestConfigurationProfile.asset";

        /// <summary>
        /// This setup function calls PlayModeTestUtilities 
        /// from MRTK - but you can also create and call a 
        /// custom Setup procedure. 
        /// </summary>
        [UnitySetUp]
        public IEnumerator Setup()
        {
            // The Setup function used here also takes an 
            // optional argument for the MRTK configuration
            // profile. This is super useful. 
            //
            // The test profile was cloned using the HLens2
            // XR SDK profile, for context.

            var profile = AssetDatabase.LoadAssetAtPath
                          <MixedRealityToolkitConfigurationProfile>
                          (configProfilePath + configProfileName); 

            PlayModeTestUtilities.Setup(profile);
            TestUtilities.PlayspaceToOriginLookingForward();
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            PlayModeTestUtilities.TearDown();
            yield return null;
        }

        #region Utilities

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

        #endregion

        [UnityTest, Ignore("This test should never run, on principle.")]
        public IEnumerator UnwantedTest()
        {
            // Although this test would obviously fail, 
            // the developer was smart enough to hide it 
            // using the Ignore attribute.  
            //
            Assert.AreEqual(Color.red, Color.green);
            yield return null; 
        }

        /// <summary>
        /// Checks the active Mixed Reality Toolkit configuration profile. 
        /// </summary>
        [UnityTest]
        public IEnumerator CheckMixedRealityConfiguration()
        {
            var profile = AssetDatabase.LoadAssetAtPath
                          <MixedRealityToolkitConfigurationProfile>
                          (configProfilePath + configProfileName);

            //Assert.IsNotNull(profile);
            //Assert.IsTrue(profile);

            yield return null; 



            /* yield return LoadTestScene();
             var MRTK_gameObject = GameObject.Find("MixedRealityToolkit");
             var configProfile = MRTK_gameObject.GetComponent<MixedRealityToolkit>();
             Debug.Log("Active profile used in this scene - " + configProfile.ActiveProfile.name);
             Assert.AreEqual(configProfile.ActiveProfile.name, "DefaultHololens2XRSDKConfigurationProfile");*/
        }

        /// <summary>
        /// This example shows how you can test an MRTK pinch slider
        /// and some game object that should react to that input. 
        /// </summary>
        [UnityTest, Ignore("Fixing tests")]
        public IEnumerator CheckFireHeightMatchesSliderValue()
        {
            yield return LoadTestScene();

            // Check that the objects exist. 
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
        /// This example shows how you can check out the 
        /// properties of a particle system. 
        /// </summary>
        [UnityTest, Ignore("Fixing tests")]
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


