using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Sound Effect")]
public class SoundEffectData : ScriptableObject
{
    public AudioClip clip;
    public float volume;
    public float pitch = 1;
    public float time;

    public AudioSource Play(AudioSource audioSourceParam = null)
    {

        var source = audioSourceParam;
        if (source == null)
        {
            var _obj = new GameObject("Sound", typeof(AudioSource));
            source = _obj.GetComponent<AudioSource>();
        }

        // set source config:
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.time = time;
        source.Play();

        Destroy(source.gameObject, source.clip.length / source.pitch);

        return source;
    }
}
