using UnityEngine;
using System.IO;

[System.Serializable]
public class Config
{
    public string aze_base_url;
    public string create_account_route;
    public string create_player_route;

    private static Config _instance;

    public static Config Instance
    {
        get
        {
            if (_instance == null)
            {
                LoadConfig();
            }
            return _instance;
        }
    }

    private static void LoadConfig()
    {
        TextAsset configTextAsset = Resources.Load<TextAsset>("Config/config");
        if (configTextAsset != null)
        {
            _instance = JsonUtility.FromJson<Config>(configTextAsset.text);
        }
        else
        {
            Debug.LogError("Config file not found in Resources/Config folder.");
        }
    }
}
