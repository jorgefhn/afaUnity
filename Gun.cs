using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Gun : MonoBehaviour
{


    public Transform bulletSpawnPoint;
    public GameObject target; 

    public float gunRange = 200;
    public float laserDuration = 1f;

    LineRenderer  laserLine;
    float fireTimer;

    void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
    }

    void Update(){
        fireTimer += Time.deltaTime;

        
            fireTimer = 0;
            StartCoroutine(ShootLaser());

            // var bullet = Instantiate(bulletPrefab,bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            // bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;

        
       

        IEnumerator ShootLaser()
        {
            laserLine.SetPosition(0,bulletSpawnPoint.position);
            RaycastHit hit;

            if (Physics.Raycast(bulletSpawnPoint.transform.position,target.transform.position,out hit,gunRange))
            {
                laserLine.SetPosition(1,hit.point);
                if (hit.transform.gameObject.name == "drone1" || hit.transform.gameObject.name == "drone2")
                {
                    Debug.Log(transform.gameObject.name + " hit "+hit.transform.gameObject.name);
                    /*int health = hit.transform.gameObject.GetComponent("health");
                    health -= 20;
                    hit.transform.gameObject.GetComponent("health") = health;
                    */
                }
            }

            else
            {
                laserLine.SetPosition(1, target.transform.position);
            }

            laserLine.enabled = true;
            yield return new WaitForSeconds(laserDuration);
            laserLine.enabled = false;

        }
    }
}
