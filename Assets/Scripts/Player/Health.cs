using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 5; // max hp
    private int current;      // huidige hp

    private Slider slider;    // healthbar slider

    private void Awake()
    {
        current = maxHealth; // start hp

        // pak slider uit children
        slider = GetComponentInChildren<Slider>();

        // slider instellen
        if (slider != null)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        current -= amount;

        if (slider != null)
            slider.value = current;

        if (current <= 0)
        {
            // verberg healthbar
            if (slider != null)
                slider.gameObject.SetActive(false);

            // bepaal winnaar
            int myIndex = GetComponent<PlayerInput>().playerIndex;
            int winner = (myIndex == 0) ? 2 : 1; // als player 1 dood is wint player 2

            GameData.Instance.winnerPlayer = winner;

            // laad endscene
            SceneManager.LoadScene("EndMenu");

            Destroy(gameObject);
        }
    }

}
