using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRGazeEvent : TimelineTrigger
{
    public float visionRadius = 45;
    public bool IsInGaze = true;
    private Camera cam;

    // Start is called before the first frame update
    void Start() {
        cam = Camera.main;
    }

    /*
    // Update is called once per frame
    void Update() {
        Vector3 angleToCam = transform.position - cam.transform.position;
        print(Vector3.Angle(cam.transform.forward, angleToCam) < visionRadius);
    }
    */

    public override bool Triggered() {
        Vector3 angleToCam = transform.position - cam.transform.position;
        return Vector3.Angle(cam.transform.forward, angleToCam) < visionRadius;
    }

    void OnDrawGizmosSelected() {
        if (cam == null) {
            if (Camera.main) {
                return;
            }
            cam = Camera.main;
        }
        float distance = Vector3.Distance(cam.transform.position, transform.position);
        DebugExtension.DrawCone(cam.transform.position, cam.transform.forward * distance, Color.red, visionRadius);
    }

}
