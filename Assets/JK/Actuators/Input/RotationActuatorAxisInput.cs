using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JK.Actuators.Input
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(IRotationActuator))]
    public class RotationActuatorAxisInput : RotationActuatorInputBehaviour
    {
        #region Inspector fields

        #endregion

        private void Awake()
        {
            if (!PlatformUtils.IsDesktop)
                Destroy(this);
        }

        private void LateUpdate()
        {
            float leftRightRotation = UnityEngine.Input.GetAxis("Mouse X");
            float upDownRotation = UnityEngine.Input.GetAxis("Mouse Y");

            GetComponent<IRotationActuator>().Input = new Vector2(leftRightRotation, upDownRotation);
        }
    }
}