using System;
using UnityEngine;

public class SoundOutput : MonoBehaviour
{
    public String enemyTag;
    public LayerMask enemyLayer;
    public float RayCount;

    public void OutputSound(float volume)
    {
        for (int i = 0; i < RayCount; i++)
        {
            // Calculate angle for current ray
            float angle = -360 / 2f + (360 / (RayCount - 1)) * i;

            // Rotate the forward vector by the calculated angle
            Vector3 dir = Quaternion.Euler(0, angle, 0) * transform.forward;

            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, volume, enemyLayer))
            {
                // Hit enemy
                if (hit.collider.CompareTag(enemyTag))
                {
                    Debug.DrawRay(transform.position, dir * hit.distance, Color.yellow);
                    GameObject enemy = hit.collider.gameObject;
                    //Use the enemys setter to tell it that its heard someting
                    enemy.GetComponent<EnemyController>().SetSoundHeard(true);
                    enemy.GetComponent<EnemyController>().SetSoundPosition(transform.position);
                }
            }
            // Hit nothing
            else
            {
                Debug.DrawRay(transform.position, dir * volume, Color.white);
            }
        }
    }
}
