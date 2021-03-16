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
using UnityEditor;
using UnityEditor.Build;

namespace HoloTest_Namespace
{
    public class PlayModeTests : IPrebuildSetup, IPostBuildCleanup
    {
        private GameObject go_SceneLogic;
        private SceneLogic SceneLogicScript;

        // [UnitySetUp]
        public void Setup() 
        {
            //EditorSceneManager.OpenScene("Assets/Scenes/ChemClub.unity");
            //SceneManager.LoadScene("ChemClub");
            Debug.Log("Called setup from here and loaded the scene");
            var active_scene = SceneManager.GetActiveScene();
            Debug.Log("active scene is currently: " + active_scene.name);
        }

        public void Cleanup()
        {
            var active_scene = SceneManager.GetActiveScene();
            SceneManager.UnloadSceneAsync(active_scene);
        }

        [Test]
        public void Misc()
        {
            var active_scene = SceneManager.GetActiveScene();
            Debug.Log("active scene is currently: " + active_scene.name);
        }

        [UnityTest]
        public IEnumerator GameObject_WithRigidBody_WillBeAffectedByPhysics()
        {
            var go = new GameObject();
            go.AddComponent<Rigidbody>();
            var originalPosition = go.transform.position.y;

            yield return new WaitForFixedUpdate();

            Assert.AreNotEqual(originalPosition, go.transform.position.y);
        }

        #region FutureTests

        /*        [Test]
                public void CheckSceneLoadsCorrectly()
                {
                    SceneManager.LoadScene("Assets/Scenes/ChemClub.unity");
                    var active_scene = SceneManager.GetActiveScene();
                    Debug.Log("active scene is currently: " + active_scene.name);
                }*/

        /*        [UnityTest]
                // [UnityPlatform (RuntimePlatform.WindowsPlayer)]
                public IEnumerator Slider_TurnFlameOff()
                {
                    int pause_time = 2;

                    GameObject slider_object = GameObject.Find("PinchSlider");
                    PinchSlider slider_ux = slider_object.GetComponent<PinchSlider>();

                    GameObject fire_object = GameObject.Find("fire_particle_system");
                    FireHandler fire_handler = fire_object.GetComponent<FireHandler>(); 

                    slider_ux.SliderValue = 3;
                    yield return new WaitForSeconds(pause_time);
                    var current_height = fire_handler.GetFlameHeight();

                    // At this point, the flame height should be non-zero
                    Assert.GreaterOrEqual(current_height, 0.001);
                }*/

        /*[UnityTest, Ignore("Not implemented yet")]
        public IEnumerator CheckFramerate()
        {
            SceneManager.LoadScene("Assets/Scenes/ChemClub.unity");
            var active_scene = SceneManager.GetActiveScene();
            Debug.Log("active scene is currently: " + active_scene.name);

            GameObject go_camera = GameObject.Find("main_camera");
            Assert.NotNull(go_camera);
            yield return new WaitForSeconds(2);

            go_SceneLogic = GameObject.Find("World");
            SceneLogicScript = go_SceneLogic.GetComponent<SceneLogic>();
            float framerate = SceneLogicScript.GetAverageFramerate();
            Assert.GreaterOrEqual(framerate, 60);
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
        }*/
        #endregion
    }
}
