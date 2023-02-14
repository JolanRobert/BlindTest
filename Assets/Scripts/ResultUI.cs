using System.Collections.Generic;
using UnityEngine;

public class ResultUI : MonoBehaviour {

    [SerializeField] private GameObject playerResultPrefab;
    [SerializeField] private Transform playerResultParent;
    
    void Start() {
        List<PlayerManager.Player> players = PlayerManager.Instance.players;
        players.Sort();

        for (int i = 0; i < players.Count; i++) {
            PlayerResult playerScore = Instantiate(playerResultPrefab, playerResultParent).GetComponent<PlayerResult>();
            playerScore.Init(players[i],i+1);
        }
    }
    
    public void OnClickExit() {
        Application.Quit();
    }
}
