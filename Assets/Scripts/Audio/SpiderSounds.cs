using System;
using System.Collections;
using System.Collections.Generic;
using SpiderSim.Player;
using UnityEngine;

namespace SpiderSim.Audio
{
    public class SpiderSounds : MonoBehaviour
    {
	    [FMODUnity.EventRef]
	    public string spiderWalk, spiderEat, spiderShoot;

	    [SerializeField]
	    private Transform soundSource;

	    private SFX walk = new SFX();
	    private SFX eat = new SFX();
	    private SFX shot = new SFX();

	    private List<SpiderStep> feet = new List<SpiderStep>();

	    private NinjaRope ninjaRope;

	    private void OnEnable()
	    {
		    ninjaRope = GetComponentInChildren<NinjaRope>();
		    ninjaRope.WebShot += OnWebShot;
		    feet.AddRange(GetComponentsInChildren<SpiderStep>());

			foreach (var foot in feet)
		    {
			    foot.Stepped += OnStepTriggered;
		    }
	    }

	    private void OnDisable()
	    {
		    ninjaRope.WebShot -= OnWebShot;

			foreach (var foot in feet)
			{
				foot.Stepped -= OnStepTriggered;
			}
		}

	    // Start is called before the first frame update
        void Start()
        {
	        Rigidbody rb = GetComponent<Rigidbody>();
            walk.Init(spiderWalk, soundSource, rb);
            eat.Init(spiderEat, soundSource, rb);
			shot.Init(spiderShoot, soundSource, rb);
        }

        private void OnStepTriggered()
        {
	        walk.Play(gameObject);
        }

        private void OnWebShot()
        {
			shot.Play(gameObject);
        }
    }
}
