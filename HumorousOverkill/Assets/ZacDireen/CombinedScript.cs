﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombinedScript : MonoBehaviour {

    // Rifle/Laser Variables.

    // This is the rifles damage.
    public float RifleDamage = 10f;
    // This is the rifles range.
    public float RifleRange = 100f;
    // This is the rifles muzzle effect.
    public ParticleSystem RifleMuzzleEffect;
    // This is the force applied to an object when it gets hit with rifle ammo.
    public float impactForce = 30.0f;
    // This is the amount maximum amount of ammo the rifle has.
    public float maxRifleAmmo = 15.0f;
    // This is the amount of ammo the rifle currently has.
    private float currentRifleAmmo;
    // This is the reload time of the rifle.
    public float reloadRifleTime = 2.5f;
    // private field showing the shots per second.
    [SerializeField]
    private float ShotsPerSecond;
    // this this is the rate of fire of the weapon.
    public float RoundsPerMinute = 600.0f;
    // this is the nextTimeToFire.
    private float nextTimeToFire = 0f;


    // Shotgun Variables.

    // This is the shotgun's damage per pellet.
    public float PelletDamage = 3.0f;
    // This is the amount of pellets the shotgun has.
    public int pelletCount = 8;
    // This is the Shotguns muzzle effect.
    public ParticleSystem ShotgunMuzzleEffect;
    // This is the amount of force applied to the target when they are hit with a pellet.
    public float pelletForce = 30.0f;
    // This is the Shotguns cone spread.
    public float spreadWidth = 2f;
    // This is the shotguns range.
    public float Range = 10f;
    // This is the amount maximum amount of ammo the shotgun has.
    public float maxShotgunAmmo = 8.0f;
    // This is the amount of ammo the shotgun currently has.
    private float currentShotgunAmmo;
    // This is the reload time of the shotgun.
    public float reloadShotgunTime = 2.5f;
    // This is the shotgun delay so it is not spammable.
    public float FireDelay = 0.4f;



    // Shared/Unique variables. variables
    
    // This boolean tests if you are already reloading or not.
    private bool isReloading;
    // This animatior is resonsible for the reloading mechanic of the weapons.
    public Animator animator;
    // This is a public int of the currently selected weapon.
    public int SelectedWeapon = 0;
    // This is the Fire Rate of the rifle.
    public FireRate fireRate;
    // This is the two different weapon types.
    public GunType gunType;
    // This is where the raycasts of the weapon will begin from.
    public GameObject StartOfRaycast;
    // This is the UI text element for the UI.
    public Text Ammo;

    // This is the Fire Rate of the rifle.
    public enum FireRate
    {
        SEMIAUTO,
        FULLAUTO

    }
    // This is the two different weapon types.
    public enum GunType
    {
        SHOTGUN,
        RIFLE

    }








    // Use this for initialization
    void Start () {

        SelectWeapon();
        currentRifleAmmo = maxRifleAmmo;
        currentShotgunAmmo = maxShotgunAmmo;
    }
	
	// Update is called once per frame
	void Update () {


        if (Input.GetKeyDown(KeyCode.Mouse1) && gunType == GunType.RIFLE)
        {
            if (fireRate == FireRate.FULLAUTO)
            {
                fireRate = FireRate.SEMIAUTO;
            }
            else
            {
                fireRate = FireRate.FULLAUTO;
            }
        }


        int previousSelectedWeapon = SelectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (SelectedWeapon >= transform.childCount - 1)
            {
                SelectedWeapon = 0;
            }
            else
            {
                SelectedWeapon++;
                
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (SelectedWeapon <= 0)
            {
                SelectedWeapon = transform.childCount - 1;
          
            }
            else
            {
                SelectedWeapon--;
                
            }
        }
        if (previousSelectedWeapon != SelectedWeapon)
        {
            SelectWeapon();
        }


        if (SelectedWeapon == 0)
        {
            if (isReloading)
            {
                return;
            }

            if (currentRifleAmmo <= 0)
            {
                StartCoroutine(Reload());
                return;
            }
        }
        
        //if (gunType == GunType.SHOTGUN)
        if(SelectedWeapon == 1)
        {

            if (isReloading)
            {
                return;
            }

            if (currentShotgunAmmo <= 0)
            {
                StartCoroutine(Reload());
                return;
            }
           
        }
        if (SelectedWeapon == 0)
        {
            gunType = GunType.RIFLE;
        }
        if (SelectedWeapon == 1)
        {
            gunType = GunType.SHOTGUN;
        }



   

        if (Input.GetButtonDown("Fire1") && gunType == GunType.RIFLE && fireRate == FireRate.SEMIAUTO)
        {
            nextTimeToFire = Time.time + 60f / RoundsPerMinute;
            Shoot();
        }
        if (Input.GetButton("Fire1") && gunType == GunType.RIFLE && fireRate == FireRate.FULLAUTO)
        {
            nextTimeToFire = Time.time + 60f / RoundsPerMinute;
            Shoot();
        }
        if (Input.GetButtonDown("Fire1") && gunType == GunType.SHOTGUN)
        {
            ShotgunMuzzleEffect.Play();

            for (int i = 0; i < pelletCount; ++i)
            {
                ShootRay();
            }
            currentShotgunAmmo--;
        }



        if (gunType == GunType.RIFLE)
        {
            Ammo.text = currentRifleAmmo + " / " + maxRifleAmmo;
        }
        if (gunType == GunType.SHOTGUN)
        {
            Ammo.text = currentShotgunAmmo + " / " + maxShotgunAmmo;
        }




        }

    IEnumerator Reload()
    {
        if (gunType == GunType.RIFLE)
        {
            isReloading = true;
            animator.SetBool("Reloading", true);
            yield return new WaitForSeconds(reloadRifleTime - 0.25f);
            animator.SetBool("Reloading", false);
            yield return new WaitForSeconds(0.25f);
            currentRifleAmmo = maxRifleAmmo;
            isReloading = false;
        }
        if (gunType == GunType.SHOTGUN)
        {
            isReloading = true;
            animator.SetBool("Reloading", true);
            yield return new WaitForSeconds(reloadShotgunTime - 0.25f);
            animator.SetBool("Reloading", false);
            yield return new WaitForSeconds(0.25f);
            currentShotgunAmmo = maxShotgunAmmo;
            isReloading = false;
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == SelectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
    void Shoot()
    {

        RifleMuzzleEffect.Play();
        // A variable that will store the imformation gathered from the raycast.
        RaycastHit hit;
        Debug.DrawRay(StartOfRaycast.transform.position, StartOfRaycast.transform.forward * 100, Color.blue, 3.0f);
        // If we hit something with our shot raycast.
        //if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) ;
        if (Physics.Raycast(StartOfRaycast.transform.position, StartOfRaycast.transform.forward, out hit, RifleRange)) ;
        {

            // Put in place the takeDamage event handler for the game manager here.
            //GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayerManager>().HandleEvent(GameEvent.)

            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            if (target != null)
            {
                target.TakeDamage(RifleDamage);
            }

        }
        currentRifleAmmo--;
    }

    void ShootRay()
    {
        //  Try this one first, before using the second one
        //  The Ray-hits will form a ring
        float randomRadius = spreadWidth;
        //  The Ray-hits will be in a circular area
        randomRadius = Random.Range(0, spreadWidth);

        float randomAngle = Random.Range(0, 2 * Mathf.PI);

        //Calculating the raycast direction
        Vector3 direction = new Vector3(
            randomRadius * Mathf.Cos(randomAngle),
            randomRadius * Mathf.Sin(randomAngle),Range
            
        );

        //Make the direction match the transform
        //It is like converting the Vector3.forward to transform.forward
        direction = StartOfRaycast.transform.TransformDirection(direction.normalized);

        //Raycast and debug
        Ray r = new Ray(StartOfRaycast.transform.position, direction);

        // the object that gets hit from the raycast.
        RaycastHit hit;
        if (Physics.Raycast(r, out hit))
        {


            Debug.DrawLine(StartOfRaycast.transform.position, hit.point, Color.black, 3.0f);


            Target shotgunTarget = hit.transform.GetComponent<Target>();
            if (shotgunTarget != null)
            {
                shotgunTarget.TakeDamage(PelletDamage);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
    }
    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }
}
