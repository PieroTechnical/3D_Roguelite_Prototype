using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    BaseCharacter character;


    void Awake()
    {
        character = GetComponent<BaseCharacter>();
    }

    
    void Update()
    {


    }
}
