using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastEvent : TimelineTrigger
{

    [SerializeField] private bool raycastHit = false;
    public bool TriggerOnHit = false;


    public void Select(CameraRaycast camera) {
        raycastHit = true;
    }

    public void Deselect(CameraRaycast camera)
    {
        raycastHit = false;
    }

    public override bool Triggered() {
        return raycastHit;
    }
}
