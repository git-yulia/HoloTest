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
        /// <summary>
        /// The SetUp function runs before any of the tests do. 
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Open the test scene used in this demo.
            string sceneName = "Assets/Scenes/ChemClub.unity";
            Scene scene = EditorSceneManager.OpenScene(sceneName);
        }

        /// <summary>
        /// Example that shows how you can open a specific scene 
        /// in the Editor.
        /// </summary>
        [Test]
        public void CheckScenesOpenCorrectly()
        {
            string sceneName = "Assets/Scenes/ChemClub.unity";
            Scene scene = EditorSceneManager.OpenScene(sceneName);
            Assert.IsTrue(scene.IsValid());

            // As an example, you can check the inverse - that a 
            // nonexistent scene does NOT open. 
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

            // I open the swap scene here to check that SetUp is 
            // indeed running before everything else does. 
            Scene swapScene = EditorSceneManager.OpenScene("Assets/Scenes/SwapScene.unity");
            Assert.IsTrue(swapScene.IsValid());
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

            // You can also use other components, such as Colliders.
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
