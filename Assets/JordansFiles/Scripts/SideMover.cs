using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMover : MonoBehaviour
{
    public GameObject leftMover;
    public GameObject rightMover;

    public float currentDistance;
    //public float currentPitch;

    private TempoObjSpawner mySpawner;
    private bool hasTriggeredAllow = false;
    private float leftPos = -600f;
    private float rightPos = 600f;
    // Start is called before the first frame update
    void Start()
    {
        mySpawner = FindObjectOfType<TempoObjSpawner>();
    }


    private void Update()
    {
        if (leftMover.transform.localPosition.x >= 0)
        {
            mySpawner.CallStuffCoroutine();
            Destroy(this.gameObject);
        }

        //Move each side depending on how fast the music is going (At basic rate, pitch is 1.0, needs to move 100 position in 1 second)
        switch (mySpawner.currentPitch)
        {
            case (1.0f):
                currentDistance -= (100.0f / 50.0f);
                break;
            case (1.1f):
                currentDistance -= (100.0f / 45.0f);
                break;
            case (1.2f):
                currentDistance -= (100.0f / 40.0f);
                break;
            case (1.3f):
                currentDistance -= (100.0f / 35.0f);
                break;
            case (1.4f):
                currentDistance -= (100.0f / 30.0f);
                break;
        }

        //Set the side tempos to the new position
        leftPos += (100 * mySpawner.currentPitch) * Time.deltaTime;
        leftMover.transform.localPosition = new Vector3(leftPos, 0.0f, 1.0f);
        rightPos -= (100 * mySpawner.currentPitch) * Time.deltaTime;
        rightMover.transform.localPosition = new Vector3(rightPos, 0.0f, 1.0f);

        if(rightPos <= (25 * (mySpawner.currentPitch)) && hasTriggeredAllow == false)
        {
            hasTriggeredAllow = true;
            mySpawner.doStuff = true;
        }
    }
}
