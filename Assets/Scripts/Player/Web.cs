using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim.Player
{
	public class Web : MonoBehaviour, IPooledObject
	{
		[SerializeField]
		private float colliderEndSpace = 1f;

		private LineRenderer _line;
		private BoxCollider _collider;

        public Vector3 beginning;
        public Vector3 end;
        public bool attached = false;

        private NinjaRope _source;

        private void Awake()
		{
			_line = GetComponent<LineRenderer>();
			_collider = GetComponent<BoxCollider>();
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

            // if we are a new web we shouldn't enable the collider right away to prevent the spider from
            // acting weird. If we are an old web being called from the pool, we can enable it right away.
            if (attached) EnableCollider();
            else Invoke(nameof(EnableCollider), 0.1f);
		}

        private void Update()
        {
            // set this object's forward to face the end point and move the object itself to halfway between
            // beginning and end. This is done so the collider is in the right position and rotation.
	        // TODO: end - beginning might be zero, logs a warning, FIX
	        float endRoom = colliderEndSpace;

            if (!attached) endRoom = colliderEndSpace * 5;

            transform.forward = end - beginning;
	        transform.position = (beginning + end) / 2;
            _line.SetPosition(0, beginning);
            _line.SetPosition(1, end);
            float length = Mathf.Abs(Vector3.Distance(beginning, end) - endRoom);
            Vector3 size = _collider.size;
            _collider.size = new Vector3(size.x, size.y, length);

            /*
            if (!attached)
            {
                if (Physics.Linecast(beginning, end, out RaycastHit hit, mask))
                {
                    IPooledObject newFromPool = ObjectPooler.Instance.SpawnFromPool("Web", _currentWeb.beginning, Quaternion.identity);
                    Web newWeb = newFromPool.GameObject().GetComponent<Web>();
                    newWeb.SetSource(this);
                    newWeb.SetupWeb(_currentWeb.beginning, _currentWeb.end, Quaternion.LookRotation(_currentWeb.end - _currentWeb.beginning), true);

                }
            }
            */
        }

        public void Activate(Vector3 position, Quaternion rotation)
        {
	        gameObject.SetActive(true);
            transform.position = position;
	        transform.rotation = rotation;
            gameObject.layer = 6;
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
