using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public List<Transform> spawnPoints = new List<Transform>();
    public Material[] playerMaterials;

    private void Start()
    {
        int count = GameData.Instance.playerCount;
        List<InputDevice> devices = GameData.Instance.joinedDevices;

        for (int i = 0; i < count; i++)
        {
            SpawnPlayer(i, devices[i]);
        }
    }

    private void SpawnPlayer(int index, InputDevice device)
    {
        Transform spawn = spawnPoints[index];

        GameObject obj = Instantiate(playerPrefab, spawn.position, spawn.rotation);

        PlayerInput pi = obj.GetComponent<PlayerInput>();
        pi.SwitchCurrentControlScheme(device);

        Renderer r = obj.GetComponentInChildren<Renderer>();
        if (r != null && index < playerMaterials.Length)
            r.material = playerMaterials[index];
    }
}
