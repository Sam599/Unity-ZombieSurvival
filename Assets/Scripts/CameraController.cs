using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    Vector3 cameraPosition;
    GameObject objectHit;

    void Start()
    {
        cameraPosition = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPosition + player.transform.position;
        UnblockView();
    }

    void UnblockView()
    {
        Ray pointToObject = Camera.main.ScreenPointToRay(player.transform.position);
        RaycastHit cameraRayImpact;
        Debug.DrawRay(pointToObject.origin, pointToObject.direction * 10, Color.red);

        if (Physics.Raycast(pointToObject, out cameraRayImpact, 20)) {
            if (cameraRayImpact.collider.tag == "Transparent" && objectHit == null) {
                objectHit = cameraRayImpact.collider.transform.parent.gameObject;
                objectHit.transform.GetChild(0).gameObject.SetActive(false);
                objectHit.transform.GetChild(1).gameObject.SetActive(true);
                //Debug.Log("Unblocking view!");
            } else if (objectHit != null && cameraRayImpact.collider.tag != "Transparent") {
                objectHit.transform.GetChild(1).gameObject.SetActive(false);
                objectHit.transform.GetChild(0).gameObject.SetActive(true);
                objectHit = null;
                //Debug.Log("Returning Object!");
            }
        }
    }
}
