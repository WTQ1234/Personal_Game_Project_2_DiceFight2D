using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRL
{
    public class BuffBase_SubClassExample : BuffBase
    {
        public BuffBase_SubClassExample(BuffInfo _buff_info, BuffController _buff_controller) : base(_buff_info, _buff_controller)
        {
            Debug.Log("BuffBase_SubClassExample");
        }
    }
}
