using System;
using UnityEngine;
using UnityEngine.Events;

public class Respawn_Trigger_Script : MonoBehaviour
{
    public UnityEvent RespawnEvent;
    public GameObject Player;
    public GameObject respawnPoint;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == Player)
        {
            RespawnEvent.Invoke();
            Respawn();
        }
    }

    private void Respawn()
    {
        Console.WriteLine("Respawn Triggered");
        CharacterController _characterController = gameObject.GetComponent<CharacterController>();
        _characterController.enabled = false;
        Player.transform.position = respawnPoint.transform.position;
        _characterController.enabled = true;
    }
}
