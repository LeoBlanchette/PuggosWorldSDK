using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LB3D.PuggosWorld.Unturned
{
    public class IDGenerator
    {
        public List<int> usedIds = new List<int>();
        public UnturnedModSetObject unturnedModSet;
        public int rangeMin = 0;
        public int rangeMax = 0;

        public IDGenerator(UnturnedModSetObject unturnedModSet)
        {
            this.unturnedModSet = unturnedModSet;
            this.rangeMin = unturnedModSet.idRangeMin;
            this.rangeMax = unturnedModSet.idRangeMax;

            foreach (UnturnedModScriptableObject unturnedModScriptableObject in unturnedModSet.unturnedMods)
            {

                foreach (UnturnedDatFileScriptableObject datFile in unturnedModScriptableObject.datFiles)
                {
                    LogId(datFile);
                }
            }
        }

        public void LogId(int id) {
            if (!usedIds.Contains(id))
            {
                usedIds.Add(id);
            }
            else
            {
                Debug.LogError(id + " already exists.");
            }
        }

        public void LogId(UnturnedDatFileScriptableObject datFile)
        {
            if (!usedIds.Contains(datFile.id))
            {
                usedIds.Add(datFile.id);
            }
            else
            {
                Debug.LogError(datFile.id + " already exists. Duplicate found in " + datFile.nameEnglish + ", " + datFile.name);
            }
        }

        public int GetNewId()
        {
            for (int i = rangeMin; i < rangeMax; i++)
            {
                if (!usedIds.Contains(i)) {
                    LogId(i);
                    return i;
                }
            }

            Debug.LogWarning("IDs have surpassed max range of " + rangeMax + ". Id not assigned");

            return 0;
        }
    }
}

