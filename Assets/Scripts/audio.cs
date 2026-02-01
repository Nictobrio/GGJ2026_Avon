using UnityEngine;

public class audio : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<AudioSource>().Play();
    }
}
