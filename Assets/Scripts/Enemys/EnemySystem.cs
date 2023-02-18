using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

//[RequireComponent(typeof(Collider2D))]

public class EnemySystem : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] int positionOfPatrool;
    [SerializeField] float stopingDistance;
     
    private bool movingRight = false;
    private EnemyState State = EnemyState.None;

    [SerializeField] Transform point;
    private Transform player;
    private Animator anim;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        if (GetComponent<Entity>().isDead) return;

        Reflect();
        DefineState();
        CheckDirection();
        PlayState();
    }

    private void Reflect()
    {
        float ScaleX = transform.localScale.x;

        if (movingRight && ScaleX < 0)
        {
            ScaleX *= -1;
        }
        else if (!movingRight && ScaleX > 0)
        {
            ScaleX *= -1;
        }

        transform.localScale = new Vector2(ScaleX, transform.localScale.y);
    }


    private enum EnemyState
    {
        None,
        Angry,
        Chill,
        GoBack
    }

    void DefineState()
    {
        if (Vector2.Distance(transform.position, player.position) < stopingDistance)
        {
            State = EnemyState.Angry;
        }
        else if (Vector2.Distance(transform.position, point.position) < positionOfPatrool)
        {
            State = EnemyState.Chill;
        }
        else if (Vector2.Distance(transform.position, player.position) > stopingDistance)
        {
            State = EnemyState.GoBack;
        }
    }


    void CheckDirection()
    {
        switch(State)
        {
            case EnemyState.Angry:
                if (transform.position.x < player.position.x)
                {
                    movingRight = true;
                }
                else if (transform.position.x > player.position.x)
                {
                    movingRight = false;
                }
                break;

            case EnemyState.Chill:
                if (transform.position.x < point.position.x - positionOfPatrool)
                {
                    movingRight = true;
                }
                else if (transform.position.x > point.position.x + positionOfPatrool)
                {
                    movingRight = false;
                }
                break;

            case EnemyState.GoBack:
                if (transform.position.x < point.position.x)
                {
                    movingRight = true;
                }
                else if (transform.position.x > point.position.x)
                {
                    movingRight = false;
                }
                break;
        }
    }

    private void PlayState()
    {
        switch(State)
        {
            case EnemyState.Angry:
                Angry();
                break;
            case EnemyState.Chill:
                Chill();
                break;
            case EnemyState.GoBack:
                GoBack();
                break;
        }
    }

    [SerializeField] float attackCoolDown = 1f;
    void Angry()
    {
        DamageSystem attack = GetComponent<DamageSystem>();
        if (attack.CheckAttackRadius() && !AttackLocked)
        {
            AttackLocked = true;
            Invoke("UnlockAttack", attackCoolDown);

            anim.Play("Attack");
        }
        else if (attack.CheckAttackRadius() == false)
        {
            Vector3 targetVector = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.position = Vector2.MoveTowards(transform.position, targetVector, moveSpeed * Time.deltaTime);
            anim.Play("Walk"); // TODO - Нужно сделать исправить аниматор
        }
        else if (AttackLocked == false)
        {
            anim.Play("Idle");
        }
    }

    bool AttackLocked = false;
    void UnlockAttack()
    {
        AttackLocked = false;
    }


    void Chill()
    {
        float moveSpeedInDirection = movingRight ? moveSpeed : moveSpeed * -1;

        transform.position = new Vector2(transform.position.x + moveSpeedInDirection * Time.deltaTime, transform.position.y);
    }

    void GoBack()
    {
        transform.position = Vector2.MoveTowards(transform.position, point.position, moveSpeed * Time.deltaTime);
    }
}
