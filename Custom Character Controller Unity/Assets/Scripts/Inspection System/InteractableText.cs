using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.CompanyName.GameName
{
    public class InteractableText : MonoBehaviour
    {
        private Inspector inspector;

        RaycastHit hit;

        [Space]
        [Header("Raycast Data")]
        [SerializeField] private int rayLength = 5;
        [SerializeField] private LayerMask layerMaskInteract;

        private void Update()
        {
            Raycast();
        }

        private void Raycast()
        {     
            Vector3 fwd = transform.TransformDirection(Vector3.forward);

            bool iWantToRayCast = Physics.Raycast(transform.position, fwd, out hit, rayLength, layerMaskInteract.value);
            Debug.DrawRay(transform.position, fwd * rayLength, Color.red);

            if (iWantToRayCast)
            {
                if (hit.collider.CompareTag("InteractObject"))
                {
                    inspector = hit.collider.gameObject.GetComponent<Inspector>();

                    inspector.enabled = true;

                    inspector.ShowItemName();

                    if (Input.GetMouseButtonDown(0))
                    {
                        inspector.ShowExtraInfo();
                    }
                }
            }
            else
            {
                if (inspector != null)
                {
                    inspector.HideItemName();

                    if (inspector.iWantToWatch)
                    {
                        inspector.enabled = true;

                    }
                    else
                    {
                        inspector.enabled = false;
                    }
                }
            }
        }
    }

}

