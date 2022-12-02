using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponPistol : MonoBehaviour
{
    public Gun[] loadout;
    public Transform weaponParent;

    //public GameObject bulletholePrefab;
    //public LayerMask canBeShot;

    private int currentIndex;
    private GameObject currentWeapon;
    private bool aiming;
    private TempoObjSpawner doStuff;

    // Start is called before the first frame update
    void Start()
    {
        doStuff = FindObjectOfType<TempoObjSpawner>();
        Equip(0);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1)) Equip(0);
        if (currentWeapon != null)
        {
            Aim();
            /*if (Input.GetMouseButton(0))
            {
                Shoot();
            }*/
            currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
        }
    }

    // Chooses the weapon to be Equipped from the loadout
    void Equip(int p_ind)
    {
        if (currentWeapon != null) Destroy(currentWeapon);

        currentIndex = p_ind;

        GameObject t_newWeapon = Instantiate(loadout[p_ind].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
        //t_newWeapon.transform.localPosition = Vector3.zero;
        //t_newWeapon.transform.localEulerAngles = Vector3.zero;

        currentWeapon = t_newWeapon;
    }

    public void EnableAimBool(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            aiming = true;
        }
        else
        {
            aiming = false;
        }
    }

    public void Aim()
    {
        Transform t_anchor = currentWeapon.transform.Find("Anchor");
        Transform t_state_ads = currentWeapon.transform.Find("States/ADS");
        Transform t_state_hip = currentWeapon.transform.Find("States/Hip");

        if (aiming)
        {
            //is Aiming
            t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_ads.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
        }
        else
        {
            //not Aiming
            t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_hip.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
        }
    }
     
    public void Shoot()
    {
        if (doStuff.doStuff)
        {
            Transform t_spawn = transform.Find("PlayerCameraRoot");

            /*//Weapon Bloom
            Vector3 t_bloom = t_spawn.position + t_spawn.forward * 1000f;
            t_bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * t_spawn.up;
            t_bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * t_spawn.right;
            t_bloom -= t_spawn.position;
            t_bloom.Normalize();

            //Raycast
            RaycastHit t_hit = new RaycastHit();
            if (Physics.Raycast(t_spawn.position, t_bloom, out t_hit, 1000f, canBeShot))
            {
                GameObject t_newHole = Instantiate(bulletholePrefab, t_hit.point + t_hit.normal * 0.001f, Quaternion.identity) as GameObject;
                t_newHole.transform.LookAt(t_hit.point + t_hit.normal);
                Destroy(t_newHole, 5f);
            }*/


            // Quaternion initial_rotation = currentWeapon.transform.localRotation;

            //WeaponRecoil
            //currentWeapon.transform.Rotate(-loadout[currentIndex].recoil, 0, 0);

            //Weapon Kickback
            currentWeapon.transform.position -= currentWeapon.transform.forward * loadout[currentIndex].kickBack;

            //Quaternion final_rotation = currentWeapon.transform.localRotation;

            //currentWeapon.transform.localRotation = Quaternion.Lerp(final_rotation, initial_rotation, 1f);

        }

    }
}
