using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float speed;
    public Vector3 playerPosWhenFired;
    public Vector3 bulletPosWhenFired;
    public float timeTilDespawn;
    public GameObject bloodParticles;
    // Start is called before the first frame update
    void Start()
    {
        SetVariables();
    }

    public void SetVariables()
    {
        //Rotate towards direction
        Vector3 targetDirection = playerPosWhenFired - bulletPosWhenFired;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 1000, 1000.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
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
        if(other.gameObject.tag != "Enemy")
            StartCoroutine(DestroyMyself());

        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Heart")
            Instantiate(bloodParticles, this.transform.position, this.transform.rotation);
    }

    IEnumerator DestroyMyself()
    {
        
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }
}
