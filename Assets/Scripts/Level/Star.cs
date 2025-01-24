using UnityEngine;

public class Star : MonoBehaviour
{
    private AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        _source.Play();
    }
}
