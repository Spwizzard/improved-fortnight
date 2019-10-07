using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* The sound manager script is based off of: 
 * https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/audio-and-sound-manager
 * The script should continued to be built upon as functions that are needed are discovered.
 */

/*
 * A note from Jacob - I need to clean up this overall script and I believe I should
 * seperate it into a SFX Manager and a Music Manager. Primarily due to the odd decision that the manager just pulls
 * the audio source off from itself. So in the inspector its confusing to see the same audio source name when it isn't.
 */
public class SoundManager : MonoBehaviour
{

    // Allow other scripts to call upon this one. Primarily how we send sounds to the sound manager.
    public static SoundManager instance;

    public AudioSource sfxSource;
    public AudioSource musicSource;

    //Used for instantiates. It will reference a prefab with a game object with an empty audio source.
    public GameObject prefabSFX;

    // Will be used in the randomize section of script to further change the sfx.
    public float lowPitchRange = 0.9f;
    public float highPitchRange = 1.1f;

    //Used to manage the music playing on overall scene
    public AudioClip titleTheme, gameTheme;
    private Scene currentScene;

    // Sets the instance and allows for it to exist betweens scenes without being destroyed.
    void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
        }

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }


    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();

        if (currentScene.buildIndex == 0)
        {
            SetMusic(titleTheme);
        }

        else if (currentScene.buildIndex == 1)
        {
            SetMusic(gameTheme);
        }
    }

    void Update()
    {
         if (currentScene != SceneManager.GetActiveScene())
        {
            currentScene = SceneManager.GetActiveScene();

            if (currentScene.buildIndex == 0)
            {
                SetMusic(titleTheme);
            }

            else if (currentScene.buildIndex == 1)
            {
                SetMusic(gameTheme);
            }
        }
    }

    //Public utility methods

    public void PlaySingle(AudioClip clip)
    {

        if (sfxSource.pitch != 1f)
        {
            sfxSource.pitch = 1f;
        }

        sfxSource.clip = clip;
        // PlayOneShot should allow the audio source to play multiple sounds without clipping. It basically creates an instance with the settings.
        sfxSource.PlayOneShot(sfxSource.clip);
    }

    //Needs to be tested still
    public void PlaySingleAtLocation(AudioClip clip, Transform audioLocation)
    {

        sfxSource.clip = clip;
        AudioSource.PlayClipAtPoint(clip, audioLocation.position);
    }

    //Will instatiate an object with a audio source based on desired base settings and then edit the pitch randomly
    public void RandomizeSfx(params AudioClip[] clips)
    {

        GameObject clone = Instantiate(prefabSFX);
        AudioSource cloneSFX = clone.GetComponent<AudioSource>();

        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        cloneSFX.pitch = randomPitch;
        cloneSFX.clip = clips[randomIndex];

        cloneSFX.PlayOneShot(cloneSFX.clip);
        Destroy(clone, cloneSFX.clip.length);
    }

    // Play overarching music (not music jingles, use play single for that). Based off level/menu.
    public void SetMusic(AudioClip clip)
    {

        StartCoroutine(FadeOutFadeIn(musicSource, 1.25f, clip));
    }

    // Couritines
    // Reference: https://forum.unity.com/threads/fade-out-audio-source.335031/

    public IEnumerator FadeOutFadeIn(AudioSource audioSource, float fadeTime, AudioClip clip)
    {

        float startVolume = audioSource.volume;

        //Fade Out
        {
            while (audioSource.volume > 0)
            {
                //Lowers volume at this rate, will still work while game is "paused"
                audioSource.volume -= Time.unscaledDeltaTime / fadeTime;

                //Waits for the next frame
                yield return null;
            }

            // audioSource.Stop();
            audioSource.volume = 0;
            audioSource.Stop();
        }

        //Swap out clip
        audioSource.clip = clip;

        //Fade In
        {
            audioSource.Play();

            while (audioSource.volume < startVolume)
            {
                audioSource.volume += Time.unscaledDeltaTime / fadeTime;

                //Waits for the next frame
                yield return null;
            }

            audioSource.volume = startVolume;
            yield break;
        }
    }

    //Currently not being used, but exist in case
    public IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.unscaledDeltaTime / fadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = 0;
    }

    public IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
    {
        float startVolume = 0.2f;

        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < 1.0f)
        {
            audioSource.volume += startVolume * Time.unscaledDeltaTime / fadeTime;

            yield return null;
        }

        audioSource.volume = 1f;
    }

}

