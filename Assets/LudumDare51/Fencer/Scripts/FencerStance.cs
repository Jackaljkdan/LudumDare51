using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.Fencer
{
    [Serializable]
    public enum FencerStance
    {
        Thrust,
    }

    public static class FencerStanceExtensions
    {
        public static string AttackAnimationName(this FencerStance self)
        {
            return self.ToString();
        }

        public static string ParryAnimationName(this FencerStance self)
        {
            return "Parry" + self;
        }

        public static string HitAnimationName(this FencerStance self)
        {
            return "Hit" + self;
        }
    }
}