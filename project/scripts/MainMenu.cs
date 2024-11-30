using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.Text;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject worldGenerationPanel;
    public GameObject settingsPanel;
    public GameObject ContinueGamePanel;
    public GameObject Loading;

    public TextMeshProUGUI worldKeyInputField;
    public TextMeshProUGUI usernameInputField;
    public TextMeshProUGUI loadInputField;
    public Dropdown languageDropdown;

    public List<GameObject> scrollbars;

    public static string worldKey = "";

    void Start()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<API_manager>().SendRequest("https://2025.nti-gamedev.ru/api/games/744e9e81-85be-4099-883c-e133b28e9a0e/players/", "POST", "{\"key\":\"value\"}");
        ShowMainMenu();
    }

    private string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_!()~{}<>";
        char[] stringChars = new char[length];
        for (int i = 0; i < length; i++)
        {
            stringChars[i] = chars[UnityEngine.Random.Range(0, chars.Length)];
        }
        return new string(stringChars);
    }

    public void UpdateInputFieldAndScrollbars()
    {
        // Генерация случайной строки длиной 10 символов
        worldKeyInputField.text = GenerateRandomString(UnityEngine.Random.Range(1, 7));

        // Установка случайных значений для всех Scrollbar
        foreach (GameObject scrollbarObject in scrollbars)
        {
            Scrollbar scrollbar = scrollbarObject.GetComponent<Scrollbar>();
            if (scrollbar != null)
            {
                scrollbar.value = UnityEngine.Random.Range(0f, 1f);
                //Debug.Log(scrollbar.value);
            }
        }
    }

    public void ShowMainMenu()
    {
        if (mainMenuPanel)
        {
            mainMenuPanel.SetActive(true);
            worldGenerationPanel.SetActive(false);
            settingsPanel.SetActive(false);
            ContinueGamePanel.SetActive(false);
            Loading.SetActive(false);
        }
    }

    public void StartNewGame()
    {
        mainMenuPanel.SetActive(false);
        worldGenerationPanel.SetActive(true);
        ContinueGamePanel.SetActive(false);
        Loading.SetActive(false);
        UpdateInputFieldAndScrollbars();
    }

    public void ContinueGame()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        ContinueGamePanel.SetActive(true);
        Loading.SetActive(false);
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        ContinueGamePanel.SetActive(false);
        Loading.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }

    public void StartGameWithWorldKey()
    {
        Loading.SetActive(true);
        //Debug.Log(worldKeyInputField.GetParsedText().ToString() != "");
        if (worldKeyInputField.GetParsedText().ToString() != "")
        {
            worldKey = StringToNumberConverter.ConvertStringToNumbers(worldKeyInputField.text).ToString();
        }
        else
        {
            worldKey = UnityEngine.Random.Range(-999, 999).ToString();
            Debug.Log(worldKey + "=-=-=-=-=-=-=-");
        }
        Debug.Log(worldKey + "=======");
        worldKey = worldKey.ToString() +'|' + generate_field1.value_tree.ToString() + '|' + generate_field1.value_rock.ToString() + '|' + generate_field1.value_Pb.ToString() + '|' + generate_field1.value_ice.ToString() + '|' + generate_field1.value_water.ToString() + '|' + generate_field1.value_emptiness.ToString() + "|;";
        Debug.Log("Starting game with THE WORLD!! key: " + worldKey);

        SceneManager.LoadScene("TestPlanet");
    }

    public void LoadGame()
    {
        //Debug.Log("Starting game with THE WORLD!! key: " + worldKey);
        Loading.SetActive(true);
        worldKey = loadInputField.text;
        Debug.Log("Starting game with THE WORLD!! key: " + worldKey);
        SceneManager.LoadScene("TestPlanet");
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void CancelWorldGeneration()
    {
        worldGenerationPanel.SetActive(false);
        Loading.SetActive(false);
        ShowMainMenu();
    }

    public void SaveSettings()
    {
        string username = usernameInputField.GetComponent<Text>().text;
        string language = languageDropdown.options[languageDropdown.value].text;
        Debug.Log("Settings saved. Username: " + username + ", Language: " + language);
        settingsPanel.SetActive(false);
        ShowMainMenu();
    }

    public void CancelSettings()
    {
        settingsPanel.SetActive(false);
        ShowMainMenu();
    }
}