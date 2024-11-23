using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Text;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject worldGenerationPanel;
    public GameObject settingsPanel;

    public TextMeshProUGUI worldKeyInputField;
    public TextMeshProUGUI usernameInputField;
    public Dropdown languageDropdown;

    public static int worldKey = 0;

    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        worldGenerationPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void StartNewGame()
    {
        mainMenuPanel.SetActive(false);
        worldGenerationPanel.SetActive(true);
    }

    public void ContinueGame()
    {
        // Логика продолжения игры
        Debug.Log("Continue Game");
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }

    public void StartGameWithWorldKey()
    {
        Debug.Log(worldKeyInputField.text);
        worldKey = StringToNumberConverter.ConvertStringToNumbers(worldKeyInputField.text);
        if (worldKey == 0)
        {
            worldKey = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }
        Debug.Log("Starting game with world key: " + worldKey);

        // Здесь вы можете сохранить ключ мира или использовать его для инициализации игры

        // Загружаем сцену TestPlanet
        SceneManager.LoadScene("TestPlanet");
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void CancelWorldGeneration()
    {
        worldGenerationPanel.SetActive(false);
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