using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class SetTargetFps : MonoBehaviour
    {
        #region Inspector

        public int targetFps = 60;

        #endregion

        private void Start()
        {
            Application.targetFrameRate = targetFps;
        }
    }
}