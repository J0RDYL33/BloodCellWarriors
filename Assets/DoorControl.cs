using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorControl : MonoBehaviour
{
    public GameObject Door1;
    public GameObject Door2;
    public GameObject Door3;
    public GameObject Door4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) { 
            Door1.SetActive(false);
            Door2.SetActive(false);
            Door3.SetActive(true);
            Door4.SetActive(true);
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Door1.SetActive(false);
            Door2.SetActive(false);
            Door3.SetActive(false);
            Door4.SetActive(true);
        }

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            Door1.SetActive(false);
            Door2.SetActive(false);
            Door3.SetActive(false);
            Door4.SetActive(false);
        }
    }
}
