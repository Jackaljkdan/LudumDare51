using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51
{
    [DisallowMultipleComponent]
    public class MobileControlsPanel : MonoBehaviour
    {
        #region Inspector



        #endregion

        private void Start()
        {
            if (!Application.isMobilePlatform)
                Destroy(this);
        }
    }
}