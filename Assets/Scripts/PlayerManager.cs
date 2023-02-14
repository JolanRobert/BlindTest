using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    
    public static PlayerManager Instance;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerParent;
    public List<Player> players = new List<Player>();
    
    void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start() {
        for (int i = 0; i < 3; i++) players.Add(new Player());
    }

    public void AddPlayer() {
        if (players.Count == 10) return;
        Instantiate(playerPrefab, playerParent);
        players.Add(new Player());
        SetupUI.Instance.SetNbPlayers(players.Count);
    }

    public void RemovePlayer() {
        if (players.Count == 3) return;
        Destroy(playerParent.GetChild(playerParent.childCount-1).gameObject);
        players.RemoveAt(players.Count-1);
        SetupUI.Instance.SetNbPlayers(players.Count);
    }

    public bool CheckPlayers() {
        for (int i = 0; i < playerParent.childCount; i++) {
            string playerName = playerParent.GetChild(i).GetComponent<TMP_InputField>().text;
            if (playerName.Equals("")) return false;
            players[i].name = playerParent.GetChild(i).GetComponent<TMP_InputField>().text;
        }

        SetupUI.Instance.SetEditPlayerText(players.Count);
        return true;
    }

    [Serializable]
    public class Player : IComparable<Player> {
        public string name;
        public int points;
        
        public int CompareTo(Player other) {
            return other.points.CompareTo(points);
        }
    }
}
