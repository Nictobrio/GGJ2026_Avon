using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public float speed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        Vector3 direccion = (playerTransform.position - transform.position).normalized;
        transform.position += direccion * speed * Time.deltaTime;
    }
}
