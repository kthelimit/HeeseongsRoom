using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    public Button resetButton;
    private void Start()
    {
        if (IsThereExist())
        {
            resetButton.gameObject.SetActive(true);
        }
        else
        {
            resetButton.gameObject.SetActive(false);
        }
    }
    public void Clicked()
    {
        SceneManager.LoadSceneAsync("Room");
    }

    public void DeleteCurrentGameData()
    {
        string LoaddataPath = Application.persistentDataPath + $"/game_info.json";
        if (File.Exists(LoaddataPath))
        {
            File.Delete(LoaddataPath);
            resetButton.gameObject.SetActive(false);
        }
    }
    public bool IsThereExist()
    {
        string LoaddataPath = Application.persistentDataPath + $"/game_info.json";
        if (File.Exists(LoaddataPath))
        {
            return true;
        }
        return false;
    }
}
