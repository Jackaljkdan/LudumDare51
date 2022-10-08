using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LudumDare51.UI
{
    public static class Tokens
    {
        public static string Format(string s)
        {
            return s.Replace(
                "{Focus}",
                PlatformUtils.IsDesktopBuild
                    ? "SPACEBAR"
                    : "FOCUS"
            );
        }
    }
}