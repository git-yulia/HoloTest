using UnityEngine;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;
using UnityEditor;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities.Editor;

namespace HoloTest_Namespace
{
    public class EditModeTests
    {
        public void Setup()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/ChemClub.unity");
        }

        [Test]
        // [UnityPlatform (RuntimePlatform.WindowsPlayer)]
        public void CheckCameraSettings()
        {
            //EditorSceneManager.OpenScene("Assets/Scenes/ChemClub.unity");
            // Test that the camera has not been moved from the origin point. 
            GameObject go_MainCamera = GameObject.Find("main_camera");
            Assert.IsNotNull(go_MainCamera);
            Vector3 camera_position = go_MainCamera.transform.position;
            Vector3 expected_position = new Vector3(0.0f, 0.0f, 0.0f);
            Assert.AreEqual(expected_position, camera_position);
        }
    }
}
