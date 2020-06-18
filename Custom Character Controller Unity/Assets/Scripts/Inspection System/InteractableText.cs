using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.CompanyName.GameName
{
    public class InteractableText : MonoBehaviour
    {
        private Inspector _inspector;

        private RaycastHit _hit;

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
            Transform rayTransform;
            Vector3 fwd = (rayTransform = transform).TransformDirection(Vector3.forward);

            bool iWantToRayCast = Physics.Raycast(rayTransform.position, fwd, out _hit, rayLength, layerMaskInteract.value);
            Debug.DrawRay(transform.position, fwd * rayLength, Color.red);

            if (iWantToRayCast)
            {
                if (_hit.collider.CompareTag("InteractObject"))
                {
                    _inspector = _hit.collider.gameObject.GetComponent<Inspector>();

                    _inspector.enabled = true;

                    _inspector.ShowItemName();

                    if (Input.GetMouseButtonDown(0))
                    {
                        _inspector.ShowExtraInfo();
                    }
                }
            }
            else
            {
                if (_inspector != null)
                {
                    _inspector.HideItemName();

                    _inspector.enabled = _inspector.iWantToWatch;
                }
            }
        }
    }

}

