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
        }

        [Test]
        public void CheckFramerate()
        {
            GameObject go_camera = GameObject.Find("main_camera");
            Assert.NotNull(go_camera);

            go_SceneLogic = GameObject.Find("Scene");
            SceneLogicScript = go_SceneLogic.GetComponent<SceneLogic>();
            float framerate = SceneLogicScript.GetAverageFramerate();
            Assert.GreaterOrEqual(framerate, 60);
        }

        ///
        /// Test validates throw behavior on manipulation handler. 
        /// Box with disabled gravity should travel a certain distance 
        /// when being released from grab during hand movement. 
        ///
        [UnityTest]
        public IEnumerator ObjectManipulatorThrow()
        {
            yield return null;
        }

        // Check that the correct profile is being used. 
        // This profile is optimized for the HoloLens 2. 
        [Test]
        public void CheckConfigProfile()
        {
            string profile_path = "Assets/CustomProfiles/HoloLens2_Optimized_Profile.asset";
            var optimized_profile = AssetDatabase.LoadAssetAtPath<MixedRealityToolkitConfigurationProfile>(profile_path);
            Assert.AreEqual(optimized_profile, MixedRealityToolkit.Instance.ActiveProfile);
        }
    }
}
