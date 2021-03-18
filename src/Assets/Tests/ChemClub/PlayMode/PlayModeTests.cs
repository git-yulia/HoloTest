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
using Microsoft.MixedReality.Toolkit.Tests; 
using UnityEditor.Build;

namespace HoloTest_Namespace
{
    public class PlayModeTests
    {
        const string testSceneName = "ChemClub";
        const string testScenePath = "Assets/Scenes/ChemClub.unity";

        public IEnumerator LoadTestScene()
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(testSceneName);
            loadOp.allowSceneActivation = true;
            while (!loadOp.isDone)
            {
                yield return null;
            }
            yield return true; 
        }

        [UnityTest]
        public IEnumerator CheckSliderExists()
        {
            yield return LoadTestScene();
            GameObject slider = GameObject.Find("PinchSlider");
            Assert.IsNotNull(slider);
        }

        /*[UnityTest]
        public IEnumerator TestPressableButtonInteraction()
        {
            yield return LoadTestScene();
            Assert.IsNotNull(3);
        }

        [UnityTest]
        public IEnumerator TestSliderInteraction()
        {
            yield return LoadTestScene();
            Assert.IsNotNull(3);
        }*/
    }
}
