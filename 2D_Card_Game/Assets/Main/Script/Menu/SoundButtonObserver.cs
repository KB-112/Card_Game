using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SoundButtonObserver : IUIObserver
{
    private TextMeshProUGUI text;
    private AudioSource audioSource;

    public SoundButtonObserver(TextMeshProUGUI textComponent, AudioSource source)
    {
        text = textComponent;
        audioSource = source;
    }

    public void OnNotify(UIEventType eventType)
    {
        if (eventType == UIEventType.Sound)
        {
            audioSource.mute = !audioSource.mute;
            text.text = audioSource.mute ? "Sound Off" : "Sound On";
        }
    }
}
