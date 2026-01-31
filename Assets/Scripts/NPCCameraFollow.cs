using UnityEngine;

public class NPCCameraFollow : MonoBehaviour
{
    

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 lookDir = Camera.main.transform.position -transform.position;
        lookDir.y = 0f;

        transform.rotation = Quaternion.LookRotation(-lookDir);
    }
}
