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

namespace HoloTest
{
    /// <summary>
    /// This classes borrows utility functions from MRTK 
    /// for demonstration purposes. 
    /// </summary>
    public class InputSimulationTests
    {
        private const string testSceneName = "ChemClub";
        private const string testProfileName = "HoloTestConfigurationProfile";

        private TestHand hand;
        private InputSimulationService simulationService;

        [UnitySetUp]
        public IEnumerator Setup()
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(testSceneName, LoadSceneMode.Single);
            loadOp.allowSceneActivation = true;
            while (!loadOp.isDone)
            {
                yield return null;
            }
            yield return true;

            // Set scene up using the correct profile
            // ...

            // Modify any Mixed Reality Toolkit settings
            // ...

            // Initialize the PlaySpace
            TestUtilities.InitializePlayspace();

            // Disable user input during tests
            simulationService = GetInputSimulationService();
            simulationService.UserInputEnabled = false;

            // Initialize anything required for input simulation
            hand = new TestHand(Handedness.Right);
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Scene scene = SceneManager.GetSceneByName(testProfileName);
            if (scene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(scene.buildIndex);
            }
            yield return null;
        }

        /// <summary>
        /// This test presses a button using simulated hand input. 
        /// </summary>
        [UnityTest]
        public IEnumerator PressButtonWithHand()
        {
            // Find game objects needed for test
            GameObject testButton = GameObject.Find("pressableButton");
            GameObject burnerObject = GameObject.Find("bunsen_burner");
            FireHandler fireHandler = burnerObject.GetComponent<FireHandler>();
            PressableButton buttonComponent = testButton.GetComponent<PressableButtonHoloLens2>();

            // Check that objects exist
            Assert.IsNotNull(burnerObject);
            Assert.IsNotNull(buttonComponent);

            TestUtilities.PlayspaceToOriginLookingForward();
            testButton.transform.position = new Vector3(0, 0, 1.067121f);
            testButton.transform.localScale = Vector3.one * 1.5f;

            // Acquire the flame type prior to pressing the button
            var startingFlameType = fireHandler.GetFireType(); 

            bool buttonPressed = false;
            buttonComponent.ButtonPressed.AddListener(() =>
            {
                buttonPressed = true;
            });

            // Move the hand forward to press button, then off to the right
            Vector3 p1 = new Vector3(0, 0, 0.5f);
            Vector3 p2 = new Vector3(0, 0, 1.08f);
            Vector3 p3 = new Vector3(0.1f, 0, 1.08f);

            yield return hand.Show(p1);
            yield return hand.MoveTo(p2);
            yield return hand.MoveTo(p3);

            Assert.IsTrue(buttonPressed, "Button was not pressed by the simulated hand.");

            // Now check that the flame type has actually been modified
            var finalFlameType = fireHandler.GetFireType();
            Assert.AreNotEqual(startingFlameType, finalFlameType);

            yield return null; 
        }

        /// <summary>
        /// Example test showing how you can grab and throw
        /// a game object using simulated hand input. 
        /// </summary>
        [UnityTest]
        public IEnumerator GrabAndThrowBeaker()
        {
            var testObject = GameObject.Find("beaker");
            Vector3 initialObjectPosition = testObject.transform.position; 

            var rigidBody = testObject.GetComponent<Rigidbody>();
            rigidBody.useGravity = false;

            var manipHandler = testObject.GetComponent<ObjectManipulator>();
            manipHandler.HostTransform = testObject.transform;
            manipHandler.SmoothingFar = false;
            manipHandler.SmoothingNear = false;

            yield return new WaitForFixedUpdate();
            yield return null;

            Vector3 initialHandPosition = new Vector3(0, 0, 0.5f);
            Vector3 throwPosition = new Vector3(1f, 0f, 1f);

            yield return hand.Show(initialHandPosition);
            yield return hand.MoveTo(initialObjectPosition);
            yield return hand.GrabAndThrowAt(throwPosition, false);

            Assert.NotZero(rigidBody.angularVelocity.magnitude);
            Assert.NotZero(rigidBody.velocity.x);
            Assert.NotZero(rigidBody.velocity.z);
            Assert.AreEqual(hand.GetVelocity(), rigidBody.velocity);
        }

        #region Utility functions borrowed from MRTK

        /// <summary>
        /// Utility function to simplify code for getting access to the running InputSimulationService
        /// </summary>
        /// <returns>Returns InputSimulationService registered for playmode test scene</returns>
        public static InputSimulationService GetInputSimulationService()
        {
            var inputSimulationService = CoreServices.GetInputSystemDataProvider<InputSimulationService>();
            Debug.Assert((inputSimulationService != null), "InputSimulationService is null!");
            return inputSimulationService;
        }

        #endregion
    }
}