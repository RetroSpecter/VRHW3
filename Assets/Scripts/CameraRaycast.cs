using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{

    public LayerMask gazeInteractive;
    public GameObject selectedObject;

    // Update is called once per frame
    void Update() {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 100, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, gazeInteractive))
        {
            if (selectedObject != hit.transform.gameObject) {
                if(selectedObject != null)
                    selectedObject.SendMessage("Deselect", this, SendMessageOptions.DontRequireReceiver);
                hit.transform.SendMessage("Select", this, SendMessageOptions.DontRequireReceiver);

                print("I hit it");
                selectedObject = hit.transform.gameObject;

            }
        } else if (selectedObject != null){
            selectedObject.SendMessage("Deselect", this, SendMessageOptions.DontRequireReceiver);
            selectedObject = null;
        }
    }
}
