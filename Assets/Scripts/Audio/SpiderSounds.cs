using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiderSim.Audio
{
    public class SpiderSounds : MonoBehaviour
    {
	    [FMODUnity.EventRef]
	    public string spiderWalk, spiderEat;

	    private SFX walk;
	    private SFX eat;
        
        // Start is called before the first frame update
        void Start()
        {
	        Rigidbody rb = GetComponent<Rigidbody>();
            walk.Init(spiderWalk, null, rb);
            eat.Init(spiderEat, null, rb);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
