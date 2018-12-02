using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BA
{

    public class BA_InputModule : StandaloneInputModule
    {
        PointerEventData ped;

        public PointerEventData GetLastPointerEventData()
        {
            //PointerEventData ped;

            //bool exists = GetPointerData(0,out ped,true);
            return GetLastPointerEventData(-1);
        }

        public bool GetPointerData()
        {            
            return GetPointerData(-1, out ped, false);
        }
    }
}
