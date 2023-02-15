using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMoving : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (GetComponent<Entity>().isDead) 
            return;

        Reflect();
        ChekingGround();
        Walk();
        Jump();
        Attack();
        Lunge();
    }


    [SerializeField] bool faceRight = true;
    void Reflect()
    {
        if ((moveVector.x > 0 && !faceRight) || (moveVector.x < 0 && faceRight))
        {
            transform.localScale *= new Vector2(-1, 1);
            faceRight = !faceRight;
        }
    }


    [SerializeField] bool onGround;
    [SerializeField] float checkRadius = 0.5f;
    [SerializeField] Transform GroundCheck;
    [SerializeField] LayerMask Ground;
    void ChekingGround()
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, Ground);
        anim.SetBool("onGround", onGround);
    }

    [SerializeField] Vector2 moveVector;
    [SerializeField] float speed = 15f;
    void Walk()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        anim.SetFloat("moveX", MathF.Abs(moveVector.x));

        if (IsAnimationPlaying("Attack1") || IsAnimationPlaying("Attack2"))
        {
            rb.velocity = new Vector2(moveVector.x * 0.1f, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveVector.x * speed, rb.velocity.y);
        }
    }


    [SerializeField] float jumpForce = 1500f;
    [SerializeField] int maxJumpValue = 2;
    int jumpCount = 0;
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Physics2D.IgnoreLayerCollision(7, 8, true);
            Invoke("IgnoreLayerOff", 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround || (++jumpCount < maxJumpValue))
            {
                rb.velocity = Vector2.zero;

                anim.StopPlayback();
                anim.Play("Jump");

                rb.AddForce(Vector2.up * jumpForce);
            }
        }

        if (onGround) { jumpCount = 0; }
    }

    void IgnoreLayerOff()
    {
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }


    void Attack()
    {
        if (onGround)
        {
            if (Input.GetMouseButtonDown(0))
            {
                anim.Play("Attack1");
            }
            else if (Input.GetMouseButtonDown(1))
            {
                anim.Play("Attack2");
            }
        }
    }


    [SerializeField] int lungeImpulse = 25000;
    [SerializeField] float coolDownTime = 2f;
    [SerializeField] Image Bar;
    void Lunge()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!lockLunge)
            {
                lockLunge = true;
                Invoke("LungeLock", coolDownTime);

                anim.StopPlayback();
                anim.Play("Lunge");
                LungeRelativeToDirection();
            }
            else
            {
                Debug.Log("Рывок недоступен");
            }
        }

        RenderCoolDownLunge();
    }

    bool lockLunge = false;
    void LungeLock()
    {
        lockLunge = false;
    }

    void LungeRelativeToDirection()
    {
        Vector2 Direction;
        if (faceRight)
        {
            Direction = Vector2.right;
        }
        else
        {
            Direction = Vector2.left;
        }

        rb.velocity = Vector2.zero;
        rb.AddForce(Direction * lungeImpulse);
    }

    float nowTime = 0f;
    void RenderCoolDownLunge()
    {
        if (lockLunge)
        {
            GetComponentInChildren<Canvas>().enabled = true;
            nowTime -= Time.deltaTime;
            Bar.fillAmount = nowTime / coolDownTime;
        }
        else
        {
            GetComponentInChildren<Canvas>().enabled = false;
            nowTime = coolDownTime;
        }
    }


    bool IsAnimationPlaying(string animationName)
    {
        var animatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (animatorStateInfo.IsName(animationName))
            return true;

        return false;
    }
}