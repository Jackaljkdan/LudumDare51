using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class MonoBehaviourUtils
    {
        public static Coroutine RunAtEndOfFrame(this MonoBehaviour self, UnityAction action)
        {
            return self.StartCoroutine(RunAtEndOfFrameCoroutine(action));
        }

        public static IEnumerator RunAtEndOfFrameCoroutine(UnityAction action)
        {
            yield return new WaitForEndOfFrame();
            action();
        }
    }
}