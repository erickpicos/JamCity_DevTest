using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    
    [SerializeField] private AudioSource audioSourceSteps; 
    [SerializeField] private AudioSource audioSourceSounds;
    [SerializeField] private AudioSource audioSourceAmbient;
    [SerializeField] private Slider sliderMasterSound;
    [SerializeField] private Slider sliderSFXSound;
    [SerializeField] private Slider sliderAmbientSound;

    public void ChangeMasterSound()
    {
        ChangeSFXSound();
        ChangeAmbientSound();
    }
    public void ChangeSFXSound()
    {
        float value = sliderMasterSound.value * sliderSFXSound.value;
        audioSourceSteps.volume = value;
        audioSourceSounds.volume = value;
    }
    public void ChangeAmbientSound()
    {
        float value = sliderMasterSound.value * sliderAmbientSound.value;
        audioSourceAmbient.volume = value;
    }
    
}
