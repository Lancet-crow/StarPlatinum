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

    public TextMeshProUGUI worldKeyInputField;
    public TextMeshProUGUI usernameInputField;
    public Dropdown languageDropdown;

    public List<GameObject> scrollbars;

    public static int worldKey = 0;

    void Start()
    {
        ShowMainMenu();
    }

    private string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_=+!@#$%^&*()~{}<>";
        char[] stringChars = new char[length];
        for (int i = 0; i < length; i++)
        {
            stringChars[i] = chars[UnityEngine.Random.Range(0, chars.Length)];
        }
        return new string(stringChars);
    }

    public void UpdateInputFieldAndScrollbars()
    {
        // ��������� ��������� ������ ������ 10 ��������
        worldKeyInputField.text = GenerateRandomString(UnityEngine.Random.Range(1, 7));

        // ��������� ��������� �������� ��� ���� Scrollbar
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
        }
    }

    public void StartNewGame()
    {
        mainMenuPanel.SetActive(false);
        worldGenerationPanel.SetActive(true);
    }

    public void ContinueGame()
    {
        // ������ ����������� ����
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