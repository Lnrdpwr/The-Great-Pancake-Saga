using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] public AudioClip _defaultTrack;

    private AudioSource _source;
    private float _defaultVolume;

    private void Start()
    {
        MusicManager[] musicManagers = FindObjectsOfType<MusicManager>();
        if (musicManagers.Length > 1)
        {
            Destroy(gameObject);
        }

        _source = GetComponent<AudioSource>();
        _defaultVolume = _source.volume;
        DontDestroyOnLoad(gameObject);
    }

    public void Transition(AudioClip track)
    {
        if (track == null)
            track = _defaultTrack;

        StartCoroutine(TransitionRoutine(track));
    }

    IEnumerator TransitionRoutine(AudioClip track)
    {
        for(float i = 0; i < 1; i += Time.deltaTime)
        {
            _source.volume = Mathf.Lerp(1, 0, i);
            yield return new WaitForEndOfFrame();
        }

        _source.clip = track;
        _source.Play();

        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            _source.volume = i;
            yield return new WaitForEndOfFrame();
        }

        _source.volume = _defaultVolume;
    }
}
