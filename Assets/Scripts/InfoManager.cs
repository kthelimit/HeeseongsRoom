using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class InfoManager
{
   public static readonly InfoManager Instance = new InfoManager();
    public GameInfo GameInfo
    {
        get;set;
    }

    private InfoManager() { }
    public void SaveLocal()
    {
        var json = JsonConvert.SerializeObject(GameInfo);
        string path = Path.Combine(Application.persistentDataPath, "game_info.json");
        File.WriteAllText(path, json);
        Debug.Log("save complete");
    }

}
