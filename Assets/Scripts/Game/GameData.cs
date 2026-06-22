using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameData : MonoBehaviour
{
    // zoek de gamedata singleton zodat alle scripts erbij kunnen
    public static GameData Instance;
    public int winnerPlayer; // wie heeft gewonnen

    // maak een lijst met aantal spelers die gejoind zijn
    public int playerCount = 0;
    public List<InputDevice> joinedDevices = new List<InputDevice>();

    // singleton voor GameData met dontdestroyonload zodat het blijft bestaan tussen scenes
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
