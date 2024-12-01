using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Text.RegularExpressions;
using System.Linq;

public class SetSaveButtonValues : MonoBehaviour
{
    private string jsonResponse = "[{\"name\":\"Beer_bear\",\"resources\":{\"Planet_Name\":\"Earth123\",\"X_pos\":1,\"Y_pos\":2,\"Wood\":10,\"Honey\":19,\"Gold\":5,\"Ice\":0,\"LastTime\":\"1696155600\"}},{\"name\":\"Beer_bear1\",\"resources\":{\"Planet_Name\":\"Mars\",\"X_pos\":3,\"Y_pos\":4,\"Wood\":20,\"Honey\":21,\"Gold\":15,\"Ice\":1,\"LastTime\":\"1696156500\"}},{\"name\":\"Beer_bear2\",\"resources\":{\"Planet_Name\":\"Venus\",\"X_pos\":5,\"Y_pos\":6,\"Wood\":30,\"Honey\":2,\"Gold\":25,\"Ice\":2,\"LastTime\":\"1696157400\"}}, {\"name\":\"Beer_bear\",\"resources\":{\"Planet_Name\":\"Earth1\",\"X_pos\":11,\"Y_pos\":21,\"Wood\":101,\"Honey\":191,\"Gold\":51,\"Ice\":10,\"LastTime\":\"16961556001\"}}]";
    public int indexToRetrieve = 0; // Укажите индекс игрока, которого хотите получить из найденных
    public TextMeshProUGUI main_text, small_text;

    public string playerNameToFind;

    private List<Item> foundPlayers;

    public void UpdateSaveSlots()
    {
        playerNameToFind = MainMenu.username.Trim(); // Удаляем пробелы
        Debug.Log("Searching for player: " + playerNameToFind);

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<API_manager>().GetPlayers();
        jsonResponse = API_manager.ret;// Удаляем пробелы
        Debug.Log("Searching for player: " + playerNameToFind);
        Debug.Log(jsonResponse);

        foundPlayers = GetPlayersByName(playerNameToFind);

        //Debug.Log(foundPlayers);

        if (foundPlayers.Count == 0)
        {
            Debug.LogWarning("No players found with the name: " + playerNameToFind);
            UpdateText(); // Обновляем текст, если игрок не найден
            return;
        }

        Debug.Log(playerNameToFind + " found players: " + foundPlayers.Count);
        GetPlayerFromFoundPlayersByIndex(foundPlayers, indexToRetrieve);
    }

    public void UpdateText()
    {
        main_text.text = "New game";
        if (indexToRetrieve == 0)
        {
            small_text.text = "Check your username and password, or start a new game!";
        }
        else
        {
            small_text.text = "Absolutely nothing...";
        }
    }

    // Метод для получения списка игроков по имени
    private List<Item> GetPlayersByName(string name)
    {
        // Десериализация массива JSON
        Item[] items = JsonHelper.FromJson<Item>(jsonResponse);
        List<Item> foundPlayers = new List<Item>();

        string trimmedPlayerName = name.Trim().ToLower(); // Удаляем пробелы и переводим в нижний регистр

        foreach (var item in items)
        {
            string cleanedItemName = Regex.Replace(item.name.ToLower(), @"\s+", "");
            string cleanedPlayerName = Regex.Replace(trimmedPlayerName, @"\s+", "");
            cleanedPlayerName = cleanedPlayerName.Substring(0, cleanedPlayerName.Length-1);

            Debug.Log($"Comparing '{cleanedItemName}' with '{cleanedPlayerName}' {cleanedItemName == cleanedPlayerName}");

            Debug.Log(string.Join(" ", cleanedItemName.Select(c => (int)c)) + '+' + string.Join(" ", cleanedPlayerName.Select(c => (int)c)));

            if (cleanedItemName == cleanedPlayerName) // Сравнение без учета регистра
            {
                foundPlayers.Add(item);
            }
        }

        return foundPlayers;
    }

    // Метод для получения игрока из найденных по индексу
    public void GetPlayerFromFoundPlayersByIndex(List<Item> foundPlayers, int index)
    {
        if (index >= 0 && index < foundPlayers.Count)
        {
            Item player = foundPlayers[index];
            /*Debug.Log("Player at index " + index + ": " + player.name);
            Debug.Log("  Planet Name: " + player.resources.Planet_Name);
            Debug.Log("  Position: (" + player.resources.X_pos + ", " + player.resources.Y_pos + ")");
            Debug.Log("  Wood: " + player.resources.Wood);
            Debug.Log("  Honey: " + player.resources.Honey);
            Debug.Log("  Gold: " + player.resources.Gold);
            Debug.Log("  Ice: " + player.resources.Ice);
            Debug.Log("  Last Time (Unix as String): " + player.resources.LastTime);*/
            main_text.text = "Save at " + player.resources.LastTime;
            small_text.text = "Planet Name: " + player.resources.Planet_Name + '\n' + "Wood: " + player.resources.Wood;
        }
        else
        {
            Debug.LogWarning("Index out of range: " + index);
        }
    }
}

// Вспомогательный класс для десериализации массива JSON
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        // Оборачиваем массив в объект
        string wrappedJson = "{\"items\":" + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(wrappedJson);
        return wrapper.items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }
}