using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    public PlayerInput myInput;

    float xRotation;
    float yRotation;

    private Vector2 lookingInput;
    private int controlScheme;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if(myInput.currentControlScheme == "Gamepad")
        {
            
        }
        else
        {
            sensX *= 0.1f;
            sensY *= 0.1f;
        }

    }

    // Update is called once per frame
    void Update()
    {
            //Get mouse input
            float mouseX = (lookingInput.x * Time.deltaTime) * sensX;
            float mouseY = (lookingInput.y * Time.deltaTime) * sensY;
            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //Rotate cam and orientation
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void OnMouseMove(InputAction.CallbackContext ctx) => lookingInput = ctx.ReadValue<Vector2>();
}
