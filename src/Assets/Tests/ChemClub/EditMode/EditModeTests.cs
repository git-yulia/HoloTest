using UnityEngine;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace HoloTest_Namespace
{
    public class EditModeTests
    {
        [UnitySetUp]
        public void Setup()
        {
            // Need to have the right scene open in the editor:
            SceneManager.LoadScene("ChemClub");
        }

        [UnityTearDown]
        public void TearDown()
        {
        }

        [Test]
        public void CheckCameraSettings()
        {
            // Test that the camera has not been moved from the origin point. 
            GameObject go_MainCamera = GameObject.Find("main_camera");
            Assert.IsNotNull(go_MainCamera);
            Vector3 camera_position = go_MainCamera.transform.position;
            Vector3 expected_position = new Vector3(0.0f, 0.0f, 0.0f);
            Assert.AreEqual(expected_position, camera_position);
        }
    }
}
