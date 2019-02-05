using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceEvent : TimelineTrigger
{

    public float distanceForEvent;
    public GameObject[] targetObject;

    public override bool Triggered()
    {

        foreach (GameObject go in targetObject)
            if (Vector3.Distance(transform.position, go.transform.position) < distanceForEvent)
            {
                print("hoi");
                return true;
            }

        return false;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanceForEvent);
    }
}
