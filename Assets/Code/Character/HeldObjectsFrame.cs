using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldObjectsFrame : MonoBehaviour
{

    public Transform parent;
    public Vector3 position;
    public Quaternion rotation;
    [SerializeField] float heldObjectsFollowSpeed;
    
    // Start is called before the first frame update
    void Start()
    {

        //parent = GetComponentInParent<Transform>();

        position = parent.position;
        rotation = parent.rotation;

    }

    // Update is called once per frame
    void Update()
    {

        //position = Vector3.Lerp(position, parent.position, heldObjectsFollowSpeed);
        //rotation = Quaternion.Lerp(rotation, parent.rotation, heldObjectsFollowSpeed);

        transform.position = Vector3.Lerp(transform.position, parent.position, heldObjectsFollowSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, parent.rotation, heldObjectsFollowSpeed * Time.deltaTime);

    }
}
