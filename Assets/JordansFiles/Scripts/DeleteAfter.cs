using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeleteMe());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DeleteMe()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
