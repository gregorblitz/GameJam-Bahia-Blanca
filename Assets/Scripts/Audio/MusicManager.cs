using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] playlist;
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private bool loopPlaylist = true;

    private int currentSongIndex;
    private bool playlistActive;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        audioSource.loop = false;
    }

    private void Start()
    {
        if (playOnStart)
        {
            PlayPlaylist();
        }
    }

    private void Update()
    {
        if (!playlistActive || playlist == null || playlist.Length == 0 || audioSource.isPlaying)
        {
            return;
        }

        PlayNextSong();
    }

    public void PlayPlaylist()
    {
        if (playlist == null || playlist.Length == 0)
        {
            playlistActive = false;
            return;
        }

        currentSongIndex = 0;
        playlistActive = true;
        PlayCurrentSong();
    }

    public void StopPlaylist()
    {
        playlistActive = false;
        audioSource.Stop();
    }

    private void PlayNextSong()
    {
        PlayCurrentSong();
    }

    private void PlayCurrentSong()
    {
        for (int checkedSongs = 0; checkedSongs < playlist.Length; checkedSongs++)
        {
            if (currentSongIndex >= playlist.Length)
            {
                if (!loopPlaylist)
                {
                    playlistActive = false;
                    return;
                }

                currentSongIndex = 0;
            }

            AudioClip song = playlist[currentSongIndex];
            currentSongIndex++;

            if (song == null)
            {
                continue;
            }

            audioSource.clip = song;
            audioSource.Play();
            return;
        }

        playlistActive = false;
    }
}
