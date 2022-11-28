using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapdoor : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.transform.GetComponent<BaseCharacter> () != null)
        {

            Application.LoadLevel(0);

        }

    }
}
