using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    private const string MusicVolumeKey = "MusicVolume";
    private const string MixerParameter = "MusicVolume";

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.8f);

        musicSlider.SetValueWithoutNotify(savedVolume);
        SetMusicVolume(savedVolume);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    private void OnDestroy()
    {
        musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
    }

    public void SetMusicVolume(float linearVolume)
    {
        if (linearVolume <= 0.0001f)
        {
            audioMixer.SetFloat(MixerParameter, -80f);
        }
        else
        {
            float volumeInDecibels = Mathf.Log10(linearVolume) * 20f;
            audioMixer.SetFloat(MixerParameter, volumeInDecibels);
        }

        PlayerPrefs.SetFloat(MusicVolumeKey, linearVolume);
    }
}