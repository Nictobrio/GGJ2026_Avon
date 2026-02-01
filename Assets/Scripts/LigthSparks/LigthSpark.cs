using UnityEngine;

public class LigthSpark : MonoBehaviour
{
    public Light fireLight;
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.3f;
    public float flickerSpeed = 5f;

    float noiseOffset;

    void Start()
    {
        if (!fireLight)
            fireLight = GetComponent<Light>();

        noiseOffset = Random.value * 100f;
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(noiseOffset, Time.time * flickerSpeed);
        fireLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);

        transform.localPosition += Vector3.up * (noise - 0.5f) * 0.01f;
        
    }
}
