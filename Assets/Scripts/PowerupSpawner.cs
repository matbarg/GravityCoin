using UnityEngine;
using System.Collections;

public class PowerupSpawner : MonoBehaviour
{
    public GameObject powerupPrefab;
    public Transform[] spawnPoints;

    [Header("Timing")]
    [Tooltip("Wie lange ein Powerup liegen bleibt, bevor es verschwindet.")]
    public float lifeTime = 10f;
    
    [Tooltip("Pause nach Aufheben/Verschwinden bis zum nächsten Spawn.")]
    public float respawnDelay = 6f;
    
    [Header("Start")]
    [Tooltip("Wie lange nach Rundenbeginn gewartet wird, bis das erste Powerup erscheint.")]
    public float initialDelay = 15f;

    private GameObject currentPowerup;
    private int lastSpawnIndex = -1;

    void Start()
    {
        StartCoroutine(InitialSpawn());
    }
    

    // Wird vom Powerup aufgerufen, wenn es eingesammelt wurde.
    public void OnPowerupCollected()
    {
        StopAllCoroutines();      // laufenden Lebens-Timer abbrechen
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnPowerup();
    }

    void SpawnPowerup()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        int randomIndex;
        if (spawnPoints.Length > 1)
        {
            do { randomIndex = Random.Range(0, spawnPoints.Length); }
            while (randomIndex == lastSpawnIndex);
        }
        else
        {
            randomIndex = 0;
        }

        lastSpawnIndex = randomIndex;
        Transform spawn = spawnPoints[randomIndex];

        currentPowerup = Instantiate(powerupPrefab, spawn.position, Quaternion.identity);

        Powerup pu = currentPowerup.GetComponent<Powerup>();
        if (pu != null)
        {
            pu.spawner = this;
        }

        // Lebens-Timer starten: verschwindet, wenn keiner es nimmt
        StartCoroutine(LifeTimer());

        Debug.Log("Powerup gespawnt an Punkt: " + randomIndex);
    }

    IEnumerator LifeTimer()
    {
        // Erst die "ruhige" Zeit warten (Lebenszeit minus Blink-Phase)
        float blinkDuration = 2.5f;
        float calmTime = lifeTime - blinkDuration;
        if (calmTime > 0f)
            yield return new WaitForSeconds(calmTime);

        // Jetzt die letzten 2 Sekunden: blinken
        SpriteRenderer sr = currentPowerup != null
            ? currentPowerup.GetComponentInChildren<SpriteRenderer>()
            : null;

        float blinkTimer = 0f;
        float blinkSpeed = 0.30f;   // wie schnell es blinkt

        while (blinkTimer < blinkDuration && currentPowerup != null)
        {
            if (sr != null)
                sr.enabled = !sr.enabled;   // an/aus schalten

            yield return new WaitForSeconds(blinkSpeed);
            blinkTimer += blinkSpeed;
        }

        // Zeit abgelaufen
        if (currentPowerup != null)
        {
            Destroy(currentPowerup);
            StartCoroutine(Respawn());
        }
    }
    IEnumerator InitialSpawn()
    {
        yield return new WaitForSeconds(initialDelay);
        SpawnPowerup();
    }
}