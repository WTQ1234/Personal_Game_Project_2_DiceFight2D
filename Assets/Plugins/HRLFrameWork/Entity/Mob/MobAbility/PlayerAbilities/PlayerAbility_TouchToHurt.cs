using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;

public class PlayerAbility_TouchToHurt : PlayerAbilityBase
{
    public Animator animator;

    public int damage;

    public bool destroyOnHit = true;

    public bool destroyOnHitGround = true;

    public ParticleSystem particleSystem;

    protected override void _SetName()
    {
        Name = "TouchToHurt";
    }

    protected override bool _Execute()
    {
        base._Execute();
        return true;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") && destroyOnHitGround)
        {
            ParticleSystem particle = Instantiate<ParticleSystem>(particleSystem);
            particle.Play();
            particle.transform.position = transform.position;
            Destroy(gameObject);
            return;
        }
        if (collision.tag == "SceneEntity")
        {
            SceneEntity sceneEntity = collision.GetComponent<SceneEntity>();
            // TODO: 其实SceneEntity也可以考虑统合到team里
            if (sceneEntity == null) return;
            sceneEntity.Damage(damage, true, transform);

            if (destroyOnHit)
            {
                ParticleSystem particle = Instantiate<ParticleSystem>(particleSystem);
                particle.Play();
                particle.transform.position = transform.position;
                Destroy(gameObject);
            }
            return;
        }
        if (collision.tag != "Player") return;
        Entity entity = collision.GetComponent<Entity>();
        if (entity == null) return;
        TeamController teamController = entity.teamController;
        int teamId = teamController.teamId;
        if (!Owner.teamController.DetectTeam(teamId))
        {
            entity.Damage(damage);
            if (destroyOnHit)
            {
                ParticleSystem particle = Instantiate<ParticleSystem>(particleSystem);
                particle.Play();
                particle.transform.position = transform.position;
                Destroy(gameObject);
            }
        }
    }
}
