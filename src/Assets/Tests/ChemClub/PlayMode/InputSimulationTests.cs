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
    public class InputSimulationTests
    {
        private const string testSceneName = "ChemClub";
        private const string testProfileName = "HoloTestConfigurationProfile";

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

            // Modify any Mixed Reality Toolkit settings

            // Initialize the PlaySpace
            InitializePlayspace();

            // Disable user input during tests
            InputSimulationService inputSimulationService = GetInputSimulationService();
            inputSimulationService.UserInputEnabled = false;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Scene scene = SceneManager.GetSceneByName(testProfileName);
            if (scene.isLoaded)
            {
                Scene playModeTestScene = SceneManager.CreateScene("Empty");
                SceneManager.SetActiveScene(playModeTestScene);
                SceneManager.UnloadSceneAsync(scene.buildIndex);
            }
            yield return null;
        }

        [UnityTest]
        public IEnumerator PressButtonWithHand()
        {
            GameObject testButton = GameObject.Find("pressableButton");

            // Move the camera to origin looking at +z to more easily see the button.
            TestUtilities.PlayspaceToOriginLookingForward();

            // For some reason, we would only get null pointers when the hand tries to click a button
            // at specific positions, hence the unusual z value.
            testButton.transform.position = new Vector3(0, 0, 1.067121f);
            // The scale of the button was also unusual in the repro case
            testButton.transform.localScale = Vector3.one * 1.5f;

            PressableButton buttonComponent = testButton.GetComponent<PressableButtonHoloLens2>();
            Assert.IsNotNull(buttonComponent);

            bool buttonPressed = false;
            buttonComponent.ButtonPressed.AddListener(() =>
            {
                buttonPressed = true;
            });

            // Move the hand forward to press button, then off to the right
            Vector3 p1 = new Vector3(0, 0, 0.5f);
            Vector3 p2 = new Vector3(0, 0, 1.08f);
            Vector3 p3 = new Vector3(0.1f, 0, 1.08f);

            TestHand hand = new TestHand(Handedness.Right);
            yield return hand.Show(p1);
            yield return hand.MoveTo(p2);
            yield return hand.MoveTo(p3);

            Assert.IsTrue(buttonPressed, "Button did not get pressed when hand moved to press it.");

            yield return null; 
        }

        [UnityTest, Ignore("Example lifted straight from MRTK tests.")]
        public IEnumerator KeyInputSimulation()
        {
            GameObject cube;
            Interactable interactable;            
            
            // Explicitly enable user input to test in editor behavior.
            InputSimulationService iss = GetInputSimulationService();
            Assert.IsNotNull(iss, "InputSimulationService is null!");
            iss.UserInputEnabled = true;

            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.localPosition = new Vector3(0, 0, 2);
            cube.transform.localScale = new Vector3(.2f, .2f, .2f);

            interactable = cube.AddComponent<Interactable>();

            KeyInputSystem.StartKeyInputStimulation();

            PlayspaceToOriginLookingForward();
            yield return null;

            // Subscribe to interactable's on click so we know the click went through
            bool wasClicked = false;
            interactable.OnClick.AddListener(() => { wasClicked = true; });

            // start click on the cube
            KeyInputSystem.PressKey(iss.InputSimulationProfile.InteractionButton);
            yield return null;
            KeyInputSystem.AdvanceSimulation();
            yield return WaitForInputSystemUpdate();

            // release the click on the cube
            KeyInputSystem.ReleaseKey(iss.InputSimulationProfile.InteractionButton);
            yield return null;
            KeyInputSystem.AdvanceSimulation();
            yield return null;

            // Check to see that the cube was clicked on
            Assert.True(wasClicked);

            yield return null;
        }

        #region Utility Functions from MRTK

        /// <summary>
        /// Utility function to simplify code for getting access to the running InputSimulationService
        /// </summary>
        /// <returns>Returns InputSimulationService registered for playmode test scene</returns>
        public static InputSimulationService GetInputSimulationService()
        {
            InputSimulationService inputSimulationService = CoreServices.GetInputSystemDataProvider<InputSimulationService>();
            Debug.Assert((inputSimulationService != null), "InputSimulationService is null!");
            return inputSimulationService;
        }

        /// <summary>
        /// Pose to create MRTK playspace's parent transform at.
        /// </summary>
        public static Pose ArbitraryParentPose { get; set; } = new Pose(new Vector3(-2.0f, 1.0f, -3.0f), Quaternion.Euler(-30.0f, -90.0f, 0.0f));

        /// <summary>
        /// Pose to set playspace at, when using <see cref="PlayspaceToArbitraryPose"/>. 
        /// </summary>
        public static Pose ArbitraryPlayspacePose { get; set; } = new Pose(new Vector3(6.0f, 2.0f, 8.0f), Quaternion.Euler(10.0f, 120.0f, 15.0f));

        /// <summary>
        /// Creates a playspace and moves it into a default position.
        /// </summary>
        public static void InitializePlayspace()
        {
            if (MixedRealityPlayspace.Transform.parent == null)
            {
                GameObject gameObject = new GameObject("MRTKPlayspaceTestParent");
                MixedRealityPlayspace.Transform.parent = gameObject.transform;
                gameObject.transform.position = ArbitraryParentPose.position;
                gameObject.transform.rotation = ArbitraryParentPose.rotation;
            }
            MixedRealityPlayspace.PerformTransformation(
            p =>
            {
                p.position = new Vector3(1.0f, 1.5f, -2.0f);
                p.LookAt(Vector3.zero);
            });
        }

        /// <summary>
        /// Forces the playspace camera to origin facing forward along +Z.
        /// </summary>
        public static void PlayspaceToOriginLookingForward()
        {
            // Move the camera to origin looking at +z to more easily see the target at 0,0,+Z
            PlayspaceToPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// Sometimes it take a few frames for inputs raised via InputSystem.OnInput*
        /// to actually get sent to input handlers. This method waits for enough frames
        /// to pass so that any events raised actually have time to send to handlers.
        /// We set it fairly conservatively to ensure that after waiting
        /// all input events have been sent.
        /// </summary>
        public static IEnumerator WaitForInputSystemUpdate()
        {
            const int inputSystemUpdateFrames = 10;
            for (int i = 0; i < inputSystemUpdateFrames; i++)
            {
                yield return null;
            }
        }

        /// <summary>
        /// Force the playspace camera into the specified position and orientation.
        /// </summary>
        /// <param name="position">World space position for the playspace.</param>
        /// <param name="rotation">World space orientation for the playspace.</param>
        /// <remarks>
        /// <para>Note that this has no effect on the camera's local space transform, but
        /// will change the camera's world space position. If and only if the camera's
        /// local transform is identity with the camera's world transform equal the playspace's.</para>
        /// </remarks>
        public static void PlayspaceToPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            MixedRealityPlayspace.PerformTransformation(
            p =>
            {
                p.position = position;
                p.rotation = rotation;
            });
        }

        #endregion
    }
}