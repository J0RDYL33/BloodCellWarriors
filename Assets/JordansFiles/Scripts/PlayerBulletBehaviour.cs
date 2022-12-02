using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehaviour : MonoBehaviour
{
    public float speed;
    public float timeTilDespawn;
    private AudioPlayer myAudio;
    public GameObject bloodParticles;
    public GameObject greenParticles;

    private void Start()
    {
        myAudio = FindObjectOfType<AudioPlayer>();
        myAudio.PlayAudioClip("PlayerShoot");
    }

    // Update is called once per frame
    void Update()
    {
        timeTilDespawn -= Time.deltaTime;

        if (timeTilDespawn <= 0)
            Destroy(this.gameObject);

        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            return;

        if(other.gameObject.tag == "Enemy")
            Instantiate(greenParticles, this.transform.position, this.transform.rotation);
        else
            Instantiate(bloodParticles, this.transform.position, this.transform.rotation);

        StartCoroutine(DestroyMyself());
    }

    IEnumerator DestroyMyself()
    {
        Instantiate(bloodParticles, this.transform.position, this.transform.rotation);
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }
}
