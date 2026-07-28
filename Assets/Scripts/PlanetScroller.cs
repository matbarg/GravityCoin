using UnityEngine;

public class PlanetScroller : MonoBehaviour
{
    [SerializeField] SpriteRenderer a;
    [SerializeField] SpriteRenderer b;
    [SerializeField] float secondsPerRotation = 100f;

    float loopWidth;
    Vector3 start;

    void Awake()
    {
        loopWidth = a.sprite.bounds.size.x;

        a.transform.localPosition = Vector3.zero;
        b.transform.localPosition = new Vector3(loopWidth, 0f, 0f);

        start = transform.localPosition;
    }

    void Update()
    {
        float speed = loopWidth / secondsPerRotation;
        transform.localPosition += Vector3.left * speed * Time.deltaTime;

        if (transform.localPosition.x <= start.x - loopWidth)
            transform.localPosition += Vector3.right * loopWidth;
    }
}