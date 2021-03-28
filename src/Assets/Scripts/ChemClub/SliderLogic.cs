using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities.Editor;

namespace HoloTest
{

    [RequireComponent(typeof(PinchSlider))]
    [AddComponentMenu("Fire particle system configuration")]
    public class SliderLogic : MonoBehaviour
    {
        [Header("Bunsen Burner GameObject")]
        [SerializeField]
        private GameObject bunsenBurner = null;

        #region Private Members
        private PinchSlider slider;
        private FireHandler fireHandlerScript;
        #endregion

        void Start()
        {
            slider = GetComponent<PinchSlider>();
            slider.OnInteractionStarted.AddListener(OnInteractionStarted);
            slider.OnInteractionEnded.AddListener(OnInteractionEnded);
            slider.OnValueUpdated.AddListener(OnValueUpdated);

            fireHandlerScript = bunsenBurner.GetComponent<FireHandler>();
        }

        public float GetSliderValue()
        {
            return slider.SliderValue;
        }

        public void SetSliderValue(float sliderValue)
        {
            slider.SliderValue = sliderValue;
            fireHandlerScript.SetFlameHeightUsingSlider(slider.SliderValue);
        }

        public void OnAnimationChange()
        {
            fireHandlerScript.SetFlameHeightUsingSlider(slider.SliderValue);
        }

        private void OnValueUpdated(SliderEventData eventData)
        {
            fireHandlerScript.SetFlameHeightUsingSlider(slider.SliderValue);
        }

        private void OnInteractionEnded(SliderEventData arg0)
        {
        }

        private void OnInteractionStarted(SliderEventData arg0)
        {
        }
    }
}