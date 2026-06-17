using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    private int nextSpawnIndex = 0;

    private PlayerInputManager pim;

    private void Awake()
    {
        // Zoek de PlayerInputManager in de scene
        pim = FindAnyObjectByType<PlayerInputManager>();
    }

    private void OnEnable()
    {
        if (pim != null)
            pim.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable()
    {
        if (pim != null)
            pim.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput player)
    {
        Transform spawn = spawnPoints[nextSpawnIndex];

        player.transform.position = spawn.position;
        player.transform.rotation = spawn.rotation;

        nextSpawnIndex = (nextSpawnIndex + 1) % spawnPoints.Length;
    }
}
