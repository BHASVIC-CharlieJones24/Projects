using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Grenade : MonoBehaviour
{
    public float explodeTime;
    public GameObject smokeEffect;
    private SoundOutput soundOutput;
    public float explodeVolume;
    private GameObject vfx;

    public float range = 10f;
    public int circleRayCount = 60;
    public string enemyTag = "Enemy";
    public string wallTag = "Wall";

    private List<GameObject> objectsInRange = new List<GameObject>();
    private List<int> damageForObjects = new List<int>();

    void Start()
    {
        soundOutput = GetComponent<SoundOutput>();
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        //Delay after being thrown before explosion
        yield return new WaitForSeconds(explodeTime);

        //Spawn explosion VFX
        vfx = Instantiate(smokeEffect, transform.position, smokeEffect.transform.rotation);
        //Output sound rays
        soundOutput.OutputSound(explodeVolume);

        for (int i = 0; i < circleRayCount; i++)
        {
            //clcualte angle of current ray
            float angle = (360f / circleRayCount) * i;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * transform.forward;

            //Shoot a ray
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, range))
            {
                //Check if it his a player or an enemy
                if (hit.collider.CompareTag(enemyTag) || hit.collider.CompareTag("Player"))
                {
                    GameObject obj = hit.collider.gameObject;
                    if (!objectsInRange.Contains(obj))
                    {
                        objectsInRange.Add(obj);
                        //Calculate damage based on distance
                        damageForObjects.Add((int)(50 * (1 - (hit.distance / range))));
                    }
                }
                else
                {
                    Debug.DrawRay(transform.position, dir * hit.distance, Color.red);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, dir * range, Color.white);
            }
        }

        //Apply damage
        for (int x = 0; x < objectsInRange.Count; x++)
        {
            if (objectsInRange[x].CompareTag(enemyTag))
            {
                objectsInRange[x].GetComponent<Enemyhealth>().damage(damageForObjects[x]);
            }
            else if (objectsInRange[x].CompareTag("Player"))
            {
                objectsInRange[x].GetComponent<Health>().damage(damageForObjects[x]);
            }
        }

        yield return new WaitForSeconds(0.7f);
        //Delete VFX and the grenade object
        Destroy(vfx);
        Destroy(gameObject);
    }
}
