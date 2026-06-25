using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameData : MonoBehaviour
{
    public static GameData Instance; // singleton
    public int winnerPlayer;         // winnaar

    public int playerCount = 0;      // aantal spelers
    public List<InputDevice> joinedDevices = new List<InputDevice>(); // devices

    private void Awake()
    {
        // check of er al een GameData bestaat
        if (Instance != null)
        {
            Destroy(gameObject); // dubbele weg
            return;
        }

        Instance = this;               // opslaan
        DontDestroyOnLoad(gameObject); // blijft tussen scenes
    }
}
