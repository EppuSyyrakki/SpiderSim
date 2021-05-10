using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim.Audio
{
	public class InsectSounds : MonoBehaviour
	{
		private enum SoundType
		{
			walk = 0,
			walkFast,
			buzz,
			buzzLow
		}

		[SerializeField]
		private SoundType soundType;

		[FMODUnity.EventRef]
		public string insectWalk, insectWalkFast, flyBuzz, flyBuzzLow;

		private SFX walk = new SFX();
		private SFX walkFast = new SFX();
		private SFX buzz = new SFX();
		private SFX buzzLow = new SFX();

		// Start is called before the first frame update
		void Start()
		{
			Rigidbody rb = GetComponent<Rigidbody>();

			switch (soundType)
			{
				case SoundType.walk:
					walk.Init(insectWalk, transform, rb);
					walk.Play(gameObject);
					break;
				case SoundType.walkFast:
					walkFast.Init(insectWalkFast, transform, rb);
					walkFast.Play(gameObject);
					break;
				case SoundType.buzz:
					buzz.Init(flyBuzz, transform, rb);
					buzz.Play(gameObject);
					break;
				case SoundType.buzzLow:
					buzzLow.Init(flyBuzzLow, transform, rb);
					buzzLow.Play(gameObject);
					break;
			}
		}
	}
}
