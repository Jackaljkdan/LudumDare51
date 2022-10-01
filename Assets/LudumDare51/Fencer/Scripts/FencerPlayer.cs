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

        private void Reset()
        {
            controller = GetComponent<FencerController>();
        }

        #endregion

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                controller.Attack();
            else if (Input.GetMouseButtonDown(1))
                controller.Parry();
        }
    }
}