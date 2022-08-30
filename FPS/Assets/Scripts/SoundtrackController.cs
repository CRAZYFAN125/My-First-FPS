using UnityEngine;
using UnityEngine.Audio;



public class SoundtrackController : MonoBehaviour
{
    [System.Serializable]
    public class AudioClipList
    {
        public AudioClip audioClip;
        public float ClipTime=15;
        public float Delay = 0f;
    }
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClipList[] audioClips;
    int nowPlaying;
    float timeReamining;


    // Use this for initialization
    void Start()
    {
        nowPlaying = 0;
        timeReamining = audioClips[nowPlaying].ClipTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeReamining <= 0-audioClips[nowPlaying].Delay)
        {
            if (nowPlaying+1<audioClips.Length)
            {
                nowPlaying++;
            }
            else
            {
                nowPlaying = 0;
            }
            timeReamining = audioClips[nowPlaying].ClipTime;

            audioSource.clip = audioClips[nowPlaying].audioClip;
            audioSource.Play();
            
        }

        timeReamining -= Time.deltaTime;
    }
}
