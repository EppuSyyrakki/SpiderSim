using UnityEngine;
using FMOD.Studio;

namespace SpiderSim.Audio
{
	/// <summary>
	/// Wrapper for audio effects to initialize and destroy it "automatically"
	/// </summary>
	public class SFX
	{
		private EventInstance SFXInstance;
		
		/// <summary>
		/// Initializes the audio event.
		/// </summary>
		/// <param name="eventName">The name of the fmod event to attach</param>
		/// <param name="transform">the transform the attach this event to</param>
		/// <param name="rb">the physics object to attach this event to</param>
		public void Init(string eventName, Transform transform = null, Rigidbody rb = null)
		{
			SFXInstance = FMODUnity.RuntimeManager.CreateInstance(eventName);

			if (SFXInstance.isValid())
			{
				if (transform != null || rb != null)
				{
					FMODUnity.RuntimeManager.AttachInstanceToGameObject(SFXInstance, transform, rb);
				}
			}
			else
			{
				Debug.LogWarning("SFX wrapper event " + eventName + " is not valid");
			}
		}

		~SFX()
		{
			if (SFXInstance.isValid())
			{
				SFXInstance.stop(STOP_MODE.ALLOWFADEOUT);
				SFXInstance.release();
				SFXInstance.clearHandle();
			}
		}

		public void Play(float volume = 1f)
		{
			SFXInstance.setVolume(volume);
			SFXInstance.start();
		}

		public void Restart()
		{
			SFXInstance.stop(STOP_MODE.IMMEDIATE);
			SFXInstance.start();
		}

		public void Stop()
		{
			SFXInstance.stop(STOP_MODE.ALLOWFADEOUT);
		}
	}
}
