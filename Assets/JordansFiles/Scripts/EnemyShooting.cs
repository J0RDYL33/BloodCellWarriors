using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public float startShootCooldown;
    public float shootCooldown;
    public GameObject bulletSpawn;
    public GameObject bullet;

    private EnemyMover myMover;
    // Start is called before the first frame update
    void Start()
    {
        myMover = GetComponentInParent<EnemyMover>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shootCooldown > 0)
            shootCooldown -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player" || other.transform.tag == "Heart")
        {
            //myMover.enemyInRange = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (myMover.enemyInRange == other.gameObject)
        {
            Debug.Log("Locked on enemy has left zone");
            myMover.hasEnemyLocked = false;
            myMover.enemyInRange = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "Player" || other.transform.tag == "Heart")
        {
            if(myMover.hasEnemyLocked == false)
            {
                myMover.enemyInRange = other.gameObject;
                myMover.hasEnemyLocked = true;
            }

            if(shootCooldown <= 0)
            {
                //Instantiate bullet
                Debug.Log("Shooting at " + other.gameObject.name);
                GameObject newBullet = Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                BulletBehaviour newBulletBeh = newBullet.GetComponent<BulletBehaviour>();
                newBulletBeh.playerPosWhenFired = myMover.enemyInRange.transform.position;
                newBulletBeh.bulletPosWhenFired = bulletSpawn.transform.position;
                
                //Reset cooldown
                shootCooldown = startShootCooldown;
            }
        }
    }
}
