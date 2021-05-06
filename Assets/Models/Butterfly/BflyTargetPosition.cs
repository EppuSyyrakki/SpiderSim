using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BflyTargetPosition : MonoBehaviour
{
    public Transform Butterfly;
    float randX, randY, randZ;
    private Vector3 startPos;
    public bool MoveDirection = true;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Butterfly.transform.position == this.gameObject.transform.position)
            if (MoveDirection == true)
            {
                {
                    randX = Random.Range(startPos.x - 40f, startPos.x + 40f);
                    randY = Random.Range(startPos.y - 40f, startPos.y + 40f);
                    randZ = Random.Range(startPos.z, startPos.z + 70f);
                    transform.position = new Vector3(randX, randY, randZ);
                }
            }
        else
            {
                {
                    randX = Random.Range(startPos.x - 40f, startPos.x + 40f);
                    randY = Random.Range(startPos.y - 40f, startPos.y + 40f);
                    randZ = Random.Range(startPos.z, startPos.z - 70f);
                    transform.position = new Vector3(randX, randY, randZ);
                }
            }
    }
}
