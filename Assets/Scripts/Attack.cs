using System;
using UnityEngine;

public class PlayerAttackState : MonoBehaviour
{
    private Animator MyAniamtor;

    public float cooldownTime = 2f;
    private float nextFireTime = 0;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1;

    private void Start()
    {
        MyAniamtor = GetComponent<Animator>();
    }

    public void Update()
    {
        if (MyAniamtor.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && MyAniamtor.GetCurrentAnimatorStateInfo(0).IsName("LightAttack1"))
        {
            MyAniamtor.SetBool("hit1", false);
        }
        if (MyAniamtor.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && MyAniamtor.GetCurrentAnimatorStateInfo(0).IsName("LightAttack2"))
        {
            MyAniamtor.SetBool("hit2", false);
        }
        if (MyAniamtor.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && MyAniamtor.GetCurrentAnimatorStateInfo(0).IsName("LightAttack3"))
        {
            MyAniamtor.SetBool("hit3", false);
        }
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
            MyAniamtor.SetFloat("Movement", 0, 0.1f, Time.deltaTime);
        }

        if (Time.time > nextFireTime)
        {
            if (Input.GetMouseButton(0))
            {
                PlayerInput_AttackEvent();
            }
        }

    }

    private void PlayerInput_AttackEvent()
    {
        lastClickedTime = Time.time;
        noOfClicks++;
        if (noOfClicks == 1)
        {
            MyAniamtor.SetBool("hit1", true);
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        if (noOfClicks >= 2 && MyAniamtor.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && MyAniamtor.GetCurrentAnimatorStateInfo(0).IsName("LightAttack1"))
        {
            MyAniamtor.SetBool("hit1", false);
            MyAniamtor.SetBool("hit2", true);
        }

        if (noOfClicks >= 3 && MyAniamtor.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && MyAniamtor.GetCurrentAnimatorStateInfo(0).IsName("LightAttack2"))
        {
            MyAniamtor.SetBool("hit2", false);
            MyAniamtor.SetBool("hit3", true);
        }
    }

}
