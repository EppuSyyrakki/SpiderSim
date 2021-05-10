using System;
using System.Collections;
using System.Collections.Generic;
using SpiderSim;
using UnityEngine;

namespace SpiderSim
{
    public abstract class Insect : MonoBehaviour, IPooledObject
    {
        public Rigidbody rb;

        public bool canMove = true;

        public virtual void Update()
        {
            if (canMove)
            {
                Move();
            }
            else
            {
                StopMovement();
            }
        }

        public virtual void Move()
        {

        }

        // Stops the insect from moving on its own. Outside force can still affect it a bit.
        public virtual void StopMovement()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        public void Activate(Vector3 position, Quaternion rotation)
        {
            gameObject.SetActive(true);
            transform.position = position;
            transform.rotation = rotation;
            rb = GetComponent<Rigidbody>();
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public GameObject GameObject()
        {
            return gameObject;
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.collider.CompareTag("Web"))
            {
                GetComponent<Animator>().speed = 1;
                canMove = true;
            }
        }
    }

}
