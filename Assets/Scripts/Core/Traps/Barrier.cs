using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Traps
{
    /// <summary>
    /// ’œ∞≠ŒÔ
    /// </summary>
    public class Barrier : TrapsItem
    {
        public int hp;
        protected override void Awake()
        {
            hp = 5;
            base.Awake();
        }
        public void Hurt()
        {
            hp--;
            if (hp == 0)
            {
                body.SetActive(false);
                Destroy(body);
            }
        }
    }
}