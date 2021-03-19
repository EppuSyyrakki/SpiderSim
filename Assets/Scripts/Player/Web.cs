using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim.Player
{
	public class Web : MonoBehaviour, IPooledObject
	{
		private LineRenderer line;

        public Vector3 beginning;
        public Vector3 end;
        public bool attached = false;

        private NinjaRope source;

		private void Awake()
		{
			line = GetComponent<LineRenderer>();
		}

        public void SetSource(NinjaRope source)
        {
            this.source = source;
        }

        public void SetupWeb(Vector3 beginning, Vector3 end, bool attached = false)
        {
	        this.attached = attached;
            this.beginning = beginning;
	        this.end = end;
        }

        private void Update()
        {
	        transform.position = beginning;
            line.SetPosition(0, beginning);
            line.SetPosition(1, end);
        }

        public void Activate(Vector3 position, Quaternion rotation)
        {
	        gameObject.SetActive(true);
            transform.position = position;
	        transform.rotation = rotation;
        }

        public void Deactivate()
        {
	        gameObject.SetActive(false);
        }

        public GameObject GameObject()
        {
	        return gameObject;
        }
    }
}
