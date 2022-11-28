using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempoObjSpawner : MonoBehaviour
{
    [Header("Misc Variables")]
    public GameObject parentCanvas;
    public GameObject tempoToSpawn;
    public float tempoCounter;
    public float currentPitch;
    public AudioSource musicPlayer;
    public AudioSource tempoPlayer;

    [Header("Can the player do stuff")]
    public bool doStuff;

    private GameObject tempSpawn;
    private bool allowUpdate = false;
    private bool newPitchIsHere;
    private float pitchToUpdate;
    private bool looping = true;
    private int tempoCount;

    //NOTE: Might need to add code to FixedUpdate to reduce this number the higher the pitch gets
    private float allowStuffTimer = 20;

    private void Start()
    {
        StartCoroutine(WaitAtStart());
    }

    private void FixedUpdate()
    {
        if (allowUpdate == true)
        {
            //Each tempo objects sets doStuff here to true when it gets to the middle. When it sets it to true, count the timer
            //Down until it reaches 0, then set doStuff to false
            /*if(doStuff == true)
            {
                allowStuffTimer -= currentPitch;
                if(allowStuffTimer <= 0)
                {
                    allowStuffTimer = 20;
                    doStuff = false;
                }
            }*/
        }
    }

    private void Update()
    {
        if (allowUpdate == true)
        {
            if(tempoPlayer.isPlaying == false)
            {
                SpawnTempo();
                tempoPlayer.Play();

                tempoCount++;

                if(tempoCount == 5)
                {
                    musicPlayer.Stop();
                    musicPlayer.Play();
                    tempoCount = 1;
                }
            }
        }
    }

    private void SpawnTempo()
    {
        //Do over the spawn rate, calculate mistake, add mistake on at the end of the next turn
        tempSpawn = Instantiate(tempoToSpawn);
        tempSpawn.transform.SetParent(parentCanvas.transform);
        tempSpawn.transform.localPosition = new Vector3(0, 0, 1);
        tempSpawn.transform.localScale = new Vector3(1, 1, 1);

        //Check if there's a new pitch, if there is, update it here and set newPitchIsHere to false
        if (newPitchIsHere)
        {
            newPitchIsHere = false;
            currentPitch = pitchToUpdate;
            musicPlayer.pitch = currentPitch;
        }

        //Reset tempo back to 0
        //tempoCounter = 0;
    }

    IEnumerator WaitAtStart()
    {
        yield return new WaitForSeconds(2.0f);
        musicPlayer.Play();
        tempoPlayer.Play();
        allowUpdate = true;
        tempoCount++;
        SpawnTempo();
        //yield return new WaitForSeconds(0.25f);
        //StartCoroutine(SpawnOnLoop());
    }

    IEnumerator SpawnOnLoop()
    {
        while(looping == true)
        {
            yield return new WaitForSecondsRealtime(1 / currentPitch);
            Debug.Log("Spawning");
            SpawnTempo();
        }
    }

    public void CallStuffCoroutine()
    {
        StartCoroutine(DisableStuffAfterFrames(((1.0f / Time.deltaTime) / 4f) / currentPitch));
    }

    IEnumerator DisableStuffAfterFrames(float framesToWait)
    {
        for(int i = 0; i <= framesToWait; i++)
            yield return null;

        doStuff = false;
    }

    public void ProvideNewTempo(float newPitch)
    {
        //pitchToUpdate = newPitch;
        currentPitch = newPitch;
        //newPitchIsHere = true;
    }
}
