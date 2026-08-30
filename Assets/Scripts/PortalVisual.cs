using System.Collections;
using UnityEngine;

public class PortalVisual : MonoBehaviour
{
    public Transform portalGraphic;
    public float spawnDuration = 0.2f;

    void OnEnable()
    {
        StartCoroutine(SpawnPortal());
    }
    private Vector3 originalScale;

    void Awake()
    {
        originalScale = portalGraphic.localScale;
    }

    IEnumerator SpawnPortal()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / spawnDuration;

            portalGraphic.localScale =
                Vector3.Lerp(Vector3.zero, originalScale, t);

            yield return null;
        }

        portalGraphic.localScale = originalScale;
    }
}