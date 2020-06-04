using System.Collections;
using System.Collections.Generic;
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

        Vector3 initialPosition;
        Quaternion initialRotation;

        Vector3 inspectPosition = new Vector3(-1.77f, 6.21f, -28.37f);
        Quaternion inspectRotation = Quaternion.Euler(0f, 0f, 0f);

        Vector3 posLastFrame;

        private InspectorManager manager;

        private void Start()
        {
            ObjectNameUI.color = Color.clear;

            // toggles this script off
            this.gameObject.GetComponent<Inspector>().enabled = false;

            manager = GameObject.FindObjectOfType<InspectorManager>();

            initialPosition = transform.localPosition;
            initialRotation = transform.localRotation;
        }

        private void Update()
        {
            Inspection();
        }

        private void LateUpdate()
        {
            InspectingRotator();
        }

        private void Inspection()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {             

                iWantToWatch = !iWantToWatch;

                if (iWantToWatch)
                {
                    iWantToWatch = true;

                    manager.Watch(itemName, itemExtraInfo);

                    transform.localPosition = inspectPosition;
                    transform.localRotation = inspectRotation;
                }
                else
                {
                    iWantToWatch = false;

                    manager.DontWatch();

                    transform.localPosition = initialPosition;
                    transform.localRotation = initialRotation;
                }
            }
        }

        private void InspectingRotator()
        {
            if (iWantToWatch)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = false;

                if (Input.GetMouseButtonDown(0))
                    posLastFrame = Input.mousePosition;

                if (Input.GetMouseButton(0))
                {
                    var delta = Input.mousePosition - posLastFrame;
                    posLastFrame = Input.mousePosition;

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
            manager.ShowName(itemName, ObjectNameUI);
        }

        public void HideItemName()
        {
            manager.HideName(ObjectNameUI);
        }

        public void ShowExtraInfo()
        {
            manager.ShowAdditionalInfo(itemExtraInfo);
        }
    }
}

