using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class Vector3Utils
    {
        public static Vector3 Create(float value)
        {
            return new Vector3(value, value, value);
        }
    }
}