using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class CameraTracking : MonoBehaviour
{
    public List<PlayerInput> players = new List<PlayerInput>();

    public playerController playerController;
    private float xPos;
    private float yPos;
    private float zPos;

    // Update is called once per frame
    void Update()
    {
        players = playerController.getPlayers();
        if (players.Count > 1)
        {
            //Use the averages of each players position to calcualte a X & Z position for the camera
            xPos = (players[0].gameObject.transform.position.x + players[1].gameObject.transform.position.x) / 2;
            zPos = (players[0].gameObject.transform.position.z + players[1].gameObject.transform.position.z) / 2;

            //Change the height of the camera based on how far the players are apart
            yPos = (Vector3.Distance(players[0].gameObject.transform.position,players[1].gameObject.transform.position) / 2) + 20;
            transform.position = new Vector3(xPos, yPos, zPos);
        }
        else if(players.Count == 1)
        {
            //Set the camera X & Z position to the position of the player 
            xPos = players[0].gameObject.transform.position.x;
            zPos = players[0].gameObject.transform.position.z;
            yPos = 20;
            transform.position = new Vector3(xPos, yPos, zPos);
        }

    }
}
 