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
    public class EditModeTests
    {
        private const string testSceneName = "ChemClub";
        private const string testScenePath = "Assets/Scenes/ChemClub.unity";

        /// <summary>
        /// The Setup function runs before any of the tests do. 
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Open the test scene used in this demo.
            EditorSceneManager.OpenScene(testScenePath);
        }

        /// <summary>
        /// Example that shows how you can open a specific scene 
        /// in the Editor and check that it is now the active scene.
        /// </summary>
        [Test]
        public void OpenValidScene()
        {
            Scene testScene = EditorSceneManager.OpenScene(testScenePath);
            Assert.IsTrue(testScene.IsValid());

            var activeSceneName = EditorSceneManager.GetActiveScene().name; 
            Assert.AreEqual(activeSceneName, testSceneName);
        }

        /// <summary>
        /// As an example, you can check the opposite - that a 
        /// nonexistent scene will not open. 
        /// </summary>
        [Test]
        public void DoNotOpenInvalidScene()
        {
            Scene fakeScene = new Scene();
            try
            {
                fakeScene = EditorSceneManager.OpenScene("Assets/Scenes/ThisSceneDoesntExist.unity");
            }
            catch
            {
                Debug.Log("Could not open nonexistent scene! (And that's good.)");
            }
            finally
            {
                Assert.IsFalse(fakeScene.IsValid());
            }
        }

        /// <summary>
        /// This examples shows how you could check that the initial 
        /// size of an object is correct. 
        /// </summary>
        [Test]
        public void CheckBurnerHeight()
        {
            // First, check that the game object exists at all 
            GameObject burner = GameObject.Find("bunsen_burner");
            Assert.IsNotNull(burner);

            // You could use other components, such as Colliders.
            Renderer renderer = burner.GetComponent<Renderer>();

            // Transform.localScale corresponds to the scale values
            // visible inside the inspector. 
            float burnerHeight = renderer.transform.localScale.y;
            Assert.GreaterOrEqual(burnerHeight, 0.4);
        }

        /// <summary>
        /// This example checks that the camera has not been moved
        /// from the origin point, since this would upset MRTK.
        /// </summary>
        [Test]
        public void CheckInitialCameraSettings()
        {
            GameObject go_MainCamera = GameObject.Find("main_camera");
            Assert.IsNotNull(go_MainCamera);

            Vector3 camera_position = go_MainCamera.transform.position;
            Vector3 expected_position = new Vector3(0.0f, 0.0f, 0.0f);
            Assert.AreEqual(expected_position, camera_position);
        }
    }
}
