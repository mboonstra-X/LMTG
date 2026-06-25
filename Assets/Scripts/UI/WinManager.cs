using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public TextMeshProUGUI winText; // winnaar tekst
    public Button RestartBut;       // restart knop
    public Button ReturnBut;        // menu knop
    public Button QuitBut;          // quit knop

    private void Start()
    {
        // knoppen koppelen
        RestartBut.onClick.AddListener(RestartGame);
        ReturnBut.onClick.AddListener(ReturnToMenu);
        QuitBut.onClick.AddListener(QuitGame);

        // winnaar ophalen
        int winner = GameData.Instance.winnerPlayer;

        // tekst zetten
        winText.text = "Player " + winner + " Wins!";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene"); // opnieuw spelen
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("StartMenu"); // terug naar menu
    }

    public void QuitGame()
    {
        Application.Quit(); // spel afsluiten
    }
}
