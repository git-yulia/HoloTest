using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;

namespace HoloTest_Namespace
{
    public class PlayModeTests
    {
        private GameObject go_SceneLogic;
        private SceneLogic SceneLogicScript;

        [UnitySetUp]
        public void Setup()
        {
            SceneManager.LoadScene("ChemClub");
            Debug.Log("Called setup and loaded the scene");
        }

        [UnityTest]
        public IEnumerator Slider_TurnFlameOff()
        {
            int pause_time = 2;
            SceneManager.LoadScene("ChemClub");

            GameObject slider_object = GameObject.Find("PinchSlider");
            SliderLogic slider_script = slider_object.GetComponent<SliderLogic>();
            PinchSlider slider_ux = slider_object.GetComponent<PinchSlider>();

            GameObject fire_object = GameObject.Find("fire_particle_system");
            FireHandler fire_handler = fire_object.GetComponent<FireHandler>(); 

            slider_ux.SliderValue = 3;
            yield return new WaitForSeconds(pause_time);
            var current_height = fire_handler.GetFlameHeight();

            // At this point, the flame height should be non-zero
            Assert.GreaterOrEqual(current_height, 0.001);
        }

        #region FutureTests
        [UnityTest, Ignore("Not implemented yet.")]
        public IEnumerator CheckFramerate()
        {
            GameObject go_camera = GameObject.Find("main_camera");
            Assert.NotNull(go_camera);
            yield return new WaitForSeconds(5);

            go_SceneLogic = GameObject.Find("Scene");
            SceneLogicScript = go_SceneLogic.GetComponent<SceneLogic>();
            float framerate = SceneLogicScript.GetAverageFramerate();
            Assert.GreaterOrEqual(framerate, 60);
            yield return null;
        }

        ///
        /// Test validates throw behavior on manipulation handler. 
        /// Box with disabled gravity should travel a certain distance 
        /// when being released from grab during hand movement. 
        ///
        [UnityTest, Ignore("Not implemented yet.")]
        public IEnumerator ObjectManipulatorThrow()
        {
            yield return null;
        }

        // Check that the correct profile is being used. 
        // This profile is optimized for the HoloLens 2. 
        [Test, Ignore("Not implemented yet.")]
        public void CheckConfigProfile()
        {
            string profile_path = "Assets/CustomProfiles/HoloLens2_Optimized_Profile.asset";
            var optimized_profile = AssetDatabase.LoadAssetAtPath<MixedRealityToolkitConfigurationProfile>(profile_path);
            Assert.AreEqual(optimized_profile, MixedRealityToolkit.Instance.ActiveProfile);
        }
        #endregion
    }
}
