using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCorrect : MonoBehaviour
{
    static int playerIndex;
    public int publicIndex;
    
    public void PlayerJoined()
    {
        playerIndex += 1;
        publicIndex = playerIndex;
    }
}
