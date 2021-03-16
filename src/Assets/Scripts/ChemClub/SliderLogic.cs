using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities.Editor;

namespace HoloTest_Namespace
{

    [RequireComponent(typeof(PinchSlider))]
    [AddComponentMenu("Fire particle system configuration")]
    public class SliderLogic : MonoBehaviour
    {
        [Header("Fire Handler")]
        [SerializeField]
        [Tooltip("Select the Particle System handler script")]
        private GameObject fire_particle_system = null;

        // temp for other 2 burners
        [Header("Fire Handler2")]
        [SerializeField]
        [Tooltip("Select the Particle System handler script")]
        private GameObject fire_particle_system_2 = null;
        [Header("Fire Handler3")]
        [SerializeField]
        [Tooltip("Select the Particle System handler script")]
        private GameObject fire_particle_system_3 = null;

        #region Private Members
        private PinchSlider slider;
        private FireHandler fire_handler;

        // temp 
        private FireHandler fire_handler_2;
        private FireHandler fire_handler_3;
        #endregion

        void Start()
        {
            slider = GetComponent<PinchSlider>();
            slider.OnInteractionStarted.AddListener(OnInteractionStarted);
            slider.OnInteractionEnded.AddListener(OnInteractionEnded);
            slider.OnValueUpdated.AddListener(OnValueUpdated);

            fire_handler = fire_particle_system.GetComponent<FireHandler>();

            // Testing on the other 2 burners
            fire_handler_2 = fire_particle_system_2.GetComponent<FireHandler>();
            fire_handler_3 = fire_particle_system_3.GetComponent<FireHandler>();
        }

        private void OnValueUpdated(SliderEventData eventData)
        {
            fire_handler.SetFlameHeightUsingSlider(slider.SliderValue);

            // Testing on the other 2 burners
            fire_handler_2.SetFlameHeightUsingSlider(slider.SliderValue);
            fire_handler_3.SetFlameHeightUsingSlider(slider.SliderValue);
        }

        private void OnInteractionEnded(SliderEventData arg0)
        {
        }

        private void OnInteractionStarted(SliderEventData arg0)
        {
        }
    }
}