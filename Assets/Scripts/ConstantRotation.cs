using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0f, 90f, 0f);
    public float speedMultiplier = 1f;

    void Update()
    {
        transform.Rotate(rotationSpeed * speedMultiplier * Time.deltaTime, Space.World);
    }
}
