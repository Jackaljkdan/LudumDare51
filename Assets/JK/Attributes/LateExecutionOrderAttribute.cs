using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Attributes
{
    [Serializable]
    public class LateExecutionOrderAttribute : DefaultExecutionOrder
    {
        public LateExecutionOrderAttribute() : base(1) { }
    }
}