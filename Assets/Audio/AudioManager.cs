using UnityEngine.Audio;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class AudioManager : MonoBehaviour
{

    public Helper Helper;
    public List<AudioClip> Clips = new List<AudioClip>();

    [SerializeField]
    public static AudioManager instance;
    public AudioSource MusicSource;
    public AudioSource EffectsSource;


    public GameManager gameManager;

    void Awake()
    {
        Helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();

        gameManager = Helper.GameManager;

        // Stops duplicate audio managers being created
        // https://www.youtube.com/watch?v=6OT43pvUyfY @ around 13:00 or so explains pretty well

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        MusicSource = gameObject.GetComponent<AudioSource>();
        EffectsSource = gameObject.GetComponent<AudioSource>();
    }

    void Start()
    {
        LoadAudioFiles();
        PlayGameMusic();
    }

    void LoadAudioFiles()
    {
        var soundDirectory = Directory.GetFiles(@".\Assets\Resources\Sounds");
        var startPath = @"\Assets\Resources\";

        var soundFileTypes = new List<string>() { "wav", "aiff", "mp3", "pcm" };

        foreach (var item in soundDirectory)
        {
            var filenameSections = item.ToString().Split('.');

            if (filenameSections.Length == 3 && soundFileTypes.Contains(filenameSections[2]))
            {
                var path = filenameSections[1].Replace(startPath, "");
                var clip = Resources.Load<AudioClip>(path);
                Clips.Add(clip);
            }
        }
    }

    public void PlayGameMusic()
    {
        var currentLevel = gameManager.currentGameLevel;
        var trackToPlay = "";
        switch (currentLevel)
        {
            case 1: trackToPlay = "GameMusic2"; break;
            case 2: trackToPlay = "GameMusic2"; break;
            case 3: trackToPlay = "GameMusic3"; break;
            default: trackToPlay = "GameMusic"; break;
        }
        AudioClip clip = Clips.Where(c => c.name == trackToPlay).First<AudioClip>();
        if (clip != null)
        {
            MusicSource.clip = clip;
            MusicSource.loop = true;
            MusicSource.Play();
        }
        else
        {
            Debug.LogError($"Cannot find sound {name} in Resources\\Sounds folder");
        }
    }

    public void PlayAudioClip(string name)
    {
        AudioClip clip = Clips.Where(c => c.name == name).First<AudioClip>();
        if (clip != null)
        {
            EffectsSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError($"Cannot find sound {name} in Resources\\Sounds folder");
        }
    }

    public void PlayAudioClip(AudioClip clip)
    {
        EffectsSource.PlayOneShot(clip);
    }

    public void ReverseGameMusic(bool r)
    {
        if (r)
        {
            MusicSource.pitch = -1;
        }

        if (!r)
        {
            MusicSource.pitch = 1;
        }
    }
}
