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
    public AudioSource tempoPlayer;
    public TextMeshProUGUI healthText;
    public float startInvulTime;
    public float invulTime;

    private float savedHealth = 100;
    private TempoObjSpawner tempoSpawner;
    private float tempoError;
    private AudioPlayer myAudio;
    // Start is called before the first frame update
    void Start()
    {
        tempoSpawner = FindObjectOfType<TempoObjSpawner>();
        myAudio = FindObjectOfType<AudioPlayer>();
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
        myAudio.PlayAudioClip("HeartHit");
        healthText.text = "Health: " + health;

        if (health <= 0)
            SceneManager.LoadScene(2);
    }

    private void UpdatePitch()
    {
        savedHealth = health;

        //Health between 100 and 0, pitch between 1 and 1.3
        savedPitch = Mathf.Lerp(1.3f, 1f, health / 100);
        pitchToGive = Mathf.Lerp(1.3f, 1f, health / 100);

        tempoSpawner.ProvideNewTempo(pitchToGive);

        musicPlayer.pitch = savedPitch;
        tempoPlayer.pitch = savedPitch;
    }
}
