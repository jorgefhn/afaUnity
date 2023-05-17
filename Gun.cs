using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Gun : MonoBehaviour
{


    public Transform bulletSpawnPoint;
    public float gunRange = 50f;
    public float fireRate = 0.2f;
    public float laserDuration = 0.05f;

    LineRenderer  laserLine;
    float fireTimer;

    public GameObject bulletPrefab;
    public float bulletSpeed = 10;


    void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
    }

    void Update(){
        fireTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && fireTimer > fireRate)
        {
            fireTimer = 0;
            laserLine.SetPosition(0,bulletSpawnPoint.position);
            RaycastHit hit;
            if (Physics.Raycast(bulletSpawnPoint.transform.position,
            bulletSpawnPoint.transform.forward,
            out hit,
            gunRange))
            {
                laserLine.SetPosition(1,hit.point);

                // Destroy(hit.transform.gameObject);
            }

            else
            {
                laserLine.SetPosition(1, bulletSpawnPoint.transform.position + (bulletSpawnPoint.transform.forward*gunRange));
            }
            StartCoroutine(ShootLaser());

            // var bullet = Instantiate(bulletPrefab,bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            // bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;

        
        }

        IEnumerator ShootLaser()
        {
            laserLine.enabled = true;
            yield return new WaitForSeconds(laserDuration);
            laserLine.enabled = false;

        }
    }
}
