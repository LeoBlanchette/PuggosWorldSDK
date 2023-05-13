using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LB3D.PuggosWorld.Unturned
{
    public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    T[] assets = Resources.LoadAll<T>("");
                    if (assets == null || assets.Length < 1)
                    {
                        throw new System.Exception("Could not find any singleton scriptable object instances in the resources.");
                    }
                    else if (assets.Length > 1) {
                        Debug.LogWarning("Multiple instances of singleton scriptable objects found in resources...");
                    }
                    instance = assets[0];
                }
                return instance;
            }
        }
    }
}