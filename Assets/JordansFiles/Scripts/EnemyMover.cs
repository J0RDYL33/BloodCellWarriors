using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public float speed;
    public bool inRange;
    public GameObject enemyInRange;
    public bool hasEnemyLocked;
    public int health;
    public float invulTime;
    public float startInvulTime;

    private GameObject heartObject;
    private float index;
    private Vector3 moveTo;
    private EnemySpawner mySpawner;
    // Start is called before the first frame update
    void Start()
    {
        inRange = false;
        heartObject = FindObjectOfType<HeartBehaviour>().gameObject;
        moveTo = heartObject.transform.position;
        mySpawner = FindObjectOfType<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        //Make the enemy bob up and down
        index += Time.deltaTime;
        float bobPos = Mathf.Abs(3.0f * Mathf.Sin(2.0f*index) + 5f);

        var step = speed * Time.deltaTime;
        Vector3 newPos = Vector3.MoveTowards(transform.position, moveTo, step);

        //If a player or the heart is in range, it'll call inRange, not letting it move other than bobbing
        if (!inRange)
        {
            transform.localPosition = new Vector3(newPos.x, bobPos, newPos.z);

            //Rotate towards direction moving
            Vector3 targetDirection = moveTo - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step/10f, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
        else
        {
            transform.localPosition = new Vector3(transform.position.x, bobPos, transform.position.z);

            //Rotate towards direction moving
            Vector3 targetDirection = enemyInRange.gameObject.transform.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step / 10f, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        if (invulTime > 0)
            invulTime -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerBullet" && invulTime <= 0)
        {
            health--;
            invulTime = startInvulTime;

            if(health <= 0)
            {
                mySpawner.enemiesLeft--;
                Destroy(this.gameObject);
            }
        }
    }
}
