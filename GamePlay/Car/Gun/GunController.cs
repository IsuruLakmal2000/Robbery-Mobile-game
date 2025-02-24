using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    [SerializeField] private GameObject v1Prefab;
    [SerializeField] private GameObject v2Prefab;
    [SerializeField] private GameObject v3Prefab;
    [SerializeField] private GameObject v4Prefab;

    [SerializeField] private GameObject bulletPrefab;  // Assign your bullet prefab
    private float fireRate = 0.2f;    // Fire rate in seconds
    [SerializeField] private float bulletSpeed = 10f;  // Speed of the bullet
    [SerializeField] private int bulletDamage = 10;    // Damage per bullet

    private float nextFire = 0f;                         // Track when the next bullet can be fired
    private bool isFiring = false;

    private Animator gunAnimator;

    private Transform gunArm; // Assign in Inspector
    public float rotationSpeed = 5f; // Speed of rotation
    public float maxRange = 10f; // Maximum range to detect police cars
    private Transform player; // Player's position
    private Transform nearestPolice; // Closest police car

    public static GunController instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {

        player = GameObject.FindGameObjectWithTag("PlayerCar").transform;
        String activatedGun = PlayerPrefs.GetString("SelectedGun", "none");

        switch (activatedGun)
        {
            case "V1":
                Instantiate(v1Prefab, transform);
                break;
            case "V2":
                Instantiate(v2Prefab, transform);
                break;
            case "V3":
                Instantiate(v3Prefab, transform);
                break;
            case "V4":
                Instantiate(v4Prefab, transform);
                break;
        }
        if (activatedGun != "none")
        {
            gunArm = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).transform;
            gunAnimator = gunArm.GetComponent<Animator>();
            nextFire = Time.time;
        }



    }

    void Update()
    {
        // Find all police cars in the scene
        if (gunArm != null)
        {
            GameObject[] policeCars = GameObject.FindGameObjectsWithTag("PoliceCar");

            // // Find the nearest police car within range
            nearestPolice = FindNearestPolice(policeCars);

            if (nearestPolice != null)
            {
                // Rotate the gun arm towards the nearest police car
                RotateTowardsTarget(nearestPolice.position);
            }
            isFiring = true;
        }

    }
    public void FireBullet()
    {
        if (isFiring && Time.time >= nextFire && nearestPolice != null)
        {
            gunAnimator.SetTrigger("Fire");
            GameObject bullet = Instantiate(bulletPrefab, gunArm.position, gunArm.rotation);

            // Get the Bullet script and set its properties
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.speed = bulletSpeed;
                bulletScript.damage = bulletDamage;
                bulletScript.target = nearestPolice.position;
            }

            // Optional: Add a debug line to show the bullet's path
            Debug.DrawLine(gunArm.position, nearestPolice.position, Color.red, 0.1f);
            nextFire = Time.time + fireRate;
        }
        // Instantiate the bullet at the tip of the gun arm

    }

    // Call this method to adjust the fire rate and bullet damage
    public void SetFireRate(float newFireRate)
    {
        fireRate = newFireRate;
    }

    public void SetBulletDamage(int newDamage)
    {
        bulletDamage = newDamage;
    }

    Transform FindNearestPolice(GameObject[] policeCars)
    {
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject police in policeCars)
        {
            Transform policeTransform = police.transform;
            float distance = Vector2.Distance(player.position, policeTransform.position);

            if (distance < minDistance && distance <= maxRange)
            {
                minDistance = distance;
                nearest = policeTransform;
            }
        }

        return nearest;
    }
    void RotateTowardsTarget(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)gunArm.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Adjust the angle by 90 degrees
        //  float correctedAngle = -angle;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);
        gunArm.rotation = Quaternion.Lerp(gunArm.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        Debug.DrawLine(gunArm.position, targetPosition, Color.red);
    }
    void OnDrawGizmos()
    {
        // Visualize the range in the Editor
        if (Application.isEditor)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, maxRange);
        }
    }
}