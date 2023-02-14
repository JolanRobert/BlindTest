using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour {
    
    public static GameUI Instance;
    
    [Header("GameObjects")]
    [SerializeField] private GameObject countdown;
    [SerializeField] private GameObject result;
    
    [Header("Elements")]
    [SerializeField] private TMP_Text songIndex;
    [SerializeField] private TMP_Text timeLeft;
    [SerializeField] private TMP_Text authorText;
    [SerializeField] private TMP_Text volumePercent;
    [SerializeField] private TMP_Text showScoreText;
    [SerializeField] private TMP_Text nextSongText;

    [Header("Prefabs")]
    private List<PlayerScore> playerScores = new List<PlayerScore>();
    [SerializeField] private GameObject playerScorePrefab;
    [SerializeField] private Transform playerScoreParent;
    
    void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start() {
        foreach (PlayerManager.Player player in PlayerManager.Instance.players) {
            PlayerScore playerScore = Instantiate(playerScorePrefab, playerScoreParent).GetComponent<PlayerScore>();
            playerScore.Init(player);
            playerScores.Add(playerScore);
        }
    }
    
    public void SetSongIndex(int index, int total) {
        songIndex.text = index + " / " + total;
        if (index == total) nextSongText.text = "Results";
    }

    public void SetTimeLeft(int time) {
        if (time == 65) timeLeft.text = "∞";
        else timeLeft.text = ""+time;
    }

    public void SetAuthor(string author) {
        authorText.text = author;
    }

    public void UpdateVolume(float value) {
        volumePercent.text = value + "%";
        AudioManager.Instance.UpdateVolume(value/100);
    }

    public void OnClickReveal() {
        result.SetActive(true);
        countdown.SetActive(false);
        AudioManager.Instance.ForceEndSong();
    }

    public void OnClickShowScores() {
        if (showScoreText.text.StartsWith("Show")) {
            foreach (PlayerScore ps in playerScores) ps.ShowScore();
            showScoreText.text = "Hide Scores";
        }
        else {
            foreach (PlayerScore ps in playerScores) ps.HideScore();
            showScoreText.text = "Show Scores";
        }
    }

    public void OnClickNext() {
        StartCoroutine(NextSong());
    }

    private IEnumerator NextSong() {
        foreach (PlayerScore ps in playerScores) ps.AddScore();

        if (AudioManager.Instance.songIndex != AudioManager.Instance.songList.Count) {
            yield return StartCoroutine(AudioManager.Instance.LoadSong());
            AudioManager.Instance.PlayNextSong();
            result.SetActive(false);
            countdown.SetActive(true);
        }
        else {
            BTManager.Instance.LoadScene("Result");
        }
    }
}