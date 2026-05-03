using UnityEditor;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
