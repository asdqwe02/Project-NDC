using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class SaveSytemManagement : MonoBehaviour
{
    
    public static void SavePlayer(string Slot, PlayerController player)
    {
        string path = Application.persistentDataPath + "/Player" + Slot + ".savefile";
        PlayerData data = new PlayerData(player);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }
    public static PlayerData LoadPlayer(string Slot)
    {
        string path = Application.persistentDataPath + "/Player" + Slot + ".savefile";
        
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData so = JsonUtility.FromJson<PlayerData>(json) as PlayerData;
            return so;
        }
        else
        {
            return null;
        }
    }
}
