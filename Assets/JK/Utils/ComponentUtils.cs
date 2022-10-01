using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class ComponentUtils
    {
        public static bool TryGetComponentInParent<T>(this Component self, out T component)
        {
            component = self.GetComponentInParent<T>();
            return component != null;
        }

        public static string GetHierarchyName(this Component self)
        {
            if (self == null)
                return UnityObjectUtils.NullName;

            string hierarchyName = self.name;

            Transform parent = self.transform.parent;

            while (parent != null)
            {
                hierarchyName = $"{parent.name} > {hierarchyName}";
                parent = parent.parent;
            }

            return $"({hierarchyName})";
        }

        public static string GetClassName(this Component self)
        {
            if (self == null)
                return UnityObjectUtils.NullName;
            else
                return self.GetType().Name;
        }

        public static string GetHierarchyDotClassName(this Component self)
        {
            if (self == null)
                return UnityObjectUtils.NullName;
            else
                return $"{self.GetHierarchyName()}.{self.GetClassName()}";
        }

        public static string GetGameObjectDotClassName(this Component self)
        {
            if (self == null)
                return UnityObjectUtils.NullName;
            else
                return $"{self.GetName()}.{self.GetClassName()}";
        }

        public static string Contextualize(this Component self, string message, bool includeHierarchy = false, bool includeClass = false)
        {
            string context;

            if (includeHierarchy)
            {
                if (includeClass)
                    context = self.GetHierarchyDotClassName();
                else
                    context = self.GetHierarchyName();
            }
            else
            {
                if (includeClass)
                    context = self.GetGameObjectDotClassName();
                else
                    context = self.GetName();
            }

            return $"[{context}] {message}";
        }
    }
}