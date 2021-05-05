using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim.Audio
{
    public class SpiderSounds : MonoBehaviour
    {
	    [FMODUnity.EventRef]
	    public string spiderWalk, spiderEat;

	    [SerializeField]
	    private Transform soundSource;

	    private SFX walk = new SFX();
	    private SFX eat = new SFX();

	    private List<SpiderStep> feet = new List<SpiderStep>();

	    private void OnEnable()
	    {
		    feet.AddRange(GetComponentsInChildren<SpiderStep>());

			foreach (var foot in feet)
		    {
			    foot.Stepped += OnStepTriggered;
		    }
	    }

	    private void OnDisable()
	    {
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
        }

        private void OnStepTriggered()
        {
			
            walk.Play(gameObject);
        }
    }
}
