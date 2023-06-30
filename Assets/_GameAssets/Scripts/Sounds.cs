using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public static class Key
    {
        public static string music { get => nameof(music); }
        public static string click { get => nameof(click); }
        public static string dead { get => nameof(dead); }
        public static string lose { get => nameof(lose); }
        public static string run { get => nameof(run); }
        public static string win { get => nameof(win); }

    }

    [System.Serializable]
    private struct SoundUnit
    {
        public string key;
        public AudioSource audioSource; 
    }



    [SerializeField] private List<SoundUnit> _sounds;

    private static Dictionary<string, SoundUnit> sounds;

    private void Awake()
    {
        sounds = new Dictionary<string, SoundUnit>();
        foreach (var sound in _sounds) sounds.Add(sound.key, sound);
    }


    public static void Play(string soundKey) 
        => sounds[soundKey].audioSource.Play();

    public static void Stop(string soundKey)
        => sounds[soundKey].audioSource.Stop();


    public static void PlayOneShot(string soundKey)
    {
        var audioSource = sounds[soundKey].audioSource;
        audioSource.PlayOneShot(audioSource.clip, volumeScale: audioSource.volume);
    }
        


}
