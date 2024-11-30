using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class API_manager : MonoBehaviour
{
    private string baseUrl = "https://2025.nti-gamedev.ru/api/games/{game_uuid}/players/";
    public void SendRequest(string gameUuid, string method, string jsonBody = null)
    {
        string url = baseUrl.Replace("{game_uuid}", gameUuid);
        StartCoroutine(SendApiRequest(url, method, jsonBody));
    }

    public static IEnumerator SendApiRequest(string url, string method, string jsonBody)
    {
        UnityWebRequest webRequest;

        if (method.ToUpper() == "POST")
        {
            webRequest = UnityWebRequest.PostWwwForm(url, jsonBody);
            webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonBody));
            webRequest.SetRequestHeader("Content-Type", "application/json");
        }
        else if (method.ToUpper() == "PUT")
        {
            webRequest = new UnityWebRequest(url, "PUT");
            webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonBody));
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
        }
        else // Default to GET
        {
            webRequest = UnityWebRequest.Get(url);
        }

        // ���������� ������ � ���� ������
        yield return webRequest.SendWebRequest();

        // ��������� �� ������
        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("error: " + webRequest.error);
        }
        else
        {
            // �������� ������ � ���� ������
            string jsonResponse = webRequest.downloadHandler.text;
            Debug.Log("API: " + jsonResponse);
        }
    }
}
