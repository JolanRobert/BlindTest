using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance;
    
    private AudioSource mySource;
    
    public List<FileInfo> songList = new List<FileInfo>();
    [SerializeField] private Song currentSong;
    
    public int songIndex;
    public int songTime = 30;
    public float myVolume;

    void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        mySource = GetComponent<AudioSource>();
    }

    void Start() {
        mySource.volume = myVolume;
    }

    public void ClearSongs() {
        songList.Clear();
        BTManager.Instance.SetConfigAudio(false);
    }

    public void AddSongs(List<FileInfo> songs) {
        songList.AddRange(songs);
        ShuffleSongs();
        BTManager.Instance.SetConfigAudio(true);
    }
    
    private void ShuffleSongs() {
        for (int i = 0; i < songList.Count - 1; i++) {
            FileInfo tmp = songList[i];
            int rdm = Random.Range(i, songList.Count);
            songList[i] = songList[rdm];
            songList[rdm] = tmp;
        }
    }

    public void UpdateVolume(float volume) {
        myVolume = volume;
        mySource.DOFade(myVolume, 0.2f);
    }

    public void ModifySongTime(int amount) {
        songTime += amount;
        songTime = Mathf.Clamp(songTime, 5, 65);
        SetupUI.Instance.SetSongTimeText(songTime);
    }

    public void PlayNextSong() {
        StopAllCoroutines();
        
        mySource.clip = currentSong.audioClip;
        mySource.volume = 0;
        
        GameUI.Instance.SetAuthor(currentSong.author);
        GameUI.Instance.SetSongIndex(songIndex,songList.Count);

        mySource.Play();
        mySource.DOFade(myVolume, 1);
        if (songTime <= 60) StartCoroutine(RunSong(songTime));
        else GameUI.Instance.SetTimeLeft(songTime);
    }

    public IEnumerator LoadSong() {
        FileInfo fileInfo = songList[songIndex++];
        AudioType audioType = fileInfo.Extension == ".mp3" ? AudioType.MPEG : AudioType.OGGVORBIS;
            
        using var uwr = UnityWebRequestMultimedia.GetAudioClip("file://"+fileInfo.FullName, audioType);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError) {
            Debug.Log(uwr.error);
            yield break;
        }
            
        DownloadHandlerAudioClip dlHandler = (DownloadHandlerAudioClip)uwr.downloadHandler;
 
        if (dlHandler.isDone) {
            Destroy(currentSong.audioClip);
            currentSong = new Song(dlHandler.audioClip, fileInfo.Name);
        }
    }

    private IEnumerator RunSong(int timeLeft) {
        while (timeLeft >= 0) {
            GameUI.Instance.SetTimeLeft(timeLeft);
            yield return new WaitForSeconds(1);
            
            timeLeft--;
            if (timeLeft == 1) mySource.DOFade(0, 1);
        }
        
        mySource.Stop();
    }

    public void ForceEndSong() {
        StopAllCoroutines();
        StartCoroutine(EndRunSong());
    }

    private IEnumerator EndRunSong() {
        if (!mySource.isPlaying) yield break;

        GameUI.Instance.SetTimeLeft(0);
        mySource.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        
        mySource.Stop();
    }

    [Serializable]
    public class Song {
        public AudioClip audioClip;
        public string author;

        public Song(AudioClip audioClip, string author) {
            this.audioClip = audioClip;
            this.author = author;
        }
    }
}
