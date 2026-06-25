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
    public GameObject controlPanel; // controls scherm

    private int nextPlayerIndex = 0;        // aantal gejoinde spelers
    private bool[] readyStates = new bool[4]; // ready per speler
    private bool controlsOpen = false;      // staat controls open

    private void Start()
    {
        // toggles uitzetten
        p1Toggle.interactable = false;
        p2Toggle.interactable = false;
        p3Toggle.interactable = false;
        p4Toggle.interactable = false;

        // toggles off
        SetToggle(0, false);
        SetToggle(1, false);
        SetToggle(2, false);
        SetToggle(3, false);

        // controls panel uit
        if (controlPanel != null)
            controlPanel.SetActive(false);

        // knoppen koppelen
        startButton.onClick.AddListener(StartGame);
        controlsButton.onClick.AddListener(ToggleControls);
        quitButton.onClick.AddListener(QuitGame);

        UpdateStatusText(); // tekst updaten
    }

    public void OnJoin(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return; // alleen bij performed

        InputDevice device = ctx.control.device; // device pakken

        if (GameData.Instance.joinedDevices.Contains(device))
            return; // al gejoined

        if (nextPlayerIndex > 3)
            return; // max 4

        SetToggle(nextPlayerIndex, true); // toggle aan

        GameData.Instance.joinedDevices.Add(device); // opslaan

        nextPlayerIndex++; // volgende slot

        UpdateStatusText(); // tekst updaten
    }

    public void OnReady(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        InputDevice device = ctx.control.device;

        int index = GameData.Instance.joinedDevices.IndexOf(device); // speler index

        if (index == -1) return; // niet gevonden

        readyStates[index] = !readyStates[index]; // toggle ready

        // toggle kleur aanpassen
        Toggle t = GetToggleByIndex(index);
        if (t != null)
        {
            ColorBlock cb = t.colors;
            cb.normalColor = readyStates[index] ? Color.green : Color.white;
            cb.highlightedColor = cb.normalColor;
            t.colors = cb;
        }

        UpdateStatusText();
    }

    private Toggle GetToggleByIndex(int index)
    {
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
            case 0: p1Toggle.isOn = state; break;
            case 1: p2Toggle.isOn = state; break;
            case 2: p3Toggle.isOn = state; break;
            case 3: p4Toggle.isOn = state; break;
        }
    }

    private void ToggleControls()
    {
        controlsOpen = !controlsOpen; // open/close

        if (controlPanel != null)
            controlPanel.SetActive(controlsOpen);

        UpdateStatusText();
    }

    private void UpdateStatusText()
    {
        if (controlsOpen)
        {
            statusText.text = "Controls open - druk CONTROLS om te sluiten";
            return;
        }

        if (nextPlayerIndex == 0)
        {
            statusText.text = "Druk JOIN om te joinen (SPACE/A)";
            return;
        }

        bool allReady = true;
        for (int i = 0; i < nextPlayerIndex; i++)
        {
            if (!readyStates[i])
            {
                allReady = false;
                break;
            }
        }

        if (!allReady)
        {
            statusText.text = "Druk READY (R/B)";
            return;
        }

        statusText.text = "Iedereen ready! Druk START";
    }

    private void StartGame()
    {
        GameData.Instance.playerCount = nextPlayerIndex; // opslaan

        if (nextPlayerIndex == 0)
        {
            Debug.Log("Geen spelers.");
            return;
        }

        for (int i = 0; i < nextPlayerIndex; i++)
        {
            if (!readyStates[i])
            {
                Debug.Log("Niet iedereen ready.");
                return;
            }
        }

        SceneManager.LoadScene("GameScene"); // start game
    }

    private void QuitGame()
    {
        Debug.Log("Quit game.");
        Application.Quit(); // afsluiten
    }
}
