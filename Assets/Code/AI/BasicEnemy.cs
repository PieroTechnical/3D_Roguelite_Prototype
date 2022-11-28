using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour
{

    Rigidbody rb;
    NavMeshAgent ai;
    Animator anim;
    Stats status;

    [SerializeField] float runSpeed = 10;
    [SerializeField] float attackMoveSpeed = 4;


    [SerializeField] Transform target;
    Vector3 momentum;

    float stunTime;


    private void Awake()
    {
        status = GetComponent<Stats>();
        ai = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        target = FindObjectOfType<BaseCharacter>().transform;
    }


    private void Update()
    {
        if (Vector3.Distance(transform.position, target.position) > 20) return;


        if (Time.realtimeSinceStartup < stunTime)
        {

            ai.enabled = false;
            rb.isKinematic = false;

        }

        else
        {

            ai.enabled = true;
            rb.isKinematic = true;

        }

        anim.SetFloat("moveSpeed", ai.velocity.magnitude);


        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {

            ai.speed = runSpeed;

            if (Vector3.Distance(transform.position, target.position) < 4)
            {

                anim.Play("Attack");

            }

        }

        else
        {

            ai.speed = attackMoveSpeed;

        }

        if(status.health <= 0)
        {

            anim.Play("Dead");
            this.enabled = false;
            ai.enabled = false;
            Destroy(gameObject, 5);

        }

    }

    public void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, target.position) > 20) return;

        if (ai.isActiveAndEnabled) ai.SetDestination(target.position);


    }

    public void TryHitMessage()
    {
        if (!this.enabled) return;

        stunTime = Time.realtimeSinceStartup + 0.3f;

        rb.isKinematic = false;

        rb.AddForce (Vector3.ClampMagnitude(transform.position - target.position, 1) * 250 + Vector3.up * 40);

        anim.Play("Damage");

    }
}
