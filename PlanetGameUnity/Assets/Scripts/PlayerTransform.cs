using UnityEngine;

public class PlayerTransform
{
    public string player_id;
    public float x;
    public float y;
    public float z;
    public PlayerTransform(string playerId, float posX, float posY, float posZ)
    {
        player_id = playerId;
        x = posX;
        y = posY;
        z = posZ;
    }
}
