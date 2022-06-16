using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonPortalController : Interactable
{
    private string nextSceneName, nextScenePassword;
    public enum NextRoom
    {
        NormalRoom,
        BossRoom,
    }
    public NextRoom room;
    public struct NextRoomInfo
    {
        public string SceneName;
        public string ScenePassword;
        public NextRoomInfo(string sname, string spassword)
        {
            SceneName = sname;
            ScenePassword = spassword;
        }
    }
    NextRoomInfo[] BossRoomInfo;
    private void Awake() {
        BossRoomInfo = new NextRoomInfo[]{
            new NextRoomInfo("Room_Boss1","Boss_Room1_Entrance"),
            new NextRoomInfo("Room_Boss2", "Boss_Room2_Entrance")
        };
        switch (room)
        {
            case NextRoom.NormalRoom:
                break;
            case NextRoom.BossRoom:
                int index = Random.Range(0,BossRoomInfo.Length);
                nextSceneName = BossRoomInfo[index].SceneName;
                nextScenePassword = BossRoomInfo[index].ScenePassword;
                break;
            default:
                break;
        }
    }
    public override void Interact(){
        PlayerController.instance.CloseInteractableIcon();
        PlayerController.instance.scenePassword = nextScenePassword;
        SceneManager.LoadScene(nextSceneName);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TurnOnIIcon(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        TurnOffIIcon(collision);
    }

}
