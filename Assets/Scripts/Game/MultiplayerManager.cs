using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerManager : MonoBehaviour
{
    // Prefab van de speler (tank)
    public GameObject playerPrefab;
    public List<Transform> spawnPoints = new List<Transform>();
    public Material[] playerMaterials;

    private void Start()
    {
        // Spawnt spelers op basis van het aantal spelers en hun devices
        int count = GameData.Instance.playerCount;
        List<InputDevice> devices = GameData.Instance.joinedDevices;

        // loop door de spelers en spawn ze
        for (int i = 0; i < count; i++)
        {
            SpawnPlayer(i, devices[i]);
        }
    }

    private void SpawnPlayer(int index, InputDevice device)
    {
        Transform spawn = spawnPoints[index];

        // Spawn tank
        GameObject obj = Instantiate(playerPrefab, spawn.position, spawn.rotation);

        // PlayerInput krijgen
        PlayerInput pi = obj.GetComponent<PlayerInput>();

        // zodat de device van de speler en control scheme kloppen
        pi.SwitchCurrentControlScheme(device);

        // Kleur van speler instellen
        Renderer r = obj.GetComponentInChildren<Renderer>();
        if (r != null && index < playerMaterials.Length)
            r.material = playerMaterials[index];
    }
}
