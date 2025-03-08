using UnityEngine;
using UnityEngine.Audio;

// This class is responsible for finding and assigning the audio mixer to the AudioSettings singleton
public class AudioSettingsConfigurator : MonoBehaviour
{
    // Assign your audio mixer in the inspector
    [SerializeField] private AudioMixer gameAudioMixer;

    private void Awake()
    {
        // Get the AudioSettings singleton instance
        AudioSettings audioSettings = AudioSettings.Instance;
        
        // Assign the audio mixer
        if (audioSettings != null && gameAudioMixer != null)
        {
            audioSettings.audioMixer = gameAudioMixer;
            Debug.Log("Audio mixer assigned to AudioSettings successfully");
        }
        else
        {
            Debug.LogError("Failed to assign audio mixer to AudioSettings");
        }
    }
}
