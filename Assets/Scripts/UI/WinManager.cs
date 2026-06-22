using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public TextMeshProUGUI winText; // tekst die laat zien wie gewonnen heeft
    public Button RestartBut;
    public Button ReturnBut;
    public Button QuitBut;

    private void Start()
    {
        RestartBut.onClick.AddListener(RestartGame);
        ReturnBut.onClick.AddListener(ReturnToMenu);
        QuitBut.onClick.AddListener(QuitGame);

        // haal winnaar op uit GameData
        int winner = GameData.Instance.winnerPlayer;
        // zet tekst
        winText.text = "Player " + winner + " Wins!";
    }

    // knop: restart game
    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene"); // jouw speel-scene naam
    }

    // knop: terug naar menu
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
