using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class TeleporterSpawner : MonoBehaviour
{
    public GameObject portalA;
    public GameObject portalB;
    public BoxCollider2D portalSpawnArena;
    public LayerMask groundLayer;
    [SerializeField] private Vector2 portalCheckSize; 
    
    private Vector2 backupPortalSpawn = new Vector2(0f, -1.4f); 
    private Vector2 portalASpawn;
    private Vector2 portalBSpawn;

    private void Start()
    {
        
        StartCoroutine(RespawnPortals());
    }

    public void PortalUsed()
        {
            portalA.SetActive(false);
            portalB.SetActive(false);
            StartCoroutine(RespawnPortals());
        }

        IEnumerator RespawnPortals()
        {

            //yield on a new YieldInstruction that waits for 5 seconds.
            yield return new WaitForSeconds(5);
            
      
            SpawnPortal();


        }

        void SpawnPortal()
        {
            
            portalASpawn = FindFreeSpawnPosition();
            portalA.transform.position = portalASpawn;
            portalBSpawn = FindFreeSpawnPosition();
            portalB.transform.position = portalBSpawn;
            portalA.SetActive(true);
            portalB.SetActive(true);
            
           
        }

        Vector2 FindFreeSpawnPosition()
        {
            for (int i = 0; i < 20; i++)
            {
                float xRange = Random.Range(portalSpawnArena.bounds.min.x, portalSpawnArena.bounds.max.x);
                float yRange = Random.Range(portalSpawnArena.bounds.min.y, portalSpawnArena.bounds.max.y);
                Vector2 position = new Vector2(xRange, yRange);

                Collider2D hit = Physics2D.OverlapBox(
                    position,
                    portalCheckSize, 
                    0f,
                    groundLayer
                );
                if (hit == null)
                {
                    return position;
                }
            }
            Debug.LogWarning("no free position for the Portal");
            return backupPortalSpawn;

        }
}
