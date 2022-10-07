using JK.Injection;
using JK.Injection.PropertyDrawers;
using LudumDare51.Fencer;
using LudumDare51.Weapon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.TenSeconds
{
    [DisallowMultipleComponent]
    public class OneShotSwordTenSecondsEffect : TenSecondsEffect
    {
        #region Inspector

        public Material oneShotMaterial;

        public int oneShotDamage = 100;

        [Injected]
        public FencerController player;

        [Injected]
        public FencerController ai;

        #endregion

        private Material normalPlayerMaterial;
        private Material normalAiMaterial;

        private int normalPlayerDamage;
        private int normalAiDamage;

        private void Awake()
        {
            Context context = Context.Find(this);

            player = context.Get<FencerController>(this, "player");
            ai = context.Get<FencerController>(this, "ai");
        }

        private void Start()
        {
            var playerSword = player.GetComponentInChildren<Sword>();
            var aiSword = ai.GetComponentInChildren<Sword>();

            normalPlayerMaterial = playerSword.GetComponent<MeshRenderer>().material;
            normalAiMaterial = aiSword.GetComponent<MeshRenderer>().material;

            normalPlayerDamage = playerSword.damage;
            normalAiDamage = aiSword.damage;
        }

        public override void Apply(UnityAction doneCallback)
        {
            var playerSword = player.GetComponentInChildren<Sword>();
            var aiSword = ai.GetComponentInChildren<Sword>();

            playerSword.GetComponent<MeshRenderer>().material = oneShotMaterial;
            aiSword.GetComponent<MeshRenderer>().material = oneShotMaterial;

            playerSword.GetComponentInChildren<OneShotParticleSystem>(includeInactive: true).Play();
            aiSword.GetComponentInChildren<OneShotParticleSystem>(includeInactive: true).Play();

            playerSword.damage = oneShotDamage;
            aiSword.damage = oneShotDamage;

            doneCallback?.Invoke();
        }

        public override bool CanApply()
        {
            return true;
        }

        public override string GetDescription()
        {
            return "One Shot Sword";
        }

        public override string GetInstructions()
        {
            return null;
        }

        public override void Revert()
        {
            var playerSword = player.GetComponentInChildren<Sword>();
            var aiSword = ai.GetComponentInChildren<Sword>();

            playerSword.GetComponent<MeshRenderer>().material = normalPlayerMaterial;
            aiSword.GetComponent<MeshRenderer>().material = normalAiMaterial;

            playerSword.GetComponentInChildren<OneShotParticleSystem>(includeInactive: true).Stop();
            aiSword.GetComponentInChildren<OneShotParticleSystem>(includeInactive: true).Stop();

            playerSword.damage = normalPlayerDamage;
            aiSword.damage = normalAiDamage;
        }
    }
}