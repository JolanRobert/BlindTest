using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupUI : MonoBehaviour {
    
    public static SetupUI Instance;

    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private TMP_Text pathText;
    [SerializeField] private TMP_Text nbSongLoad;
    [SerializeField] private TMP_Text songTimeText;
    [SerializeField] private TMP_Text editPlayers;
    public Button start;

    [Header("Player Menu")]
    [SerializeField] private GameObject playerMenu;
    [SerializeField] private TMP_Text nbPlayers;
    [SerializeField] private Button confirmPlayers;
    
    void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    //
    // MAIN MENU
    //
    
    public void SetPath(string text) {
        pathText.text = "Path : "+text;
    }

    public void SetNbSongLoad(int nbSong) {
        nbSongLoad.text = nbSong + " tracks found";
    }

    public void SetSongTimeText(int songTime) {
        if (songTime == 65) songTimeText.text = "∞";
        else songTimeText.text = songTime + "s";
    }

    public void SetEditPlayerText(int nbPlayer) {
        editPlayers.text = "Edit Players (" + nbPlayer + ")";
    }
    
    public void OnClickEditPlayers() {
        mainMenu.SetActive(false);
        playerMenu.SetActive(true);
    }

    public void OnClickStart() {
        BTManager.Instance.LoadScene("Game");
    }
    
    //
    // PLAYER MENU
    //

    public void SetNbPlayers(int nbPlayer) {
        nbPlayers.text = "" + nbPlayer;
    }
    
    public void OnClickConfirmPlayers() {
        if (!PlayerManager.Instance.CheckPlayers()) return;
        mainMenu.SetActive(true);
        playerMenu.SetActive(false);
        BTManager.Instance.SetConfigPlayer(true);
    }
}