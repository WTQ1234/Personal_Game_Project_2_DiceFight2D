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

        // �˴������� �������㹥�� �� ������������ ���������ܻ�������֮��ġ����������߼�����ability��
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

        // TODO: ��ʱ�����Զ���׼
        public void OnSetEntityTarget(Entity enemyTarget)
        {
            ability_Attack?.OnSetEntityTarget(enemyTarget);
            ability_shoot?.OnSetEntityTarget(enemyTarget);
        }

        #region �;�
        [Title("�;�")]
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
            //����ֵ���壺
            //С��0�����ڴ�������ǰ��
            //����0�����ֵ�ǰ��λ�ò���
            //����0�����ڴ������ĺ���

            // ��Ҫ�ӵ���Ӧ�����������������Ӳ�ϡ�еģ������;õ͵ģ�����ӽ�ս
            // Զ���ں��棬��ս��ǰ��
            if ((!weaponInfo.shoot) && other.weaponInfo.shoot)
            {
                return -1;
            }
            else if (weaponInfo.shoot && (!other.weaponInfo.shoot))
            {
                return 1;
            }
            // �����ߵ���ǰ��
            if (weaponInfo.quality > other.weaponInfo.quality) return -1;
            // TODO: �;�/��ϻ��ʾ
            //if (weaponInfo.quality > other.weaponInfo.quality) return -1;
            return 1;
        }

        [Title("��")]
        public SpriteRenderer hand_SpriteRenderer;
        public Sprite hand_empty;
        public Sprite hand_green;
        public Sprite hand_orange;
        public Sprite hand_red;
        public Sprite hand_white;
    }
}