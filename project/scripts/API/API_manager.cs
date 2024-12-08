using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class API_manager : MonoBehaviour
{
    public string baseUrl = "https://2025.nti-gamedev.ru/api/games/744e9e81-85be-4099-883c-e133b28e9a0e/";
    public string players = "https://2025.nti-gamedev.ru/api/games/744e9e81-85be-4099-883c-e133b28e9a0e/players/";
    public string logs = "https://2025.nti-gamedev.ru/api/games/{game_uuid}/logs/";

    private bool hasInternetConnection;

    private List<GameData> all_usernames;

    public static string ret;

    public static API_manager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CheckPlayerExistence();
        //StartCoroutine(SendPostRequest());
        //StartCoroutine(SendGetRequest());
    }

    IEnumerator CheckInternetConnection(Action<bool> action)
    {
        UnityWebRequest www = new(baseUrl);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            print(www.result);
            action(false);
        }
        else
        {
            action(true);
        }
    }

    public void CheckPlayerExistence()
    {
        StartCoroutine(WaitForPlayerExistence());
    }

    public void PostPlayer()
    {
        StartCoroutine(SendPostPlayerRequest());
    }

    private IEnumerator WaitForPlayerExistence()
    {
        yield return SendGetPlayersRequest();
        print(all_usernames);
    }

    private IEnumerator SendGetPlayersRequest()
    {
        yield return CheckInternetConnection((isConnected) => {
            hasInternetConnection = isConnected;
        });
        if (hasInternetConnection)
        {
            UnityWebRequest request = UnityWebRequest.Get(players);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка: " + request.error);
            }
            else
            {
                ret = request.downloadHandler.text;
                all_usernames = JsonUtility.FromJson <List<GameData>>(ret);
            }
        }
    }
    private IEnumerator SendGetResourcesRequest()
    {
        var username = "";
        yield return StartCoroutine(CheckInternetConnection((isConnected) => {
            hasInternetConnection = isConnected;
        }));
        if (hasInternetConnection)
        {
            UnityWebRequest request = UnityWebRequest.Get($"{players}{username}");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка: " + request.error);
            }
            else
            {
                ret = request.downloadHandler.text;
                print(ret);
            }
        }
    }

    private IEnumerator SendPostPlayerRequest()
    {
        yield return CheckInternetConnection((isConnected) => {
            hasInternetConnection = isConnected;
        });
        if (hasInternetConnection)
        {
            var gameData = new GameData
            {
                username = MainMenu.username
            };

            UnityWebRequest request = UnityWebRequest.PostWwwForm(players, JsonUtility.ToJson(gameData));

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка: " + request.error);
            }
            else
            {
                ret = request.downloadHandler.text;
                print(ret);
            }
        } 
    }

    private IEnumerator SendPutRequest()
    {
        UnityWebRequest request= UnityWebRequest.Get(players);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ошибка: " + request.error);
        }
        else
        {
            ret = request.downloadHandler.text;
            Debug.Log(request.downloadHandler.text);
        }
    }

    private IEnumerator SendDeletePlayerRequest(string username)
    {
        UnityWebRequest request = UnityWebRequest.Delete($"{players}{username}");
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ошибка: " + request.error);
        }
    }
}