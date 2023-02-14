using TMPro;
using UnityEngine;

public class PlayerResult : MonoBehaviour {

    [SerializeField] private TMP_Text playerResultText;
    [SerializeField] private Color firstColor;
    [SerializeField] private Color secondColor;
    [SerializeField] private Color thirdColor;
    [SerializeField] private Color otherColor;
    
    private PlayerManager.Player myPlayer;

    public void Init(PlayerManager.Player player, int position) {
        myPlayer = player;

        if (position == 1) {
            playerResultText.text = "1st - " + myPlayer.name + " : " + myPlayer.points+"pts";
            playerResultText.color = firstColor;
        }
        
        else if (position == 2) {
            playerResultText.text = "2nd - " + myPlayer.name + " : " + myPlayer.points+"pts";
            playerResultText.color = secondColor;
        }
        
        else if (position == 3) {
            playerResultText.text = "3rd - " + myPlayer.name + " : " + myPlayer.points+"pts";
            playerResultText.color = thirdColor;
        }

        else {
            playerResultText.text = position+"th - " + myPlayer.name + " : " + myPlayer.points+"pts";
            playerResultText.color = otherColor;
        }
    }
}
