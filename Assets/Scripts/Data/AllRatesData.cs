using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class AllRatesData
    {
        public string DATE;
        public string BASE;
        public Dictionary<string, float> RATES;
    }
}
