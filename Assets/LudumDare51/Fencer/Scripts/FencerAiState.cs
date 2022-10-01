using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.Fencer
{
    [Serializable]
    public enum FencerAiState
    {
        Idle,
        IdleNoParry,
        Attacking,
        ParryWaiting,
        Parrying,
    }
}