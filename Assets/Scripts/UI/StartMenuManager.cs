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

    [Header("Status Text")]
    public TMPro.TMP_Text statusText;

    [Header("Control Panel")]
    public GameObject controlPanel; // het panel dat de controls laat zien

    private int nextPlayerIndex = 0;
    // array om bij te houden welke spelers ready zijn
    private bool[] readyStates = new bool[4];

    // bool om bij te houden of het controls panel open staat
    private bool controlsOpen = false;

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

        // zorg dat het control panel uit staat bij start
        if (controlPanel != null)
            controlPanel.SetActive(false);

        // voeg listeners toe aan de buttons indien dat nog niet gedaan is
        startButton.onClick.AddListener(StartGame);
        controlsButton.onClick.AddListener(ToggleControls);
        quitButton.onClick.AddListener(QuitGame);

        // update de status tekst bij start
        UpdateStatusText();
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

        // update status tekst
        UpdateStatusText();
    }

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

        // update status tekst
        UpdateStatusText();
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

    private void ToggleControls()
    {
        // toggle het controls panel aan/uit
        controlsOpen = !controlsOpen;

        if (controlPanel != null)
            controlPanel.SetActive(controlsOpen);

        // update status tekst zodat het niet verwarrend is
        UpdateStatusText();
    }

    private void UpdateStatusText()
    {
        // als controls open zijn, laat alleen controls info zien
        if (controlsOpen)
        {
            statusText.text = "Controls open - druk CONTROLS om te sluiten";
            return;
        }

        // als er nog geen spelers zijn
        if (nextPlayerIndex == 0)
        {
            statusText.text = "Druk op JOIN om een speler toe te voegen";
            return;
        }

        // check of alle spelers ready zijn
        bool allReady = true;
        for (int i = 0; i < nextPlayerIndex; i++)
        {
            if (!readyStates[i])
            {
                allReady = false;
                break;
            }
        }

        // als niet iedereen ready is
        if (!allReady)
        {
            statusText.text = "Druk op READY (R / Button East)";
            return;
        }

        // als iedereen ready is
        statusText.text = "Alle spelers zijn ready! Druk op START";
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

    // sluit de game (werkt niet in editor)
    private void QuitGame()
    {
        Debug.Log("Quit game.");
        Application.Quit();
    }
}
