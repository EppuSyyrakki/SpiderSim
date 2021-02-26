using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim
{
    [DisallowMultipleComponent]
    public class Fly : PooledObject
    {
        [SerializeField] Vector3 movementVector;
        [SerializeField] float period = 2f;

        float movementFactor;

        Vector3 startingPos;

        void Update()
        {
            if (period <= Mathf.Epsilon) { return; }
            float cycles = Time.time / period; // Grows continually from 0

            const float tau = Mathf.PI * 2;
            float rawSinWave = Mathf.Sin(cycles * tau);

            movementFactor = rawSinWave / 2f + 0.5f;
            Vector3 offset = movementFactor * movementVector;
            transform.position = startingPos + offset;
        }

        public override void OnObjectSpawn()
        {
            startingPos = transform.position;
        }
    }
}
