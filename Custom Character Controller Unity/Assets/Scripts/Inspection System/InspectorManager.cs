using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

namespace Com.CompanyName.GameName
{
    public class InspectorManager : MonoBehaviour
    {
        [Space]
        [Header("Canvas References")]
        [SerializeField] private Text ExamineObjectName;
        [SerializeField] private Text extraInfoUI;
        [SerializeField] private GameObject extraInfoBG;

        [Space]
        [Header("Other References")]
        [SerializeField] private GameObject thePlayer;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private GameObject postProcessObject;


        [Space]
        [Header("Timers")]
        [SerializeField] private float onScreenTimer;
        [SerializeField] private float fadeTime;

        private bool startTimer;

        private float timer;

        DepthOfField dof = null;
        TextureOverlay overlayTexture = null;

        private void Start()
        {
            PostProcessVolume volume = postProcessObject.GetComponent<PostProcessVolume>();

            volume.profile.TryGetSettings(out dof);
            volume.profile.TryGetSettings(out overlayTexture);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            extraInfoBG.SetActive(false);
        }

        private void Update()
        {
            Timer();
        }

        public void Watch(string itemName, string itemExtraInfo)
        {
            // set player.getcomponent look script to false
            thePlayer.GetComponent<Look>().enabled = false;

            ExamineObjectName.text = itemName;

            extraInfoBG.SetActive(true);

            startTimer = false;

            extraInfoUI.text = itemExtraInfo;

            // enable texture overlay
            overlayTexture.enabled.value = true;

            // enable dof
            dof.enabled.value = true;

        }

        public void DontWatch()
        {
            startTimer = true;

            thePlayer.GetComponent<Look>().enabled = true;

            // enable texture overlay
            overlayTexture.enabled.value = false;

            // enable dof
            dof.enabled.value = false;
        }

        public void ShowName(string itemName, Text ObjectNameUI)
        {
            ObjectNameUI.text = itemName;
            ObjectNameUI.color = Color.Lerp(ObjectNameUI.color, Color.white, fadeTime * Time.smoothDeltaTime);
            ObjectNameUI.transform.LookAt(cameraTransform.position);
            ObjectNameUI.transform.rotation = Quaternion.LookRotation(ObjectNameUI.transform.position - cameraTransform.position);
        }

        public void HideName(Text ObjectNameUI)
        {
            ObjectNameUI.color = Color.Lerp(ObjectNameUI.color, Color.clear, fadeTime * Time.smoothDeltaTime);
        }

        public void ShowAdditionalInfo(string itemExtraInfo)
        {
            timer = onScreenTimer;
            startTimer = true;
            extraInfoBG.SetActive(true);
            extraInfoUI.text = itemExtraInfo;
        }

        void ClearAdditionalInfo()
        {
            extraInfoBG.SetActive(false);
            extraInfoUI.text = "";
        }

        private void Timer()
        {
            if (startTimer)
            {
                timer -= Time.smoothDeltaTime;

                if (timer <= 0)
                {
                    timer = 0;
                    ClearAdditionalInfo();
                    startTimer = false;
                }
            }
        }
    }
}

 
