using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class playerController : MonoBehaviour
{
    private List<PlayerInput> players = new List<PlayerInput>();
    public PlayerInputManager inputManager;

    public Transform[] spawnPoints;

    void OnPlayerJoined(PlayerInput playerInput)
    {
        //Add player to the list of players
        players.Add(playerInput);
        Rigidbody rb = playerInput.GetComponent<Rigidbody>();
        rb.MovePosition(spawnPoints[playerInput.playerIndex].transform.position);
        //Stop more players from joining after 2 have joined
        if(playerInput.playerIndex >= 1)
        {
            inputManager.DisableJoining();
        }
    }

    void OnPlayerLeft(PlayerInput playerInput)
    {
        players.Remove(playerInput);
    }

    public List<PlayerInput> getPlayers()
    {
        return players;
    }

}
