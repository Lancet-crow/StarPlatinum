using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject minimapPanel;
    public GameObject buildingPanel;
    public GameObject finalPanel;

    /// <summary>
    /// ����������, ������� �������� ��� ���������, ��������� ������� � ������ �������������
    /// <para>������������ ��� ������ <see cref="BuildingSystem.PlaceABuilding(BuildingSystem.BuildingType?, GameObject)"/></para>
    /// </summary>
    public BuildingSystem.BuildingType? currentBuildingTypeChoice = null;
    /// <summary>
    /// �������, �������� ��������� ���� �� ������� ������ ��� ��������� ��������
    /// </summary>
    [SerializeField] private SerializedDictionary<ResourceManager.Resource, TMPro.TextMeshProUGUI> resourceTexts;
    /// <summary>
    /// �������, �������� ����� ��������� ����. � ��� ����� �������� ������ �� �����-������
    /// </summary>
    [SerializeField] private SerializedDictionary<string, TMPro.TextMeshProUGUI> otherTexts;

    [SerializeReference] public IPlayerState currentGameMode;

    public static GameManager Instance { get; private set; }
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
        currentGameMode = new NormalMode();
    }

    private void Start()
    {
        Time.timeScale = 1f;
        UpdateResourceTexts();
        UpdateWorkersText();
    }

    public void ChangeGameMode(IPlayerState newGameMode)
    {
        currentGameMode?.Exit();
        currentGameMode = newGameMode;
        currentGameMode?.Enter();
    }
    public void ChangeGameMode(string newGameModeString)
    {
        System.Type newGMType = TypeSearcher.FindType(newGameModeString);
        var newGameMode = (IPlayerState)System.Activator.CreateInstance(newGMType);
        ChangeGameMode(newGameMode);
    }

    void Update()
    {
        // ��������� ������� ������ ������
        currentGameMode?.HandleInput();
        currentGameMode?.Update();
        // ������� ���� �����, ���� ��� ��� �� ������� � ������ ������ Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenu.activeInHierarchy)
            {
                OpenPauseMenu();
            }
            else
            {
                ClosePauseMenu();
            }
        }
        // �������� ������� ��� ������
        CheckFinaleConditions();
    }


    /// <summary>
    /// ��������� ����� ����, ���� ��������� ��� �������
    /// </summary>
    public void  CheckFinaleConditions()
    {
        if (ResourceManager.Instance.resourceStorage[ResourceManager.Resource.Electricity] > 100 &&
            BuildingSystem.Instance.buildingAmounts[BuildingSystem.BuildingType.LivingHouse] > 40 &&
            !finalPanel.activeInHierarchy)
        {
            finalPanel.SetActive(true);
        }
    }

    private void OnApplicationQuit()
    {
        generate_field1.Instance.SaveAndExit();
    }

    /// <summary>
    /// ���������� ����� ��������� ���� <paramref name="buildingType"/> � ����������� ���������� ��� ����������� � �������������
    /// <param name="buildingType">��� ��������� ���������</param>
    public void SetCurrentBuildingTypeChoice(BuildingSystem.BuildingType buildingType)
    {
        currentBuildingTypeChoice = buildingType;
    }
    /// <summary>
    /// ��������������� ����� ��� <see cref="SetCurrentBuildingTypeChoice(BuildingSystem.BuildingType)"/>
    /// <para>��������� ������ <see cref="BuildingSystem.BuildingType"/> ������ � ��� �� ���������</para>
    /// </summary>
    /// <param name="buildingTypeString">������ � ��������� ����� ���������. ������ ��������� �� ��������� � <see cref="BuildingSystem.BuildingType"/></param>
    public void SetCurrentBuildingTypeChoice(string buildingTypeString)
    {
        var buildingType = (BuildingSystem.BuildingType)System.Enum.Parse(typeof(BuildingSystem.BuildingType), buildingTypeString);
        SetCurrentBuildingTypeChoice(buildingType);
    }
    public void SetCurrentBuildingTypeChoice(int buildingTypeInt)
    {
        var buildingType = (BuildingSystem.BuildingType)buildingTypeInt;
        SetCurrentBuildingTypeChoice(buildingType);
    }

    /// <summary>
    /// ��������� �������� �������� �� ������� ������
    /// </summary>
    public void UpdateResourceTexts()
    {
        foreach (KeyValuePair<ResourceManager.Resource, TMPro.TextMeshProUGUI> keyValuePair in resourceTexts)
        {
            keyValuePair.Value.text = ResourceManager.Instance.resourceStorage[keyValuePair.Key].ToString();
        }
    }

    /// <summary>
    /// <i><b>DEPRECATED. ����� �� ������������.</b></i>
    /// ��������� �������� ���������� ����� �� ������
    /// </summary>
    public void UpdateHousesAmountText()
    {
        otherTexts["housesAmountText"].text = BuildingSystem.Instance.buildingAmounts[BuildingSystem.BuildingType.LivingHouse].ToString();
    }

    /// <summary>
    /// ��������� �������� ������� �� ������� ������
    /// </summary>
    public void UpdateWorkersText()
    {
        otherTexts["allWorkersText"].text = ResourceManager.Instance.workersAmount.Sum(x => x.Value).ToString();
        otherTexts["freeWorkersText"].text = ResourceManager.Instance.workersAmount[ResourceManager.WorkerState.Free].ToString();
        otherTexts["busyWorkersText"].text = ResourceManager.Instance.workersAmount[ResourceManager.WorkerState.Busy].ToString();
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
