using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerManager : MonoBehaviour
{
    public GameObject playerPrefab;          // player prefab
    public List<Transform> spawnPoints;      // spawn plekken
    public Material[] playerMaterials;       // kleuren per speler

    private void Start()
    {
        int count = GameData.Instance.playerCount;          // aantal spelers
        List<InputDevice> devices = GameData.Instance.joinedDevices; // devices

        // spawn spelers
        for (int i = 0; i < count; i++)
            SpawnPlayer(i, devices[i]);
    }

    private void SpawnPlayer(int index, InputDevice device)
    {
        Transform spawn = spawnPoints[index]; // spawn plek

        GameObject obj = Instantiate(playerPrefab, spawn.position, spawn.rotation); // spawn player

        PlayerInput pi = obj.GetComponent<PlayerInput>();
        pi.SwitchCurrentControlScheme(device); // device koppelen

        Renderer r = obj.GetComponentInChildren<Renderer>();
        if (r != null && index < playerMaterials.Length)
            r.material = playerMaterials[index]; // kleur zetten
    }
}
