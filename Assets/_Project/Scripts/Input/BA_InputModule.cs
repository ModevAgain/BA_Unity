using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InputSystem
{

    public class BA_InputModule : StandaloneInputModule
    {
        PointerEventData ped;

        

        public PointerEventData GetLastPointerEventDataCustom(int id = -1)
        {
                return GetLastPointerEventData(id);
        }

        public bool GetPointerData()
        {            
            return GetPointerData(-1, out ped, false);
        }
    }
}
