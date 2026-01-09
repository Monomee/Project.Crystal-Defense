using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SpawnBullet(Random.Range(1,3)));
    }

    IEnumerator SpawnBullet(int interval)
    {
        yield return new WaitForSeconds(2);
        while (true)
        {
            Vector2 randomPoint = Random.insideUnitCircle.normalized * 15f;

            Vector3 spawnPos = (Vector3)randomPoint + Camera.main.transform.position;
            spawnPos.z = 0;

            GameObject bullet = BulletPooling.instance.GetPooledObject();

            if(bullet != null )
            {
                bullet.SetActive(true);
                bullet.GetComponent<Projectile>().Fire(spawnPos);
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
