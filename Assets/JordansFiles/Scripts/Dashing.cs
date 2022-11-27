using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dashing : MonoBehaviour
{
    [Header("References to self")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;

    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    [Header("Input")]
    public string dashKey = "Dash";

    [Header("Other Objects")]
    private HeartBehaviour theHeart;
    private TempoObjSpawner doStuffChecker;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        theHeart = FindObjectOfType<HeartBehaviour>();
        doStuffChecker = FindObjectOfType<TempoObjSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown(dashKey))
            //Dash();

        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
    }

    public void DoDash(InputAction.CallbackContext ctx)
    {
        Dash();
    }

    private void Dash()
    {
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCd;

        if (doStuffChecker.doStuff == true)
        {
            pm.dashing = true;

            Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;

            delayedForceToApply = forceToApply;
            Invoke(nameof(DelayedDashForce), 0.025f);

            Invoke(nameof(ResetDash), dashDuration);
        }
        else
        {
            if(pm.dead == false)
                theHeart.TakeDamage(2);
        }
    }

    private Vector3 delayedForceToApply;
    private void DelayedDashForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm.dashing = false;
    }
}
