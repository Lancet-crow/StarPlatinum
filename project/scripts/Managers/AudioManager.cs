using AYellowpaper.SerializedCollections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic((int)environment);
    }

    private void OnDisable()
    {
        AudioMusic.Stop();
        AudioSounds.Stop();
        AudioMusic.clip = null;
        AudioSounds.clip = null;
    }

    public enum SoundType
    {
        breakBuilding,
        buildBuilding,
        closeMenu,
        openMenu
    }

    public enum MusicType
    {
        mainMenuMusic,
        ingameMusic
    }


    [SerializedDictionary]
    public SerializedDictionary<SoundType, AudioClip> soundClips = new();
    [SerializedDictionary]
    public SerializedDictionary<MusicType, AudioClip> musicClips = new();

    public AudioSource AudioSounds;
    public AudioSource AudioMusic;

    public enum Environment
    {
        mainMenu,
        inGame
    }

    [SerializeField]
    private Environment environment;

    public void PlaySound(SoundType soundType)
    {
        AudioSounds.PlayOneShot(soundClips[soundType]);
    }

    public void PlaySound(int soundTypeInt)
    {
        AudioSounds.PlayOneShot(soundClips[(SoundType)soundTypeInt]);
    }

    public void PlaySound(string soundTypeName)
    {
        AudioSounds.PlayOneShot(soundClips[(SoundType)System.Enum.Parse(typeof(SoundType), soundTypeName)]);
    }

    public void PlayMusic(MusicType musicType)
    {
        AudioMusic.clip = musicClips[musicType];
        AudioMusic.Play();
    }

    public void PlayMusic(int musicTypeInt)
    {
        AudioMusic.clip = musicClips[(MusicType)musicTypeInt];
        AudioMusic.Play();
    }

    public void PlayMusic(string musicTypeName)
    {
        AudioMusic.clip = musicClips[(MusicType)System.Enum.Parse(typeof(MusicType), musicTypeName)];
        AudioMusic.Play();
    }
}
