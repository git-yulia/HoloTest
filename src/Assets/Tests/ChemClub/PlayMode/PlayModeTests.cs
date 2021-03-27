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
using Microsoft.MixedReality.Toolkit.Tests;
using Microsoft.MixedReality.Toolkit.Utilities;

namespace HoloTest_Namespace
{
    public class PlayModeTests
    {
        private const string testSceneName = "ChemClub";
        private const string testProfileName = "HoloTestConfigurationProfile";

        #region Surprise Tools That Will Help Us Later
        private const string testProfilePath = "Assets/MixedRealityToolkit.Generated/CustomProfiles" +
                                               "/HoloTestConfigurationProfile.asset";
        #endregion  

        [UnitySetUp]
        public IEnumerator Setup()
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(testSceneName);
            loadOp.allowSceneActivation = true;
            while (!loadOp.isDone)
            {
                yield return null;
            }
            yield return true;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Scene scene = SceneManager.GetSceneByName(testProfileName);
            if (scene.isLoaded)
            {
                // Not sure why Unity requires empty scene to be created first - 
                // need to investigate this. 
                Scene playModeTestScene = SceneManager.CreateScene("Empty");
                SceneManager.SetActiveScene(playModeTestScene);
                SceneManager.UnloadSceneAsync(scene.buildIndex);
            }
            yield return null;
        }

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
        public IEnumerator CheckMixedRealityConfigurationProfile()
        {
            var MRTK_gameObject = GameObject.Find("MixedRealityToolkit");
            var activeProfile = MRTK_gameObject.GetComponent<MixedRealityToolkit>();

            Debug.Log("Active profile used in this scene - " + activeProfile.ActiveProfile.name);
            Assert.AreEqual(activeProfile.ActiveProfile.name, testProfileName);

            yield return null;
        }

        /// <summary>
        /// This example tests scripts attached to GameObjects - 
        /// in this case, the burner and an MRTK PinchSlider.
        /// </summary>
        [UnityTest]
        public IEnumerator CheckFireHeightMatchesSliderValue()
        {
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

            yield return null; 
        }

        /// <summary>
        /// This example shows how you can inspect the 
        /// properties of a particle system. 
        /// </summary>
        [UnityTest]
        public IEnumerator CanChangeFireAnimation()
        {
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

            yield return null;
        }

        // This is kind of neat. For future me to look at: 
        //
        //  ProcessYAMLAssets(allFilesUnderAssets, Application.dataPath.
        //  Replace("Assets", "NuGet/Content"), remapDictionary, compiledClassReferences);
    }
}


