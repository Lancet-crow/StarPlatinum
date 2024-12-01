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

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("������: " + request.error);
        }
        else
        {
            ret = request.downloadHandler.text;
            Debug.Log(request.downloadHandler.text);
        }
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
    public string LastTime; // ���� ��� �������� ������� ���������� ��������� � ������� ������
}

[System.Serializable]
public class Item
{
    public string name; // ��� ���� ����� �������������� ��� ������
    public Resource resources; // ��������� ������ �������
}