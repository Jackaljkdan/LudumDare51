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
    public class HittingHealsTenSecondsEffect : TenSecondsEffect
    {
        #region Inspector

        public int healAmount = 1;

        [Injected]
        public FencerController player;

        [Injected]
        public FencerController ai;

        #endregion

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

            normalPlayerDamage = playerSword.damage;
            normalAiDamage = aiSword.damage;
        }

        public override void Apply(UnityAction doneCallback)
        {
            player.onHit.AddListener(OnPlayerHit);
            ai.onHit.AddListener(OnAiHit);

            var playerSword = player.GetComponentInChildren<Sword>();
            var aiSword = ai.GetComponentInChildren<Sword>();

            playerSword.damage = -healAmount;
            aiSword.damage = -healAmount;

            foreach (var cross in playerSword.GetComponentsInChildren<Cross>(includeInactive: true))
                cross.gameObject.SetActive(true);

            foreach (var cross in aiSword.GetComponentsInChildren<Cross>(includeInactive: true))
                cross.gameObject.SetActive(true);

            doneCallback?.Invoke();
        }

        private void OnPlayerHit()
        {
            HealFencer(ai);
        }

        private void OnAiHit()
        {
            HealFencer(player);
        }

        private void HealFencer(FencerController fencer)
        {
            if (fencer.attributes.healthPoints.Value < fencer.attributes.maxHealthPoints.Value)
                fencer.attributes.healthPoints.Value += healAmount;
        }

        public override bool CanApply()
        {
            return true;
        }

        public override string GetDescription()
        {
            return "Hitting Heals";
        }

        public override string GetInstructions()
        {
            return null;
        }

        public override void Revert()
        {
            player.onHit.RemoveListener(OnPlayerHit);
            ai.onHit.RemoveListener(OnAiHit);

            var playerSword = player.GetComponentInChildren<Sword>();
            var aiSword = ai.GetComponentInChildren<Sword>();

            playerSword.damage = normalPlayerDamage;
            aiSword.damage = normalAiDamage;

            foreach (var cross in playerSword.GetComponentsInChildren<Cross>())
                cross.gameObject.SetActive(false);

            foreach (var cross in aiSword.GetComponentsInChildren<Cross>())
                cross.gameObject.SetActive(false);
        }
    }
}