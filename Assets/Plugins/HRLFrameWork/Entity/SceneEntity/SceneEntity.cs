using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRL
{
    public class SceneEntity : Entity
    {
        public bool takeDamage = false;

        public override EntityType getEntityType()
        {
            return EntityType.SceneEntity;
        }
    }
}