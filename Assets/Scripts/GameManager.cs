using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Transform[] teleportPoints;
    [SerializeField] private Transform robot;
    [SerializeField] private Transform player;

    public bool hasPressedTheMenu;

    public RobotScript robotScript => robot.GetComponent<RobotScript>();

    public void TeleportPlayer(int index)
    {
        player.position = robot.position = teleportPoints[index].position;
        player.rotation = robot.rotation = teleportPoints[index].rotation;
        robot.localPosition += robot.forward + new Vector3(0, 0, 2);
    }
}
