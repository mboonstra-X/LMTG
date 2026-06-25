using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 5; // max hp
    private int current;      // huidige hp

    private Slider slider;    // healthbar

    private void Awake()
    {
        current = maxHealth; // start hp

        slider = GetComponentInChildren<Slider>(); // pak slider

        if (slider != null)
        {
            slider.maxValue = maxHealth; // max
            slider.value = maxHealth;    // current
        }
    }

    public void TakeDamage(int amount)
    {
        current -= amount; // hp omlaag

        if (slider != null)
            slider.value = current; // update bar

        if (current <= 0)
        {
            if (slider != null)
                slider.gameObject.SetActive(false); // hide bar

            int myIndex = GetComponent<PlayerInput>().playerIndex; // wie ben ik

            int winner = (myIndex == 0) ? 2 : 1; // andere wint

            GameData.Instance.winnerPlayer = winner; // opslaan

            SceneManager.LoadScene("EndMenu"); // endscreen

            Destroy(gameObject); // tank weg
        }
    }
}
