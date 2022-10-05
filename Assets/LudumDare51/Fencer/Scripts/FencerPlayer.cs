using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.Fencer
{
    [DisallowMultipleComponent]
    public class FencerPlayer : MonoBehaviour
    {
        #region Inspector

        public FencerController controller;

        public KeyCode attackKey = KeyCode.Mouse0;
        public KeyCode parryKey = KeyCode.Mouse1;
        public KeyCode focusKey = KeyCode.Space;

        private void Reset()
        {
            controller = GetComponent<FencerController>();
        }

        #endregion

        private bool wasFocusing;

        private void Start()
        {
            if (PlatformUtils.IsMobileBuild)
                Destroy(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(attackKey))
            {
                controller.Attack();
            }
            else if (Input.GetKeyDown(parryKey))
            {
                controller.Parry();
            }
            else if (Input.GetKey(focusKey))
            {
                wasFocusing = true;
                controller.EnterFocus();
            }
            else if (wasFocusing)
            {
                wasFocusing = false;
                controller.ExitFocus();
            }
        }
    }
}