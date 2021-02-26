using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim.Web
{
	public class Web : MonoBehaviour, IPooledObject
	{
		private LineRenderer line;

        public Vector3 beginning;
        public Vector3 end;
        public bool attached = false;

        private WebSource source;

		private void Awake()
		{
			line = GetComponent<LineRenderer>();
            beginning = transform.position;
            end = transform.position;
        }

        public void SetSource(WebSource source)
        {
            this.source = source;
        }

        private void Update()
        {
            if (!attached)
            {
                beginning = source.transform.position;
                line.SetPosition(0, beginning);
            }

            line.SetPosition(1, end);
        }

        public void OnObjectSpawn()
        {
	        
        }
	}
}
