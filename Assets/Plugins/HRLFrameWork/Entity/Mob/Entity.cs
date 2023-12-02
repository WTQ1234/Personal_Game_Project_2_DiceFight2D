using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HRL
{
    public enum EntityType
    {
        Default = 0,
        Player = 1,
        PlayerSummon = 2,
        NPC = 3,
        Enemy = 4,
        SceneEntity = 5,
        Weapon = 6,
        Bullet = 7,
    }

    // Entity 指可活动物体，如玩家，NPC，敌人等
    public class Entity : MonoBehaviour
    {
        public TeamController teamController
        {
            get
            {
                if (_teamController == null)
                {
                    _teamController = gameObject.GetComponent<TeamController>();
                }
                if (_teamController == null)
                {
                    _teamController = gameObject.AddComponent<TeamController>();
                }
                return _teamController;
            }
        }
        [ShowInInspector]
        protected TeamController _teamController;

        public virtual EntityType getEntityType()
        {
            return EntityType.Default;
        }

        public virtual bool isPlayer()
        {
            return getEntityType() == EntityType.Player;
        }

        public virtual void Damage(int damage = 1, bool knock_back = false, Transform trans_damage_from = null)
        {

        }

        protected virtual void Awake() { }

        protected virtual void Update() { }
    }
}