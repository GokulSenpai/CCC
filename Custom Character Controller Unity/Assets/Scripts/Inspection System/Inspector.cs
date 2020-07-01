using System.Collections;
using System.Collections.Generic;
using Inspection_System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

namespace Com.CompanyName.GameName
{
    public class Inspector : MonoBehaviour
    {
        [SerializeField] public Text ObjectNameUI;

        [SerializeField] public string itemName;

        [TextArea] [SerializeField] public string itemExtraInfo;

        [HideInInspector] public bool iWantToWatch = false;

        private Vector3 _initialPosition;
        private Quaternion _initialRotation;

        private readonly Vector3 _inspectPosition = new Vector3(-1.77f, 6.21f, -28.37f);
        private readonly Quaternion _inspectRotation = Quaternion.Euler(0f, 0f, 0f);

        private Vector3 _posLastFrame;

        private InspectorManager _manager;

        private void Start()
        {
            ObjectNameUI.color = Color.clear;

            // toggles this script off
            this.gameObject.GetComponent<Inspector>().enabled = false;

            _manager = GameObject.FindObjectOfType<InspectorManager>();

            var objectTransform = transform;
            _initialPosition = objectTransform.localPosition;
            _initialRotation = objectTransform.localRotation;
        }

        private void Update()
        {
            Inspection();
        }

        private void LateUpdate()
        {
            Rotate();
        }

        private void Inspection()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                iWantToWatch = !iWantToWatch;

                if (iWantToWatch)
                {
                    iWantToWatch = true;

                    _manager.Watch(itemName, itemExtraInfo);

                    FindObjectOfType<Player>().GetComponent<CharacterController>().enabled = false;

                    var objectTransform = transform;
                    objectTransform.localPosition = _inspectPosition;
                    objectTransform.localRotation = _inspectRotation;
                }
                else
                {
                    iWantToWatch = false;
                    
                    FindObjectOfType<Player>().GetComponent<CharacterController>().enabled = true;

                    _manager.DontWatch();
                    

                    var objectTransform = transform;
                    objectTransform.localPosition = _initialPosition;
                    objectTransform.localRotation = _initialRotation;
                }
            }
        }

        private void Rotate()
        {
            if (iWantToWatch)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                if (Input.GetMouseButtonDown(0))
                    _posLastFrame = Input.mousePosition;

                if (Input.GetMouseButton(0))
                {
                    var delta = Input.mousePosition - _posLastFrame;
                    _posLastFrame = Input.mousePosition;

                    var axis = Quaternion.AngleAxis(-90f, Vector3.forward) * delta;
                    transform.rotation = Quaternion.AngleAxis(delta.magnitude * 0.1f, axis) * transform.rotation;
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public void ShowItemName()
        {
            _manager.ShowName(itemName, ObjectNameUI);
        }

        public void HideItemName()
        {
            _manager.HideName(ObjectNameUI);
        }

        public void ShowExtraInfo()
        {
            _manager.ShowAdditionalInfo(itemExtraInfo);
        }
    }
}

