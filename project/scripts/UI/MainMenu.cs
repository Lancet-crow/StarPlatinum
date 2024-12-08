using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject worldGenerationPanel;
    public GameObject settingsPanel;
    public GameObject ContinueGamePanel;
    public GameObject Loading;
    public GameObject selectedSaveSlot;
    public GameObject authorMenu;
    public Animator logoAnimator;

    public TextMeshProUGUI worldKeyInputField;
    public TextMeshProUGUI usernameInputField;
    public TextMeshProUGUI passwordInputField;
    public TextMeshProUGUI loadInputField;
    public Dropdown languageDropdown;

    public List<GameObject> scrollbars;

    public static string worldKey = "";
    public static int newSaveID = 0;
    public static string username = "";
    public static string password = "";

    void Start()
    {
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
        username = usernameInputField.text;
        password = passwordInputField.text;
        Debug.Log(username+'+'+ password);
        if (GameObject.FindGameObjectWithTag("TrueSelect")) GameObject.FindGameObjectWithTag("TrueSelect").GetComponent<SelectSlotSave>().SetFalse();
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
        SaveManager.Instance.howManyBuildingsString = null;
        SaveManager.Instance.resourceString = "";
        SaveManager.Instance.saveCode = "";
        SaveManager.Instance.workersState = new();
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

    private IEnumerator LoadGameAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        asyncLoad.allowSceneActivation = false;
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }
        float timeToWaitInSeconds = 5f;
        float estimatedTime = 0f;
        while (estimatedTime < timeToWaitInSeconds)
        {
            estimatedTime += Time.deltaTime;
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
    }

    public void StartGameWithWorldKey()
    {
        ShowLoadingScreen();
        //Debug.Log(worldKeyInputField.GetParsedText().ToString() != "");
        if (worldKeyInputField.GetParsedText().ToString() != "")
        {
            worldKey = StringToNumberConverter.ConvertStringToNumbers(worldKeyInputField.text).ToString();
        }
        else
        {
            worldKey = Random.Range(-99999, 99999).ToString();
            Debug.Log(worldKey + "=-=-=-=-=-=-=-");
        }
        Debug.Log(worldKey + "=======");
        worldKey = worldKey.ToString() +'|' + generate_field1.value_tree.ToString() + '|' + generate_field1.value_rock.ToString() + '|' + generate_field1.value_Pb.ToString() + '|' + generate_field1.value_ice.ToString() + '|' + generate_field1.value_water.ToString() + '|' + generate_field1.value_emptiness.ToString() + "|;";
        Debug.Log("Starting game with THE WORLD!! key: " + worldKey);

        StartCoroutine(LoadGameAsync());
    }

    public void OpenSelectedSaveSlot()
    {
        if (selectedSaveSlot)
        {
            newSaveID = selectedSaveSlot.transform.GetSiblingIndex();
            SaveManager.Instance.LoadFile(newSaveID);
            if (SaveManager.Instance.saveCode == "")
            {
                StartNewGame();
                print("Starting new game...");
            }
            else
            {
                worldKey = SaveManager.Instance.saveCode;
                print(worldKey);
                ShowLoadingScreen();
                StartCoroutine(LoadGameAsync());
            }
        }
    }

    public void DeleteSelectedSaveSlot()
    {
        if (selectedSaveSlot)
        {
            newSaveID = selectedSaveSlot.transform.GetSiblingIndex();
            SaveManager.Instance.DeleteFile(newSaveID);
            selectedSaveSlot.GetComponent<SetSaveButtonValues>().UpdateText();
        }
    }

    public void LoadGame()
    {
        //Debug.Log("Starting game with THE WORLD!! key: " + worldKey);
        ShowLoadingScreen();
        worldKey = loadInputField.text;
        Debug.Log("Starting game with THE WORLD!! key: " + worldKey);
        StartCoroutine(LoadGameAsync());
    }

    public void ShowLoadingScreen()
    {
        Loading.SetActive(true);
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        ContinueGamePanel.SetActive(false);
        worldGenerationPanel.SetActive(false);
    }

    public void ExitToMainMenu()
    {
        ShowMainMenu();
    }

    public void CancelWorldGeneration()
    {
        worldGenerationPanel.SetActive(false);
        Loading.SetActive(false);
        ShowMainMenu();
    }

    public void SaveSettings()
    {
        string username = usernameInputField.GetComponent<TextMeshProUGUI>().text;
        /*
        string language = languageDropdown.options[languageDropdown.value].text;
        Debug.Log("Settings saved. Username: " + username + ", Language: " + language);*/
        settingsPanel.SetActive(false);
        ShowMainMenu();
    }

    public void CancelSettings()
    {
        settingsPanel.SetActive(false);
        ShowMainMenu();
    }

    public void OpenAuthorMenu()
    {
        authorMenu.SetActive(true);
    }

    public void CloseAuthorMenu()
    {
        authorMenu.SetActive(false);
    }

    public void SelectSaveSlot(GameObject slot)
    {
        GameObject.FindGameObjectWithTag("TrueSelect").GetComponent<SelectSlotSave>().Selection = slot.transform;
        GameObject.FindGameObjectWithTag("TrueSelect").GetComponent<SelectSlotSave>().SetPos();
        selectedSaveSlot = slot;
    }
}