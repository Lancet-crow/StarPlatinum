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
            stringChars[i] = chars[Random.Range(0, chars.Length)];
        }
        return new string(stringChars);
    }

    public void UpdateInputFieldAndScrollbars()
    {
        // ��������� ��������� ������ ������ 10 ��������
        worldKeyInputField.text = GenerateRandomString(Random.Range(1, 7));

        // ��������� ��������� �������� ��� ���� Scrollbar
        foreach (GameObject scrollbarObject in scrollbars)
        {
            Scrollbar scrollbar = scrollbarObject.GetComponent<Scrollbar>();
            if (scrollbar != null)
            {
                scrollbar.value = Random.Range(0f, 1f);
                //Debug.Log(scrollbar.value);
            }
        }
    }

    public void ShowMainMenu()
    {
        username = usernameInputField.text;
        password = passwordInputField.text;
        Debug.Log(username+'+'+ password);
        // ���� ���� ���� ���� ���������� ��������� ���������, �������� �����
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

    /// <summary>
    /// <para>�������� ��� ������ ������ � ��������� ������ ��������� ����.</para>
    /// </summary>
    public void StartNewGame()
    {
        SaveManager.Instance.ClearAllVariables();
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

    /// <summary>
    /// <para>��������� �������� ����� ����, ������������ �������� ������ � ��������.</para>
    /// <para>����� �������� ����� �� 90% ���������� �������� ����� ���� � ������� �������, ������������� ���������� <paramref name="timeToWaitInSeconds"/></para>
    /// <para>�������� <paramref name="timeToWaitInSeconds"/> �� ��������� - 5 ������</para>
    /// </summary>
    /// <param name="timeToWaitInSeconds">����� � ��������, � ������� �������� ������ �������� ����� ����� ������������</param>
    private IEnumerator LoadGameAsync(float timeToWaitInSeconds = 5f)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        asyncLoad.allowSceneActivation = false;
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }
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

        LoadGame();
    }
    ///<summary>
    ///<para>���� � ���� �������� ����������, �� ��������� ��������� ����� � ���� �����������</para>
    ///<para>���� � ���� ������ �� ��������, ��������� ��������� ���� ������ ����� ����</para>
    ///</summary>
    ///<remarks>
    ///<para>����������:</para>
    ///<para>1) <see cref="selectedSaveSlot"/> �� ������ ���� ����� null(����� ����� �� ���������)</para>
    ///<para>2) �����������, ������ ���� ������ ���� ����������(��������� ������ ������ � ����)</para>
    ///</remarks>
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
                LoadGame();
            }
        }
    }
    ///<summary>
    ///<para>���� � ���� �������� ����������, �� ��������� ������� ���� ���������� � ��������� ��������� �����</para>
    ///<para>���� � ���� ������ �� ��������, �� ��������� �����������, �� ������ �� ��������</para>
    ///</summary>
    ///<remarks>
    ///<para>����������:</para>
    ///<para>1) <see cref="selectedSaveSlot"/> �� ������ ���� ����� null(����� ����� �� ���������)</para>
    ///<para>2) �����������, ������ ���� ������ ���� ����������(��������� ������ ������ � ����)</para>
    ///</remarks>
    public void DeleteSelectedSaveSlot()
    {
        if (selectedSaveSlot)
        {
            newSaveID = selectedSaveSlot.transform.GetSiblingIndex();
            SaveManager.Instance.DeleteFile(newSaveID);
            selectedSaveSlot.GetComponent<SetSaveButtonValues>().UpdateText();
        }
    }

    /// <summary>
    /// ��������� ������� �������� ����: ��������� ������ �������� � ���������������� �������� ������� �����
    /// </summary>
    public void LoadGame()
    {
        //Debug.Log("Starting game with THE WORLD!! key: " + worldKey);
        ShowLoadingScreen();
        StartCoroutine(LoadGameAsync());
    }

    /// <summary>
    /// ����� ��������� ��� ������(��������, �������� ����, ������ ���������� � ��������� ����) � �������� ������ ��������
    /// </summary>
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

    /// <summary>
    /// <para>������������� ��������� ���� ��� �������� <see cref="selectedSaveSlot"/> </para>
    /// <para>���������� ��������� � ���������� �������(�� ���) �����</para>
    /// </summary>
    /// <param name="slot">������ �����, �� �������� ������� �����</param>
    public void SelectSaveSlot(GameObject slot)
    {
        GameObject.FindGameObjectWithTag("TrueSelect").GetComponent<SelectSlotSave>().Selection = slot.transform;
        GameObject.FindGameObjectWithTag("TrueSelect").GetComponent<SelectSlotSave>().SetPos();
        selectedSaveSlot = slot;
    }
}