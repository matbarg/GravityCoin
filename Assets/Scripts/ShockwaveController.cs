using UnityEngine;
using System.Collections;

public class ShockwaveController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer shockwaveRenderer;
    [SerializeField] private float duration = 0.8f;
    [SerializeField] private float maxRadius = 0.2f;

    private Material shockwaveMaterial;
    private Coroutine currentShockwave;

    private void Awake()
    {
        if (shockwaveRenderer == null)
        {
            shockwaveRenderer = GetComponent<SpriteRenderer>();
        }

        // Eigene Materialinstanz für diese Shockwave
        shockwaveMaterial = shockwaveRenderer.material;

        // Am Anfang nicht sichtbar
        shockwaveRenderer.enabled = false;
    }

    public void PlayShockwave(Vector2 worldPosition, float worldRadius)
    {
        if (currentShockwave != null)
            StopCoroutine(currentShockwave);

        currentShockwave = StartCoroutine(
            ShockwaveRoutine(worldPosition, worldRadius)
        );
    }

    private IEnumerator ShockwaveRoutine(Vector2 worldPosition, float worldRadius)
    {
        Vector3 localPosition =
            shockwaveRenderer.transform.InverseTransformPoint(worldPosition);

        Bounds spriteBounds = shockwaveRenderer.sprite.bounds;

        float uvX = Mathf.InverseLerp(
            spriteBounds.min.x,
            spriteBounds.max.x,
            localPosition.x
        );

        float uvY = Mathf.InverseLerp(
            spriteBounds.min.y,
            spriteBounds.max.y,
            localPosition.y
        );

        shockwaveMaterial.SetVector(
            "_RingSpawnPosition",
            new Vector2(uvX, uvY)
        );

        // tatsächliche Größe des Backgrounds in der Welt
        float backgroundWorldHeight = shockwaveRenderer.bounds.size.y;

        // World Units -> UV Radius
        float maxProgress = worldRadius / backgroundWorldHeight;

        Debug.Log(
            "Shockwave Radius World: " + worldRadius +
            " | Background Height: " + backgroundWorldHeight +
            " | Shader Progress: " + maxProgress
        );

        shockwaveRenderer.enabled = true;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);

            float progress = Mathf.Lerp(
                0f,
                maxProgress,
                t
            );

            shockwaveMaterial.SetFloat("_Progress", progress);

            yield return null;
        }

        shockwaveRenderer.enabled = false;

        currentShockwave = null;
    }
}