using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveInfo : MonoBehaviour
{
    [Header("Wave infomation")]
    public int numberOfEnemies;
    public int[] spawnLocations;
    public float timeBetweenEnemies;
    public bool openDoor;
    public GameObject doorToOpen;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        doorToOpen.SetActive(false);
    }

}
