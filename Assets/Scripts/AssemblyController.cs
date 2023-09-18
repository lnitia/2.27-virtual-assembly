using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Examples.Demos;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace SixAxisBogieAssembly
{
    public class AssemblyController : MonoBehaviour
    {
        public delegate void PartAssemblyControllerDelegate();

        [SerializeField] private GameObject replaceObject;
        private Transform locationToPlace = default;

        [SerializeField] private float MinDistance = 0.001f;
        [SerializeField] private float MaxDistance = 0.04f;

        public List<MeshCollider> otherColliders;

        private bool isPunEnabled;
        private bool shouldCheckPlacement;
        private bool checking;

        private AudioSource audioSource;
        private ToolTipSpawner toolTipSpawner;
        private List<Collider> colliders;
        private List<AssemblyController> AssemblyControllers;

        private Transform originalParent;
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private Vector3 originalScale;

        private IEnumerator checkPlacementCoroutine;

        private bool hasAudioSource;
        private bool hasToolTip;

        [HideInInspector] public bool isPlaced;
        private bool isResetting;

        public bool IsPunEnabled
        {
            set => isPunEnabled = value;
        }

        private void Start()
        {
            // Check if object should check for placement
            locationToPlace = replaceObject.transform;
            //replaceObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("XRAY");
            if (locationToPlace != transform) shouldCheckPlacement = true;

            // Cache references
            audioSource = GetComponent<AudioSource>();
            toolTipSpawner = GetComponent<ToolTipSpawner>();

            colliders = new List<Collider>();
            if (shouldCheckPlacement)
                foreach (var col in GetComponents<Collider>())
                    colliders.Add(col);

            AssemblyControllers = new List<AssemblyController>();
            foreach (var controller in FindObjectsOfType<AssemblyController>())
            {
                AssemblyControllers.Add(controller);
            }

            var trans = transform;
            originalParent = trans.parent;
            originalPosition = trans.localPosition;
            originalRotation = trans.localRotation;
            originalScale = trans.localScale;

            checkPlacementCoroutine = CheckPlacement();

            // Check if object has audio source
            hasAudioSource = audioSource != null;

            // Check if object has tool tip
            hasToolTip = toolTipSpawner != null;
            
            // Start coroutine to continuously check if the object has been placed
            if (shouldCheckPlacement) StartCoroutine(checkPlacementCoroutine);
            checking = true;
        }

        private void Update()
        {
            if (!checking)
            {
                if (shouldCheckPlacement) StartCoroutine(checkPlacementCoroutine);
                checking = true;
            }
            
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
            checking = true;

            // Play audio snapping sound
            if (hasAudioSource) audioSource.Play();

            // Disable ability to manipulate object
            foreach (var col in colliders) col.enabled = true;

            // Disable tool tips
            if (hasToolTip) toolTipSpawner.enabled = false;

            // Set parent and placement of object to target
            var trans = transform;
            trans.SetParent(locationToPlace.parent);
            trans.position = locationToPlace.position;
            trans.rotation = locationToPlace.rotation;
            trans.localScale = locationToPlace.localScale;
            Debug.Log("set");

            gameObject.GetComponent<ObjectManipulator>().enabled = false;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            
            foreach (var tri in otherColliders) tri.enabled = true;

            //replaceObject.SetActive(false);
        }

        /// <summary>
        ///     Triggers the reset placement feature.
        ///     Hooked up in Unity.
        /// </summary>
        public void ResetPlacement()
        {
            foreach (var controller in AssemblyControllers)
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
            checking = false;

            // Enable ability to manipulate object
            //foreach (var col in colliders) col.enabled = false;

            // Enable tool tips
            if (hasToolTip) toolTipSpawner.enabled = true;

            // Reset parent and placement of object
            var trans = transform;
            trans.SetParent(originalParent);
            trans.localPosition = originalPosition;
            trans.localRotation = originalRotation;
            trans.localScale = originalScale;

            foreach (var tri in otherColliders) tri.enabled = false;
            
            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            for (int i = 0; i < mr.materials.Length; i++)
            {
                mr.materials[i].color = Color.white;
            }
            gameObject.GetComponent<ObjectManipulator>().enabled = true;
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
                    trans.localScale = locationToPlace.localScale;
                }
                else
                {
                    break;
                }
            }
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
