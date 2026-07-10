using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Musik")]
    [SerializeField] private AudioClip titleMusic;
    [SerializeField] private AudioClip gameMusic;

    private AudioSource audioSource;

    private void Awake()
    {
        // Verhindert mehrere MusicManager beim Szenenwechsel
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    public void PlayTitleMusic()
    {
        PlayMusic(titleMusic);
    }

    public void PlayGameMusic()
    {
        PlayMusic(gameMusic);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Es wurde kein AudioClip zugewiesen.");
            return;
        }

        // Musik nicht neu starten, wenn sie bereits läuft
        if (audioSource.clip == clip && audioSource.isPlaying)
        {
            return;
        }

        audioSource.clip = clip;
        audioSource.Play();
    }
}