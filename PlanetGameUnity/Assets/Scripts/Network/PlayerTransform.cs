using UnityEngine;

public class PlayerTransform
{
    public string player_id;
    public float x;
    public float y;
    public float z;
    public float rot_y;
    public PlayerTransform(string playerId, float posX, float posY, float posZ, float rotY)
    {
        player_id = playerId;
        x = posX;
        y = posY;
        z = posZ;
        rot_y = rotY;
    }
}
