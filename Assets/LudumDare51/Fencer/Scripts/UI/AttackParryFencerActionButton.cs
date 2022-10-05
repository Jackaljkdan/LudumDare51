using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LudumDare51.Fencer.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class AttackParryFencerActionButton : FencerActionButton
    {
        #region Inspector

        public bool isAttackButton = true;

        #endregion

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClicked);
        }

        private void OnClicked()
        {
            if (!enabled)
                return;

            if (isAttackButton)
                controller.Attack();
            else
                controller.Parry();
        }
    }
}