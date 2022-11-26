using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HeartBehaviour : MonoBehaviour
{
    public float health;
    public float savedPitch;
    public float pitchToGive;
    public AudioSource musicPlayer;
    public TextMeshProUGUI healthText;
    public float startInvulTime;
    public float invulTime;

    private float savedHealth = 100;
    private TempoObjSpawner tempoSpawner;
    private float tempoError;
    // Start is called before the first frame update
    void Start()
    {
        tempoSpawner = FindObjectOfType<TempoObjSpawner>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(health != savedHealth)
        {
            UpdatePitch();
        }
    }

    private void Update()
    {
        if (invulTime > 0)
            invulTime -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "EnemyBullet" && invulTime <= 0)
        {
            invulTime = startInvulTime;
            TakeDamage(4);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthText.text = "Health: " + health;

        if (health <= 0)
            SceneManager.LoadScene(2);
    }

    private void UpdatePitch()
    {
        savedHealth = health;

        /*switch (health)
        {
            case float n when (n >  80):
                savedPitch = 1.0f;
                tempoSpawner.ProvideNewTempo(1.0f);
                break;
            case float n when (n > 60 && n <= 80):
                savedPitch = 1.1f;
                tempoSpawner.ProvideNewTempo(1.1f);
                break;
            case float n when (n > 40 && n <= 60):
                savedPitch = 1.2f;
                tempoSpawner.ProvideNewTempo(1.2f);
                break;
            case float n when (n > 20 && n <= 40):
                savedPitch = 1.3f;
                tempoSpawner.ProvideNewTempo(1.3f);
                break;
            case float n when (n > 0 && n <= 20):
                savedPitch = 1.4f;
                tempoSpawner.ProvideNewTempo(1.4f);
                break;
        }*/

        //Health between 100 and 0, pitch between 1 and 1.3
        savedPitch = Mathf.Lerp(1.2935f, 0.995f, health / 100);
        pitchToGive = Mathf.Lerp(1.3f, 1f, health / 100);

        tempoSpawner.ProvideNewTempo(pitchToGive);

        //Work out tempo difference
        //Currently 1.24 was wrong, 1.22 was perfect
        //tempoError = (savedPitch - 1f) / 12f;

        musicPlayer.pitch = savedPitch;
    }
}
