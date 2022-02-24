using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HandPoseGen.Models.Avatar
{
    public class ProxyHandModel : HandPoseGenElement
    {
        [Header("Models")]
        public HandModel master;
        public HandModel ghost;

        private void Awake()
        {
            master.proxyHand = this;
            ghost.proxyHand = this;
        }
    }
}
