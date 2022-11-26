using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public EnemyMover myMover;
    public int enemiesInRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Heart")
        {
            enemiesInRange++;
            myMover.inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Heart")
        {
            enemiesInRange--;
        }

        if(enemiesInRange == 0)
        {
            myMover.inRange = false;
        }
    }
}
