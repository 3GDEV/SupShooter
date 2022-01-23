using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeVolume : MonoBehaviour
{

    public Slider volumeSlider;

    public AudioSource musicAudio;
    
    static ChangeVolume instance;

    // Start is called before the first frame update
    void Start()
    {
        musicAudio = GetComponent<AudioSource>();
        volumeSlider.value = PlayerPrefs.GetFloat("VolumeSlider");   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VolumePrefs()
    {
        PlayerPrefs.SetFloat("VolumeSlider", musicAudio.volume);
        musicAudio.volume = volumeSlider.value;
    }
}
