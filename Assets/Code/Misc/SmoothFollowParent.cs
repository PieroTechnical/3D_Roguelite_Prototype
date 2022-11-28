using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowParent : MonoBehaviour
/*

    Causes the attached transform to smoothly lerp to the target on Update instead of snapping every frame on Update. It does not follow the transform of its parent.
 
*/
{
    [SerializeField] float lerpSpeed = 0.1f;
    Vector3 lastPosition;
    public Transform target;

    void Start()
    {
        lastPosition = transform.position;
    }


    private void Update()
    {
        lastPosition = transform.position;
        transform.position = Vector3.Lerp(lastPosition, target.position, lerpSpeed);
    }


    private void LateUpdate()
    {
        transform.position = lastPosition;
    }
}
