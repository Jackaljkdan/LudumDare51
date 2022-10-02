using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.Weapon
{
    [DisallowMultipleComponent]
    public class Cross : MonoBehaviour
    {
        #region Inspector



        #endregion

        private void Start()
        {
            gameObject.SetActive(false);
        }
    }
}