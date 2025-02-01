using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] public AudioClip _banger;

    private AudioSource _source;

    private void Start()
    {
        MusicManager[] musicManagers = FindObjectsOfType<MusicManager>();
        if (musicManagers.Length > 1)
        {
            Destroy(gameObject);
        }

        _source = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
        StartCoroutine(SkipTrack());
    }

    IEnumerator SkipTrack()
    {
        yield return new WaitWhile(()=> _source.isPlaying);

        _source.clip = _banger;
        _source.Play();
        _source.loop = true;
    }
}
