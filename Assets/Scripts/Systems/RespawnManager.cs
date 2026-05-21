using UnityEngine;
using System.Collections;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay = 1.5f;

    public IEnumerator RespawnPlayer(PlayerMovement player)
    {
        yield return new WaitForSeconds(respawnDelay);

        player.Respawn(respawnPoint.position);

        GameEventBus.OnPlayerRespawn?.Invoke();
    }
}