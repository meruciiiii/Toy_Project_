using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider[] sliders;

    public void ControllVolume(string audioGroup)
    {
        switch (audioGroup)
        {
            case "BGM":
                audioMixer.SetFloat("BGM", sliders[0].value);
                break;

            case "SFX":
                audioMixer.SetFloat("SFX", sliders[1].value);
                break;
        }
    }
}
