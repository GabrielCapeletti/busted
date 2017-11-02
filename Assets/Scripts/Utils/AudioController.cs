using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    #region SINGLETON PATTERN
    public static AudioController _instance;
    public static AudioController Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<AudioController>();

                if (_instance == null) {
                    GameObject container = Instantiate((Resources.Load("AudioController") as GameObject));
                    _instance = container.GetComponent<AudioController>();
                }
            }

            return _instance;
        }
    }
    #endregion

    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private List<AudioSource> sfxSourcePool;

    private Dictionary<string, AudioClip> audioPool = new Dictionary<string, AudioClip>();
    private int sfxSourceIndex = 0;

    private bool loadedSound = false;


    private float volume  = 1;
    public float Volume {
        get { return this.volume; }
    }

    private PotaTween fadeMusicTween;

    void Awake() {
        DontDestroyOnLoad(this.transform.gameObject);
        this.fadeMusicTween = PotaTween.Create(this.musicSource.gameObject);

        this.fadeMusicTween.SetFloat(1f,0f);
        this.fadeMusicTween.SetEaseEquation(Ease.Equation.OutSine);
        this.fadeMusicTween.UpdateCallback(this.FadeMusic);

        this.Setup();
    }

    public void SetVolume(float _volume) {
        this.musicSource.volume = _volume * 0.5f;
        this.volume = _volume;
    }


    private void FadeMusic() {
        this.musicSource.volume = this.fadeMusicTween.Float.Value;
    }

    public void Setup() {
        foreach (var audio in Resources.LoadAll<AudioClip>("Audio/")) {
            this.RegisterSound(audio.name,audio);
        }

        this.loadedSound = true;
    }

    public void RegisterSound(string tag,AudioClip clip) {
        if (!this.audioPool.ContainsKey(tag)) {
            this.audioPool.Add(tag,clip);
        }
    }

    public void PlayMusic(string tag,bool loop = true,float volume = 1) {
        if (!this.loadedSound) this.Setup();

        if (this.audioPool.ContainsKey(tag)) {
            this.musicSource.clip = this.audioPool[tag];
            this.musicSource.loop = loop;
            this.musicSource.volume = volume * this.volume;
            this.musicSource.Play();
        }

    }

    public void StopMusic() {
        this.fadeMusicTween.Play();
    }

    public void PlaySFX(string tag,bool loop = false,float volume = 1) {
        if (!this.loadedSound) this.Setup();

        if (this.audioPool.ContainsKey(tag)) {
            this.sfxSourcePool[this.sfxSourceIndex].Stop();
            this.sfxSourcePool[this.sfxSourceIndex].clip = this.audioPool[tag];
            this.sfxSourcePool[this.sfxSourceIndex].loop = loop;
            this.sfxSourcePool[this.sfxSourceIndex].volume = volume * this.volume;
            this.sfxSourcePool[this.sfxSourceIndex].Play();
        }

        this.sfxSourceIndex++;

        if (this.sfxSourceIndex >= this.sfxSourcePool.Count) {
            this.sfxSourceIndex = 0;
        }
    }
}
