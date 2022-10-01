using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class RandomUtils
    {
        public static float Exponential(float k)
        {
            float uniformRand = UnityEngine.Random.Range(0.0f, 1.0f);
            return -Mathf.Log(uniformRand) * 1 / k;
        }

        /// <summary>
        /// Interpret the parameter as the average time between two events
        /// and the return value as a random time to wait with that average
        /// </summary>
        public static float ExponentialInversed(float k)
        {
            float uniformRand = UnityEngine.Random.Range(0.0f, 1.0f);
            return -Mathf.Log(uniformRand) * k;
        }

        public static float TimeUntilNextEvent(float averageTime)
        {
            return ExponentialInversed(averageTime);
        }

        public static float TimeUntilNextEvent(float averageTime, float maxDelta)
        {
            float randomDelta = UnityEngine.Random.Range(-maxDelta, maxDelta);
            return averageTime + randomDelta;
        }

        public static bool Should(float probability)
        {
            return UnityEngine.Random.Range(0f, 1f) <= probability;
        }
    }
}