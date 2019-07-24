using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Networking;

public class DataController : MonoBehaviour
{
    private RoundData[] allRoundData;
    // private ResponseData[] responseData;

    private PlayerProgress playerProgress;
    private string gameDataFileName = "data.json";

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad (gameObject);
        LoadGameData();
        LoadPlayerProgress ();
        SceneManager.LoadScene ("MenuScreen");
    }

    public RoundData GetCurrentRoundData ()
    {
        return allRoundData [0];
    }

    private void LoadPlayerProgress()
    {
        playerProgress = new PlayerProgress ();

        if (PlayerPrefs.HasKey("highestScore"))
        {
            playerProgress.highestScore = PlayerPrefs.GetInt ("highestScore");
        }
    }

    private void SavePlayerProgress()
    {
        PlayerPrefs.SetInt ("highestScore", playerProgress.highestScore);
    }

    public void SubmitNewPlayerScore(int newScore)
    {
        if (newScore > playerProgress.highestScore)
        {
            playerProgress.highestScore = newScore;
            SavePlayerProgress();
        }
    }

    public int GetHighestScore ()
    {
        return playerProgress.highestScore;
    }

    private void LoadGameData()
    {
        StartCoroutine(GetRequest("http://192.168.0.10:8000/api/getPregunta"));
        // string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        // if (File.Exists(filePath))
        // {
        //     string dataAsJson = File.ReadAllText(filePath);
        //     GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);

        //     allRoundData = loadedData.allRoundData;
        // } else
        // {
        //     Debug.LogError("No se pudo cargar los datos del juego!");
        // }
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                string dataAsJson = webRequest.downloadHandler.text;
                ResponseData responseData = JsonUtility.FromJson<ResponseData>(dataAsJson);
                allRoundData = responseData.allRoundData;
                // Debug.Log(pages[page] + ":\nReceived: " + dataAsJson);
            }
        }
    }
}
