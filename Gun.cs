using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Gun : MonoBehaviour
{


    public Transform bulletSpawnPoint;
    public GameObject target; 

    public float gunRange = 600;
    public float fireRate = 0.2f;
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

            Debug.Log(Physics.Raycast(bulletSpawnPoint.transform.position,bulletSpawnPoint.transform.forward,out hit,gunRange));
            if (Physics.Raycast(bulletSpawnPoint.transform.position,target.transform.position,out hit,gunRange))
            {
                laserLine.SetPosition(1,hit.point);
                Debug.Log("He dado a : "+hit.transform.gameObject.name);
                Destroy(hit.transform.gameObject);
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
