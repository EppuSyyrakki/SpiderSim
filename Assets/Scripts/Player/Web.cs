using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim.Player
{
	public class Web : MonoBehaviour, IPooledObject
	{
		private LineRenderer _line;
		private CapsuleCollider _collider;

        public Vector3 beginning;
        public Vector3 end;
        public bool attached = false;

        private NinjaRope _source;

		private void Awake()
		{
			_line = GetComponent<LineRenderer>();
			_collider = GetComponent<CapsuleCollider>();
			_collider.enabled = false;
		}

        public void SetSource(NinjaRope source)
        {
            this._source = source;
        }

        public void SetupWeb(Vector3 beginning, Vector3 end, Quaternion rotation, bool attached = false)
        {
	        this.attached = attached;
            this.beginning = beginning;
	        this.end = end;
	        transform.rotation = rotation;

            if (attached) EnableCollider();
            else Invoke(nameof(EnableCollider), 0.1f);
		}

        private void Update()
        {
	        transform.forward = end - beginning;
	        transform.position = (beginning + end) / 2;
            _line.SetPosition(0, beginning);
            _line.SetPosition(1, end);
            _collider.height = Vector3.Distance(beginning, end) - 1f;
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

        public void EnableCollider()
        {
	        _collider.enabled = true;
        }
    }
}
