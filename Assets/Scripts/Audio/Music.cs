using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim.Audio
{
	public class Music : MonoBehaviour
	{
		private enum SceneMusic
		{
			intro = 0,
			kitchen,
			bathroom
		}

		[SerializeField]
		private SceneMusic sceneMusic = 0;

		[FMODUnity.EventRef]
		public string introMusic, kitchenMusic, bathroomMusic;

		private SFX intro = new SFX();
		private SFX kitchen = new SFX();
		private SFX bathroom = new SFX();

		private void Start()
		{
			Rigidbody nullRb = GetComponent<Rigidbody>();

			switch (sceneMusic)
			{
				case SceneMusic.intro:
					intro.Init(introMusic, transform, nullRb);
					intro.Play(gameObject);
					break;
				case SceneMusic.kitchen:
					kitchen.Init(kitchenMusic, transform, nullRb);
					kitchen.Play(gameObject);
					break;
				case SceneMusic.bathroom:
					bathroom.Init(bathroomMusic, transform, nullRb);
					bathroom.Play(gameObject);
					break;
			}
		}
	}
}
