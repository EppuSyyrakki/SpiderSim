using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim.Web
{
	public class Web : MonoBehaviour
	{
		private LineRenderer line;

		private void Awake()
		{
			line = GetComponent<LineRenderer>();
		}
	}
}
