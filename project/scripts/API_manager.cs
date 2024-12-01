using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class API_manager : MonoBehaviour
{
    public string baseUrl = "https://2025.nti-gamedev.ru/api/games/744e9e81-85be-4099-883c-e133b28e9a0e/";
    public string players = "https://2025.nti-gamedev.ru/api/games/744e9e81-85be-4099-883c-e133b28e9a0e/players/";

    public static string ret;

    private void Start()
    {
        StartCoroutine(SendGetRequest());
        //StartCoroutine(SendPostRequest());
        //StartCoroutine(SendGetRequest());
    }

    public void GetPlayers()
    {
        StartCoroutine(SendGetRequest());
    }

    private IEnumerator SendGetRequest()
    {
        UnityWebRequest request = UnityWebRequest.Get(players);

        yield return request.SendWebRequest();

        ret = request.downloadHandler.text;

        Debug.Log(request.downloadHandler.text);
    }
    
    private IEnumerator SendPostRequest()
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(players, "{\"name\': \'Cyber_Beer_Bear\"");

        yield return request.SendWebRequest();
    }
}
[System.Serializable]
public class Resource
{
    public string Planet_Name;
    public int X_pos;
    public int Y_pos;
    public int Wood;
    public int Honey;
    public int Gold;
    public int Ice;
    public string LastTime; // Поле для хранения времени последнего изменения в формате строки
}

[System.Serializable]
public class Item
{
    public string name; // Это поле будет использоваться для поиска
    public Resource resources; // Оставляем только ресурсы
}