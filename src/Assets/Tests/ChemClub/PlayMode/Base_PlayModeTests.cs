using System.Collections;
using UnityEngine.TestTools;

namespace HoloTest_Namespace
{
    public abstract class BasePlayModeTests
    {
        [UnitySetUp]
        public virtual IEnumerator Setup()
        {
            PlayModeTestManager.Setup();
            yield return null;
        }

        [UnityTearDown]
        public virtual IEnumerator TearDown()
        {
            PlayModeTestManager.TearDown();
            yield return null;
        }
    }
}
