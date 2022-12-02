using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioClip enemyHit1, enemyHit2, enemyHit3, jump, playerHit, playerShoot, heartHit;
    private AudioSource mySource;
    // Start is called before the first frame update
    void Start()
    {
        mySource=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudioClip(string clip)
    {
        switch(clip)
        {
            case ("EnemyHit"):
                int randInt = Random.Range(0, 3);
                if(randInt == 0)
                    mySource.PlayOneShot(enemyHit1);
                else if(randInt == 1)
                    mySource.PlayOneShot(enemyHit2);
                else if(randInt == 2)
                    mySource.PlayOneShot(enemyHit3);
                break;
            case ("Jump"):
                mySource.PlayOneShot(jump);
                break;
            case ("PlayerHit"):
                mySource.PlayOneShot(playerHit);
                break;
            case ("PlayerShoot"):
                mySource.PlayOneShot(playerShoot);
                break;
            case ("HeartHit"):
                mySource.PlayOneShot(heartHit);
                break;

        }
    }
}
