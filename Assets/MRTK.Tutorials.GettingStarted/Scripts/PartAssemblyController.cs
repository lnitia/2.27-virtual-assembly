using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Examples.Demos;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace MRTK.Tutorials.GettingStarted
{
    public class PartAssemblyController : MonoBehaviour
    {
        public delegate void PartAssemblyControllerDelegate();

        [SerializeField] private Transform locationToPlace = default;

        [SerializeField] private float MinDistance = 0.001f;
        [SerializeField] private float MaxDistance = 0.01f;

        private bool isPunEnabled;
        private bool shouldCheckPlacement;

        private AudioSource audioSource;
        private ToolTipSpawner toolTipSpawner;
        private List<Collider> colliders;
        private List<PartAssemblyController> partAssemblyControllers;

        private Transform originalParent;
        private Vector3 originalPosition;
        private Quaternion originalRotation;

        private IEnumerator checkPlacementCoroutine;

        private MeshOutline outLine;
        private int outlinedelay = 1500;
        private bool hasAudioSource;
        private bool hasToolTip;

        private bool isPlaced;
        private bool isResetting;

        public bool IsPunEnabled
        {
            set => isPunEnabled = value;
        }

        private void Start()
        {
            // Check if object should check for placement
            if (locationToPlace != transform) shouldCheckPlacement = true;

            // Cache references
            audioSource = GetComponent<AudioSource>();
            toolTipSpawner = GetComponent<ToolTipSpawner>();
            outLine = GetComponent<MeshOutline>();

            colliders = new List<Collider>();
            if (shouldCheckPlacement)
                foreach (var col in GetComponents<Collider>())
                    colliders.Add(col);

            partAssemblyControllers = new List<PartAssemblyController>();
            foreach (var controller in FindObjectsOfType<PartAssemblyController>()) {
                partAssemblyControllers.Add(controller);
            }

            var trans = transform;
            originalParent = trans.parent;
            originalPosition = trans.localPosition;
            originalRotation = trans.localRotation;

            checkPlacementCoroutine = CheckPlacement();

            // Check if object has audio source
            hasAudioSource = audioSource != null;

            // Check if object has tool tip
            hasToolTip = toolTipSpawner != null;

            // Start coroutine to continuously check if the object has been placed
            if (shouldCheckPlacement) StartCoroutine(checkPlacementCoroutine);
        }

        /// <summary>
        ///     Triggers the placement feature.
        /// </summary>
        private void SetPlacement()
        {
            if (isPunEnabled)
                OnSetPlacement?.Invoke();
            else
                Set();
        }

        /// <summary>
        ///     Parents the part to the assembly and places the part at the target location.
        /// </summary>
        public void Set()
        {
            // Update placement state
            isPlaced = true;

            // Play audio snapping sound
            if (hasAudioSource) audioSource.Play();

            // Disable ability to manipulate object
            foreach (var col in colliders) col.enabled = false;

            // Disable tool tips
            if (hasToolTip) toolTipSpawner.enabled = false;

            // Set parent and placement of object to target
            var trans = transform;
            trans.SetParent(locationToPlace.parent);
            trans.position = locationToPlace.position;
            trans.rotation = locationToPlace.rotation;
            //outLine.enabled = true;
            //outLine.OutlineMaterial.color = Color.green;//需检测是否有干涉，无干涉才变绿
            //CloseOutline();
            Debug.Log("set");
        }

        /// <summary>
        ///     Triggers the reset placement feature.
        ///     Hooked up in Unity.
        /// </summary>
        public void ResetPlacement()
        {
            foreach (var controller in partAssemblyControllers)
                if (isPunEnabled)
                    controller.OnResetPlacement?.Invoke();
                else
                    controller.Reset();
        }

        /// <summary>
        ///     Resets the part's parent and placement.
        /// </summary>
        public void Reset()
        {
            // Update placement state
            isPlaced = false;

            // Enable ability to manipulate object
            foreach (var col in colliders) col.enabled = true;

            // Enable tool tips
            if (hasToolTip) toolTipSpawner.enabled = true;

            // Reset parent and placement of object
            var trans = transform;
            trans.SetParent(originalParent);
            trans.localPosition = originalPosition;
            trans.localRotation = originalRotation;
            outLine.OutlineMaterial.color = Color.red;
        }

        /// <summary>
        ///     Checks the part's position and snaps/keeps it in place if the distance to target conditions are met.
        /// </summary>
        private IEnumerator CheckPlacement()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.01f);

                if (!isPlaced)
                {
                    if (Vector3.Distance(transform.position, locationToPlace.position) > MinDistance &&
                        Vector3.Distance(transform.position, locationToPlace.position) < MaxDistance)
                        SetPlacement();
                }
                else if (isPlaced)
                {
                    if (!(Vector3.Distance(transform.position, locationToPlace.position) > MinDistance)) continue;
                    var trans = transform;
                    trans.position = locationToPlace.position;
                    trans.rotation = locationToPlace.rotation;
                }
                else
                {
                    break;
                }
            }
        }
        private async void CloseOutline()
        {
            await Task.Delay(outlinedelay);
            outLine.enabled = false;
        }

        /// <summary>
        ///     Raised when RestPlacement is called and PUN is enabled.
        /// </summary>
        public event PartAssemblyControllerDelegate OnResetPlacement;

        /// <summary>
        ///     Raised when SetPlacement is called and PUN is enabled.
        /// </summary>
        public event PartAssemblyControllerDelegate OnSetPlacement;
    }
}
