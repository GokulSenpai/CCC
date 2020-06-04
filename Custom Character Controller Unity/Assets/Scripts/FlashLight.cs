using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.CompanyName.GameName
{
    public class FlashLight : MonoBehaviour
    {
        public GameObject flashLightObject;
        private bool flashTheLight = false;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                flashTheLight = !flashTheLight;
                flashLightObject.SetActive(flashTheLight);
            }
        }
    }

}

