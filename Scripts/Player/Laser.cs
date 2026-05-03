using UnityEngine;

public class Laser : MonoBehaviour
{
    public Transform startPoint;
    public float maxDistance = 50f; 
    private LineRenderer line;    

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
    }

    void Update()
    {
        line.SetPosition(0, startPoint.position);

        //Shoot a ray
        if (Physics.Raycast(startPoint.position, startPoint.forward, out RaycastHit hit, maxDistance))
        {
            //Hit
            line.SetPosition(1, hit.point);
        }
        else
        {
            //Hit nothing
            line.SetPosition(1, startPoint.position + startPoint.forward * maxDistance);
        }
    }
}
