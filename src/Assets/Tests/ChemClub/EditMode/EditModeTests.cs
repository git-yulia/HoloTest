using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities.Editor;

namespace HoloTest_Namespace
{
    // You can also organize tests with the following attribute:
    // [UnityPlatform (RuntimePlatform.WindowsPlayer)]

    public class EditModeTests
    {
        [Test]
        public void CheckSceneLoadsCorrectly()
        {
            var scene = EditorSceneManager.OpenScene("Assets/Scenes/ChemClub.unity");
            Assert.IsTrue(scene.IsValid());
        }

        [Test]
        public void InvalidSceneDoesNotLoad()
        {
            Scene scene = new Scene(); 

            try
            {
                scene = EditorSceneManager.OpenScene("Assets/Scenes/ThisSceneDoesntExist.unity");
            }
            catch
            {
                Debug.Log("Could not open this scene, since it doesn't exist! (And that's good.)");
            }
            finally
            {
                Assert.IsFalse(scene.IsValid());
            }
        }

        [Test]
        public void CheckCameraSettings()
        {
            // Test that the camera has not been moved from the origin point. 
            EditorSceneManager.OpenScene("Assets/Scenes/ChemClub.unity");
            GameObject go_MainCamera = GameObject.Find("main_camera");
            Assert.IsNotNull(go_MainCamera);
            Vector3 camera_position = go_MainCamera.transform.position;
            Vector3 expected_position = new Vector3(0.0f, 0.0f, 0.0f);
            Assert.AreEqual(expected_position, camera_position);
        }
    }
}
