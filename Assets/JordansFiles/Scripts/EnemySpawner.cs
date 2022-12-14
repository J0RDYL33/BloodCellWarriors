using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    //All the waves on objects
    public WaveInfo[] arrayOfWaves;
    public GameObject enemyPrefab;
    public int enemiesLeft;
    public TextMeshProUGUI enemyText;
    public GameObject waveText;

    private Vector3[] spawnLocations = new Vector3[4];
    private int currentWave;
    // Start is called before the first frame update
    void Start()
    {
        //Set the value of enemiesLeft to the number of enemies in wave 1
        enemiesLeft = arrayOfWaves[0].numberOfEnemies;

        //List all the spawn locations and store them in spawnLocations
        spawnLocations[0] = new Vector3(0, 4, -150);
        spawnLocations[1] = new Vector3(0, 4, 150);
        spawnLocations[2] = new Vector3(150, 4, 12);
        spawnLocations[3] = new Vector3(-150, 4, 12);

        if (arrayOfWaves[currentWave].openDoor)
            arrayOfWaves[currentWave].OpenDoor();

        //Wait 5 seconds before starting first wave
        StartCoroutine(WaveCooldown());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        enemyText.text = "Enemies Left: " + enemiesLeft;

        if(enemiesLeft == 0)
        {
            currentWave++;

            if (currentWave > arrayOfWaves.Length)
                SceneManager.LoadScene(6);

            enemiesLeft = arrayOfWaves[currentWave].numberOfEnemies;

            if (arrayOfWaves[currentWave].openDoor)
                arrayOfWaves[currentWave].OpenDoor();

            StartCoroutine(WaveCooldown());
        }
    }

    IEnumerator WaveCooldown()
    {
        waveText.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        StartCoroutine(SpawnEnemies());
        waveText.SetActive(false);
    }

    IEnumerator SpawnEnemies()
    {
        //Spawn the amount of enemies set in that wave
        for (int i = 0; i < arrayOfWaves[currentWave].numberOfEnemies; i++)
        {
            //Choose a location available to spawn the enemy
            int randLocation = Random.Range(0, arrayOfWaves[currentWave].spawnLocations.Length);

            int locationToSpawn = arrayOfWaves[currentWave].spawnLocations[randLocation];

            //Instantiate the enemy
            Instantiate(enemyPrefab, spawnLocations[locationToSpawn], Quaternion.Euler(0,0,0));

            //Wait the amount of time specified in the wave object
            yield return new WaitForSeconds(arrayOfWaves[currentWave].timeBetweenEnemies);
        }
    }
}
