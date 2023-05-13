using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LB3D.PuggosWorld.Unturned
{
    [CreateAssetMenu(fileName = "ModSet", menuName = "PuggosWorld/Unturned/ModSet", order = 3)]
    public class UnturnedModSetObject : ScriptableObject
    {
        public int idRangeMin;
        public int idRangeMax;
        public List<UnturnedModScriptableObject> unturnedMods = new List<UnturnedModScriptableObject>();
    }
}
