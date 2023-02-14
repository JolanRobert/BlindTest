using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {

    [SerializeField] private TMP_Text playerName;
    [SerializeField] private Image scoreButton;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Color oneColor;
    [SerializeField] private Color twoColor;
    [SerializeField] private Color threeColor;

    private PlayerManager.Player myPlayer;

    public void Init(PlayerManager.Player player) {
        myPlayer = player;

        playerName.text = player.name;
    }

    public void AddScore() {
        myPlayer.points += selectedValue;

        HideScore();
        selectedValue = 0;
        scoreButton.color = Color.white;
        scoreText.text = "+0";
    }

    public void ShowScore() {
        playerName.text = myPlayer.name + " (" + (myPlayer.points + selectedValue) + ")";
    }
    
    public void HideScore() {
        playerName.text = myPlayer.name;
    }

    public int selectedValue;

    public void OnClickScore() {
        selectedValue++;
        if (selectedValue > 3) selectedValue = 0;

        if (selectedValue == 0) {
            scoreButton.color = Color.white;
            scoreText.text = "+0";
        }
        else if (selectedValue == 1) {
            scoreButton.color = oneColor;
            scoreText.text = "+1";
        }
        else if (selectedValue == 2) {
            scoreButton.color = twoColor;
            scoreText.text = "+2";
        }
        else if (selectedValue == 3) {
            scoreButton.color = threeColor;
            scoreText.text = "+3";
        }
        
        if (!playerName.text.Equals(myPlayer.name)) ShowScore();
    }
}
