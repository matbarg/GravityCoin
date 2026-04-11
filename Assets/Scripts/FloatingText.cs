using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float amplitude = 7f;
    public float frequency = 2f;

    private Vector3 startPos;
    private float phaseOffset;

    void Start()
    {
        startPos = transform.localPosition;
        
        phaseOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * frequency + phaseOffset) * amplitude;
        transform.localPosition = startPos + new Vector3(0, offsetY, 0);
    }
}