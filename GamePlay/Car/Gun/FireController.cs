// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class FireController : MonoBehaviour
// {
//     [SerializeField] private GameObject bulletPrefab;  // Assign your bullet prefab
//     [SerializeField] private float fireRate = 0.5f;    // Fire rate in seconds
//     [SerializeField] private float bulletSpeed = 10f;  // Speed of the bullet
//     [SerializeField] private int bulletDamage = 10;    // Damage per bullet

//     [SerializeField] private Transform gunArm;                            // The rotating arm of the gun
//     private Transform nearestPolice;                     // Closest police car
//     private float nextFire = 0f;                         // Track when the next bullet can be fired
//     private bool isFiring = false;                      // Track if the fire button is pressed

//     // public void Initialize(Transform gunArm, Transform nearestPolice)
//     // {
//     //     this.gunArm = gunArm;
//     //     this.nearestPolice = nearestPolice;
//     // }

//     void Update()
//     {
//         // Check if the fire button is pressed and it's time to fire
//         if (isFiring && Time.time >= nextFire && nearestPolice != null)
//         {
//             // Fire a bullet
//             FireBullet();

//             // Update the next fire time based on fire rate
//             nextFire = Time.time + fireRate;
//         }
//     }

//     void FireBullet()
//     {
//         // Instantiate the bullet at the tip of the gun arm
//         GameObject bullet = Instantiate(bulletPrefab, gunArm.position, gunArm.rotation);

//         // Get the Bullet script and set its properties
//         Bullet bulletScript = bullet.GetComponent<Bullet>();
//         if (bulletScript != null)
//         {
//             bulletScript.speed = bulletSpeed;
//             bulletScript.damage = bulletDamage;
//             bulletScript.target = nearestPolice.position;
//         }

//         // Optional: Add a debug line to show the bullet's path
//         Debug.DrawLine(gunArm.position, nearestPolice.position, Color.red, 0.1f);
//     }

//     public void OnFireButtonDown()
//     {
//         isFiring = true;
//     }

//     public void OnFireButtonUp()
//     {
//         isFiring = false;
//     }

//     // Call this method to adjust the fire rate and bullet damage
//     public void SetFireRate(float newFireRate)
//     {
//         fireRate = newFireRate;
//     }

//     public void SetBulletDamage(int newDamage)
//     {
//         bulletDamage = newDamage;
//     }
// }