using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRL
{
    public class PlayerAbilityController : MonoBehaviour
    {
        public Entity currentTarget;

        public List<PlayerAbilityBase> abilities;

        void Start()
        {
        }

        void Update()
        {

        }

        public void OnSetEntityTarget(Entity target)
        {
            currentTarget = target;
            _OnSetTargetEntity();
        }

        private void _OnSetTargetEntity()
        {
            foreach(PlayerAbilityBase ability in abilities)
            {
                ability.OnSetEntityTarget(currentTarget);
            }
        }
    }
}
