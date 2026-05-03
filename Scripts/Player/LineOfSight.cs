using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public float rayLength;
    public float circleRayLength;
    public string enemyTag = "Enemy";
    public string wallTag = "Wall";
    public int rayCount;
    public GameObject eyes;

    public int circleRayCount;
    public float visionAngle;
    private List<GameObject> previousHits = new List<GameObject>();

    void Update()
    {
        List<GameObject> currentHits = new List<GameObject>();

        //Vision Cone
        for (int i = 0; i < rayCount; i++)
        {
            //Calculate angle for the current ray
            float angle = -visionAngle / 2f + (visionAngle / (rayCount - 1)) * i;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * transform.forward;

            //Shoot a ray
            if (Physics.Raycast(eyes.transform.position, dir, out RaycastHit hit, rayLength))
            {
                if (hit.collider.CompareTag(enemyTag))
                {
                    Debug.DrawRay(eyes.transform.position, dir * hit.distance, Color.red);
                    GameObject enemy = hit.collider.gameObject;
                    currentHits.Add(enemy);

                    MeshRenderer[] enemyMeshRenderers = enemy.GetComponentsInChildren<MeshRenderer>();
                    SkinnedMeshRenderer[] enemySkinnedMeshRenderer = enemy.GetComponentsInChildren<SkinnedMeshRenderer>();
                    //Mesh renderers
                    for (int x = 0; x < enemyMeshRenderers.Length; x++)
                    {
                        if (enemyMeshRenderers[x] != null)
                        {
                            enemyMeshRenderers[x].enabled = true;
                        }

                    }

                    //Skinned mesh renderers
                    for (int x = 0; x < enemySkinnedMeshRenderer.Length; x++)
                    {
                        if (enemySkinnedMeshRenderer[x] != null)
                        {
                            enemySkinnedMeshRenderer[x].enabled = true;
                        }
                    }
                }
                else if (hit.collider.CompareTag(wallTag))
                {
                    Debug.DrawRay(eyes.transform.position, dir * hit.distance, Color.green);
                }
            }
            else
            {
                Debug.DrawRay(eyes.transform.position, dir * rayLength, Color.white);
            }
        }

        //Circle Rays
        for (int i = 0; i < circleRayCount; i++)
        {
            //Calculate the angle for the current ray
            float angle = (360f / circleRayCount) * i;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * transform.forward;

            //Shoot a ray
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, circleRayLength))
            {
                //If the collision object is an enemy
                if (hit.collider.CompareTag(enemyTag))
                {
                    Debug.DrawRay(transform.position, dir * hit.distance, Color.red);
                    GameObject enemy = hit.collider.gameObject;
                    currentHits.Add(enemy);

                    MeshRenderer[] enemyMeshRenderers = enemy.GetComponentsInChildren<MeshRenderer>();
                    SkinnedMeshRenderer[] enemySkinnedMeshRenderer = enemy.GetComponentsInChildren<SkinnedMeshRenderer>();

                    //Mesh renderers
                    for (int x = 0; x < enemyMeshRenderers.Length; x++)
                    {
                        if (enemyMeshRenderers[x] != null)
                        {
                            enemyMeshRenderers[x].enabled = true;
                        }

                    }

                    //Skinned mesh renderers
                    for (int x = 0; x < enemySkinnedMeshRenderer.Length; x++)
                    {
                        if (enemySkinnedMeshRenderer[x] != null)
                        {
                            enemySkinnedMeshRenderer[x].enabled = true;  
                        }
                    }
                }
                else if (hit.collider.CompareTag(wallTag))
                {
                    Debug.DrawRay(transform.position, dir * hit.distance, Color.green);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, dir * circleRayLength, Color.white);
            }
        }

        //Disable enemies that are no longer in the vision cone
        for (int i = 0; i < previousHits.Count; i++)
        {
            GameObject enemy = previousHits[i];
            if (!currentHits.Contains(enemy) && enemy != null)
            {
                MeshRenderer[] enemyMeshRenderers = enemy.GetComponentsInChildren<MeshRenderer>();
                SkinnedMeshRenderer[] enemySkinnedMeshRenderer = enemy.GetComponentsInChildren<SkinnedMeshRenderer>();

                //Mesh renderers
                for (int x = 0; x < enemyMeshRenderers.Length; x++)
                {
                    if (enemyMeshRenderers[x] != null)
                    {
                        enemyMeshRenderers[x].enabled = false; 
                    }
                }
                //Skinned mesh renderers
                for (int x = 0; x < enemySkinnedMeshRenderer.Length; x++)
                {
                    if (enemySkinnedMeshRenderer[x] != null)
                    {
                        enemySkinnedMeshRenderer[x].enabled = false;
                    }

                }
            }
        }

        previousHits = currentHits;
    }
}
