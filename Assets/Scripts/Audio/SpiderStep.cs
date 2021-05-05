using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim.Audio
{
	public class SpiderStep : MonoBehaviour
	{
		// This event triggers when the step is done
		public event System.Action Stepped;

		private void OnTriggerEnter(Collider other)
		{
			// When the trigger hits something (the walking surface), tell SpiderSounds class to play a sound
			// SpiderSounds is the client of this class
		}
	}
}
