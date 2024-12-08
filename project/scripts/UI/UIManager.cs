using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject minimapPanel;
    public GameObject buildingPanel;
    public GameObject finalPanel;

    public BuildingSystem.BuildingType? currentBuildingTypeChoice = null;
    public enum ModeState
    {
        defaultMode,
        buildingMode,
        destroyingMode
    }
    public ModeState modeState;

    [SerializeField] private SerializedDictionary<ResourceManager.Resource, TMPro.TextMeshProUGUI> resourceTexts;
    [SerializeField] private SerializedDictionary<string, TMPro.TextMeshProUGUI> otherTexts;

    public static UIManager Instance { get; private set; }
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
    }

    private void Start()
    {
        Time.timeScale = 1f;
        UpdateResourceTexts();
        UpdateWorkersText();
    }

    // Update is called once per frame
    void Update()
    {
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

    public void SetCurrentBuildingTypeChoice(BuildingSystem.BuildingType buildingType)
    {
        currentBuildingTypeChoice = buildingType;
    }
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

    public void ChangeModeState(ModeState mode)
    {
        modeState = mode != modeState ? mode : ModeState.defaultMode; // Если этот режим уже включён, то вернуться к дефолтному
        ReactToModeStateChange();
    }

    public void ChangeModeState(string modeString)
    {
        var mode = (ModeState)System.Enum.Parse(typeof(ModeState), modeString);
        ChangeModeState(mode);
    }

    public void ChangeModeState(int modeInt)
    {
        var mode = (ModeState)modeInt;
        ChangeModeState(mode);
    }

    public void ReactToModeStateChange()
    {
        buildingPanel.SetActive(modeState == ModeState.buildingMode);
        currentBuildingTypeChoice = null;
    }

    public void UpdateResourceTexts()
    {
        foreach (KeyValuePair<ResourceManager.Resource, TMPro.TextMeshProUGUI> keyValuePair in resourceTexts)
        {
            keyValuePair.Value.text = ResourceManager.Instance.resourceStorage[keyValuePair.Key].ToString();
        }
    }

    public void UpdateHousesAmountText()
    {
        otherTexts["housesAmountText"].text = BuildingSystem.Instance.buildingAmounts[BuildingSystem.BuildingType.LivingHouse].ToString();
    }

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
