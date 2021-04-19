using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BflyMovement : MonoBehaviour
{
    public Transform target;
    public float speed = 1.0f;

    void Update()
    {
        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);


        Quaternion targetRotation = Quaternion.identity;
        Vector3 targetDirection = (target.position - transform.position).normalized;
        targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360*Time.deltaTime);


        /*
        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
        */

    }
}
