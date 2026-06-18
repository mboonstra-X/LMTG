using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Toggle p1Toggle;
    public Toggle p2Toggle;
    public Toggle p3Toggle;
    public Toggle p4Toggle;
    [Header("Buttons")]
    public Button startButton;
    public Button controlsButton;
    public Button quitButton;

    private int nextPlayerIndex = 0;
    // array om bij te houden welke spelers ready zijn
    private bool[] readyStates = new bool[4];

    private void Start()
    {
        // zet de toggles interactables uit als ik dat vergeten ben
        p1Toggle.interactable = false;
        p2Toggle.interactable = false;
        p3Toggle.interactable = false;
        p4Toggle.interactable = false;
        // zet de toggles uit als dat niet standaard is
        SetToggle(0, false);
        SetToggle(1, false);
        SetToggle(2, false);
        SetToggle(3, false);
        // voeg listeners toe aan de buttons indien dat nog niet gedaan is
        startButton.onClick.AddListener(StartGame);
        controlsButton.onClick.AddListener(ShowControls);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void OnJoin(InputAction.CallbackContext ctx)
    {
        // checck of de actie wordt uitgevoerd, anders return
        if (!ctx.performed) return;
        // krijg het device van de speler
        InputDevice device = ctx.control.device;

        // Check of device al gejoined is
        if (GameData.Instance.joinedDevices.Contains(device))
            return;
        // check of er nog plek is (MAX 4)
        if (nextPlayerIndex > 3)
            return;

        // Toggle aan als nieuwe speler gejoind is
        SetToggle(nextPlayerIndex, true);

        // Device opslaan
        GameData.Instance.joinedDevices.Add(device);

        nextPlayerIndex++;
    }

    // wordt aangeroepen door de Ready action in het Menu action map
    public void OnReady(InputAction.CallbackContext ctx)
    {
        // check of de actie wordt uitgevoerd, anders return
        if (!ctx.performed) return;
        // krijg het device van de speler
        InputDevice device = ctx.control.device;
        // zoek de index van dit device in de joinedDevices lijst
        int index = GameData.Instance.joinedDevices.IndexOf(device);
        // als device niet gevonden is, return
        if (index == -1) return;
        // toggle de ready state van deze speler
        readyStates[index] = !readyStates[index];
        // verander de kleur van de toggle zodat je ziet dat hij ready is
        Toggle t = GetToggleByIndex(index);
        if (t != null)
        {
            ColorBlock cb = t.colors;
            // groen als ready, wit als niet ready
            cb.normalColor = readyStates[index] ? Color.green : Color.white;
            cb.highlightedColor = cb.normalColor;
            t.colors = cb;
        }
    }

    private Toggle GetToggleByIndex(int index)
    {
        // geef de juiste toggle terug op basis van index
        switch (index)
        {
            case 0: return p1Toggle;
            case 1: return p2Toggle;
            case 2: return p3Toggle;
            case 3: return p4Toggle;
        }
        return null;
    }

    private void SetToggle(int index, bool state)
    {
        switch (index)
        {
            // Zet de juiste toggle aan of uit
            case 0: p1Toggle.isOn = state; break;
            case 1: p2Toggle.isOn = state; break;
            case 2: p3Toggle.isOn = state; break;
            case 3: p4Toggle.isOn = state; break;
        }
    }

    private void StartGame()
    {
        // sla het aantal spelers op in GameData zodat MultiplayerManager weet hoeveel spelers er zijn
        GameData.Instance.playerCount = nextPlayerIndex;
        // check of er spelers zijn gejoined, anders log een bericht en return
        if (nextPlayerIndex == 0)
        {
            Debug.Log("Geen spelers ready.");
            return;
        }
        // check of alle gejoinde spelers ook echt ready zijn
        for (int i = 0; i < nextPlayerIndex; i++)
        {
            // als een speler niet ready is, log en return
            if (!readyStates[i])
            {
                Debug.Log("Niet alle spelers zijn ready.");
                return;
            }
        }
        // laad de game scene en start het spel
        SceneManager.LoadScene("GameScene");
    }
    // laat controls zien (heb nog niet)
    private void ShowControls()
    {
        Debug.Log("Controls screen nog niet geïmplementeerd.");
    }
    // sluit de game (werkt niet in editor)
    private void QuitGame()
    {
        Debug.Log("Quit game.");
        Application.Quit();
    }
}
