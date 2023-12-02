using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using System;
namespace HRL
{
    public class Weapon : Entity, IComparable<Weapon>
    {
        public Player Owner;

        public PickupInfo pickupInfo;
        public WeaponInfo weaponInfo;

        public PlayerAbility_Attack ability_Attack;

        public PlayerAbility_Gun ability_shoot;

        public int curIndex = 0;

        public int durable = 0;
        private static int default_durable = 10;
        public AudioSource hitSource;

        public override EntityType getEntityType()
        {
            return EntityType.Weapon;
        }

        protected override void Awake()
        {
            base.Awake();
            if (ability_shoot != null)
            {
                ability_shoot.weapon = this;
                ability_shoot.enabled = true;
            }
            if (ability_Attack != null)
            {
                ability_Attack.weapon = this;
                ability_Attack.enabled = true;
            }
            hitSource = GetComponent<AudioSource>();
        }

        public void SetWeaponInfo(WeaponInfo _weaponInfo)
        {
            weaponInfo = _weaponInfo;
            _RefreshWeaponInfo();
        }

        private void _RefreshWeaponInfo()
        {
            if (weaponInfo == null)
            {
                return;
            }
            if (weaponInfo.shoot && (ability_shoot != null))
            {
                ability_shoot.damage = weaponInfo.bullet_damage;
                ability_shoot.bullet_volicity = weaponInfo.bullet_vol;
                ability_shoot.prefab_bullet = weaponInfo.prefab_bullet;
            }
            if (ability_Attack != null)
            {
                ability_Attack.damage = weaponInfo.damage;
            }

            if (weaponInfo.durable > 0)
            {
                durable = weaponInfo.durable;
            }
            else
            {
                durable = default_durable;
            }
            _OnRefreshDurable();
        }

        // 此处制作了 基础单点攻击 和 长按持续攻击 ，后续可能会有蓄力之类的。长按持续逻辑放在ability中
        public void Attack()
        {
            if (hitSource != null)
            {
                hitSource.Play();
            }
            ability_shoot?.OnAbilityCall();
            ability_Attack?.OnAbilityCall();
        }

        public void Attack_Release()
        {
            hitSource.Play();
            ability_shoot?.OnAbilityCancel();
            ability_Attack?.OnAbilityCancel();
        }

        // TODO: 临时处理自动瞄准
        public void OnSetEntityTarget(Entity enemyTarget)
        {
            ability_Attack?.OnSetEntityTarget(enemyTarget);
            ability_shoot?.OnSetEntityTarget(enemyTarget);
        }

        #region 耐久
        [Title("耐久")]
        public SpriteRenderer handRender;
        public Color color_green;
        public Color color_red;
        public void OnSetDurable(int _durable)
        {
            durable = _durable;
            _OnRefreshDurable();
        }

        public void OnCostDurable(int cost)
        {
            durable -= cost;
            if (durable <= 0)
            {
                if (ability_shoot != null)
                {
                    ability_shoot.enabled = false;
                }
                if (ability_Attack != null)
                {
                    ability_Attack.enabled = false;
                }
                Owner.ExhaustWeapon(curIndex);
            }
            else
            {
                _OnRefreshDurable();
            }
        }

        private void _OnRefreshDurable()
        {
            handRender.color = Color.Lerp(color_red, color_green, (float)durable / (float)weaponInfo.durable);
        }
        #endregion

        #region Editor
        public string GetWeaponInfoName()
        {
            return weaponInfo?.Name;
        }
        #endregion

        public int CompareTo(Weapon other)
        {
            //返回值含义：
            //小于0：放在传入对象的前面
            //等于0：保持当前的位置不变
            //大于0：放在传入对象的后面

            // 需要扔掉对应数量的武器，优先扔不稀有的，再扔耐久低的，最后扔近战
            // 远程在后面，近战在前面
            if ((!weaponInfo.shoot) && other.weaponInfo.shoot)
            {
                return -1;
            }
            else if (weaponInfo.shoot && (!other.weaponInfo.shoot))
            {
                return 1;
            }
            // 质量高的在前面
            if (weaponInfo.quality > other.weaponInfo.quality) return -1;
            // TODO: 耐久/弹匣显示
            //if (weaponInfo.quality > other.weaponInfo.quality) return -1;
            return 1;
        }

        [Title("手")]
        public SpriteRenderer hand_SpriteRenderer;
        public Sprite hand_empty;
        public Sprite hand_green;
        public Sprite hand_orange;
        public Sprite hand_red;
        public Sprite hand_white;
    }
}