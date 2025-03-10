using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float shotTimer;

    public override void Enter()
    {
        Debug.Log("Masuk ke AttackState");
    }

    public override void Exit()
    {
        Debug.Log("Keluar dari AttackState");
    }

    public override void Perform()
    {
        if (enemy == null || enemy.Player == null)
        {
            Debug.LogWarning("Enemy atau Player tidak ditemukan dalam AttackState!");
            return;
        }

        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;
            enemy.transform.LookAt(enemy.Player.transform);

            if (shotTimer > enemy.fireRate)
            {
                Shoot();
            }

            if (moveTimer > Random.Range(3, 7))
            {
                if (enemy.Agent != null && enemy.Agent.isActiveAndEnabled) 
                {
                    enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                    moveTimer = 0;
                }
                else
                {
                    Debug.LogWarning("NavMeshAgent tidak ditemukan atau tidak aktif di Enemy!");
                }
            }

            enemy.LastKnowPos = enemy.Player.transform.position;
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 8)
            {
                Debug.Log("Pindah ke SearchState karena kehilangan jejak Player.");
                stateMachine.ChangeState(new SearchState());
            }
        }
    }

    public void Shoot()
    {
        if (enemy == null || enemy.gunBarrel == null)
        {
            Debug.LogWarning("GunBarrel tidak ada atau belum diassign di Inspector!");
            return;
        }

        Debug.Log("Shoot!");
        shotTimer = 0;
        Transform gunbarrel = enemy.gunBarrel;

        GameObject bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
        if (bulletPrefab == null)
        {
            Debug.LogError("Prefab Bullet tidak ditemukan! Periksa jalur di Resources.");
            return;
        }

        GameObject bullet = GameObject.Instantiate(bulletPrefab, gunbarrel.position, gunbarrel.rotation);
        if (bullet.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Vector3 shootDirection = (enemy.Player.transform.position - gunbarrel.position).normalized;
            rb.velocity = shootDirection * 40;
        }
        else
        {
            Debug.LogError("Prefab Bullet tidak memiliki Rigidbody!");
        }
    }
}
