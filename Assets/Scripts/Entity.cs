using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Entity : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;
    [SerializeField] float health = 100;
    public bool isDead { get; private set; }

    public static Action<float> ChangeHpBarValue;


    public void TakeHit(float damage)
    {
        if (isDead) return;

        health -= damage;
        GetComponent<Animator>().Play("TakeHit");

        if (tag == "Player")
        {
            ChangeHpBarValue?.Invoke(health / maxHealth);
        }

        if (health <= 0)
        {
            isDead = true;
            GetComponent<Animator>().Play("Dead");
            //GetComponent<Animator>().SetBool("isDead", isDead); TODO  ����� ����������� ����� �������� ������
        }
    }

    public void Healing(float bonusHealth)
    {
        health += bonusHealth;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    private void onDead() // ����������� � ����� �������� Death
    {
        Destroy(gameObject);
        if (tag == "Player")
        {
            SceneLoader.LoadRestartWindow();
        }
    }
}
