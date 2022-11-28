using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseCharacter : MonoBehaviour
{
    [HideInInspector] public Transform cameraTarget;

    Rigidbody rb;
    CharacterInventory inv;
    [SerializeField] Animator animFirstPerson;
    Stats stats;

    float lookH = 0;
    float lookV = 0;

    [Header("Character Movement Attributes")]
    public float groundSpeed = 5;
    public float jumpStrength = 6;
    public float jumpHoldStrength = 0.5f;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inv = GetComponent<CharacterInventory>();
        stats = GetComponent<Stats>();
    }

    private void FixedUpdate()
    {
        FixedMovement();
    }

    private void Update()
    {
        UpdateMovement();


        if (Input.GetButtonDown("Jump")) JumpPressed();

        if (Input.GetMouseButton(0)) HoldPrimaryAttack();

        if (Input.GetMouseButtonUp(0)) PrimaryAttack();

        if (Input.GetMouseButtonDown(1)) PressBlock();

        if (Input.GetMouseButton(1)) HoldBlock();

        if (Input.GetMouseButtonUp(1)) EndBlock();


    }

    public virtual void FixedMovement() {

        if (Input.GetButton("Jump")) JumpHold();



        Vector3 inputAxis = transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal");
        Vector3 inputAxisClamped = Vector3.ClampMagnitude(inputAxis, 1);

        AddVelocity(inputAxisClamped * groundSpeed);

    }

    public virtual void UpdateMovement() {


        lookV = Mathf.Clamp(lookV - Input.GetAxis("Mouse Y"), -90, 90);
        lookH += Input.GetAxis("Mouse X");

        transform.localRotation = Quaternion.Euler(0, lookH, 0);
        cameraTarget.localRotation = Quaternion.Euler(lookV, 0, 0);

    }

    public virtual void JumpPressed()
    {

        AddVelocity(Vector3.up * jumpStrength);

    }

    public virtual void JumpHold()
    {

        if(rb.velocity.y > 0)
            AddVelocity(Vector3.up * jumpHoldStrength);

    }

    public virtual void PrimaryAttack()
    {

        ItemWeapon attackItem = inv.heldItem;

        if (attackItem.TryAttack())
        {

            animFirstPerson.Play(attackItem.GetAnim(), 0);

        }

    }

    public virtual void HoldPrimaryAttack()
    {

        animFirstPerson.Play("Windup", 0);

    }

    public virtual void PressBlock()
    {

        stats.blockITime = 0.4f;

    }

    public virtual void HoldBlock()
    {

        animFirstPerson.Play("StartBlock");

    }
    public  virtual void EndBlock()
    {
        animFirstPerson.Play("Idle");
    }

    public void AddVelocity(Vector3 vel)
    {
        Vector3 newVelocity = rb.velocity + vel;
        newVelocity.x = vel.x;
        newVelocity.z = vel.z;

        rb.velocity = newVelocity;
    }

    public void TryHitMessage()
    {



    }

}
