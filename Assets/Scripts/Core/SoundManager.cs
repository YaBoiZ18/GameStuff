using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance {  get; private set; }

    private AudioSource soundSource;
    private AudioSource musicSource;

    private void Awake()
    {        
        soundSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        //Keep this object even when we go to a new scene
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        // Destroy duplicate gameObjects
        else if (instance != null && instance!= this) 
        {
           Destroy(gameObject); 
        }

        //Assing initial volumes
        ChangeSoundVolume(0);
        ChangeMusicVolume(0);
        
    }

    public void PlaySound(AudioClip _sound)
    {
        soundSource.PlayOneShot(_sound);
    }

    public void ChangeSoundVolume(float _change) 
    {
        ChangeSourceVolume(1, "soundVolume", _change, soundSource);
    }

    public void ChangeMusicVolume(float _change)
    {
        ChangeSourceVolume(0.3f, "musicVolume", _change, musicSource);
    }

    private void ChangeSourceVolume(float baseVolume, string volumeName, float change, AudioSource source)
    {
        //Get initial value of volume and change it
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1); //Load last saved sound volume from player prefs
        currentVolume += change;

        // Check if we reached the maximum or minimum value
        if (currentVolume > 1)
        {
            currentVolume = 0;
        }
        else if (currentVolume < 0)
        {
            currentVolume = 1;
        }

        // Assing final value
        float finalVolume = currentVolume * baseVolume;
        source.volume = finalVolume;

        //Save final value from player prefs
        PlayerPrefs.SetFloat(volumeName, currentVolume);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
