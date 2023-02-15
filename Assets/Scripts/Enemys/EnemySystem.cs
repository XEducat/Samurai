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
     
    [SerializeField] Transform point;
    Transform player;
    Animator anim;

    bool movingRight = false;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (GetComponent<Entity>().isDead) return;

        Reflect();
        CheckEnvironment();
    }

    void Reflect()
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

    enum EnemyState
    {
        None,
        Angry,
        Chill,
        GoBack
    }

    void CheckEnvironment()
    {
        EnemyState state = EnemyState.None;

        if (Vector2.Distance(transform.position, player.position) < stopingDistance)
        {
            state = EnemyState.Angry;
        }
        else if (Vector2.Distance(transform.position, point.position) < positionOfPatrool)
        {
            state = EnemyState.Chill;
        }
        else if (Vector2.Distance(transform.position, player.position) > stopingDistance)
        {
            state = EnemyState.GoBack;
        }

        CheckDirection(state);
        PlayState(state);
    }

    private void PlayState(EnemyState State)
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

    void CheckDirection(EnemyState State)
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

    void Angry()
    {
        if (!inAttack)
        {
            if (transform.position.x == player.position.x)
            {
                anim.Play("Idle");
            }
            else
            {
                // targetVector - создан что бы заморозить передвижение врага по оси Y
                Vector3 targetVector = new Vector3(player.position.x, transform.position.y, player.position.z);
                transform.position = Vector2.MoveTowards(transform.position, targetVector, moveSpeed * Time.deltaTime);
                anim.Play("Walk"); // TODO - Нужно сделать через условие для бега ( Для коректной отработки TakeHit )
            }
        }
        TryAttack();
    }

    [SerializeField] float attackCoolDown = 1f;
    bool inAttack = false;
    void TryAttack()
    {
        if (!isLockAttack)
        {
            isLockAttack = true;
            Invoke("UnlockAttack", attackCoolDown);

            DamageSystem attack = GetComponent<DamageSystem>();
            if (attack.CheckAttackRadius())
            {
                inAttack = true;
                anim.StopPlayback();
                anim.Play("Attack");
            }
            else
            {
                inAttack = false;
            }
        }
    }

    bool isLockAttack = false;
    void UnlockAttack()
    {
        isLockAttack = false;
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
