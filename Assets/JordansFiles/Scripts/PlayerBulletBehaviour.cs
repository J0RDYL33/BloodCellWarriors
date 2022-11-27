using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehaviour : MonoBehaviour
{
    public float speed;
    public float timeTilDespawn;

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
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
            return;

        StartCoroutine(DestroyMyself());
    }

    IEnumerator DestroyMyself()
    {
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }
}
