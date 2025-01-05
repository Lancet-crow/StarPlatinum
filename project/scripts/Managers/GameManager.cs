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
    /// Переменная, которая содержит тип постройки, выбранный игроком в режиме строительства
    /// <para>Используется для вызова <see cref="BuildingSystem.PlaceABuilding(BuildingSystem.BuildingType?, GameObject)"/></para>
    /// </summary>
    public BuildingSystem.BuildingType? currentBuildingTypeChoice = null;
    /// <summary>
    /// Словарь, хранящий текстовые поля на главной панели для выбранных ресурсов
    /// </summary>
    [SerializeField] private SerializedDictionary<ResourceManager.Resource, TMPro.TextMeshProUGUI> resourceTexts;
    /// <summary>
    /// Словарь, хранящий любые текстовые поля. К ним можно получить доступ по ключу-строке
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
        // Обработка событий внутри режима
        currentGameMode?.HandleInput();
        currentGameMode?.Update();
        // Открыть меню паузы, если оно ещё не открыто и нажата кнопка Esc
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
        // Проверка условий для финала
        CheckFinaleConditions();
    }


    /// <summary>
    /// Запускает финал игры, если соблюдены все условия
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
    /// Закрепляет выбор постройки типа <paramref name="buildingType"/> в специальную переменную для дальнейшего её использования
    /// <param name="buildingType">Тип выбранной постройки</param>
    public void SetCurrentBuildingTypeChoice(BuildingSystem.BuildingType buildingType)
    {
        currentBuildingTypeChoice = buildingType;
    }
    /// <summary>
    /// Вспомогательный метод для <see cref="SetCurrentBuildingTypeChoice(BuildingSystem.BuildingType)"/>
    /// <para>Принимает вместо <see cref="BuildingSystem.BuildingType"/> строку с тем же значением</para>
    /// </summary>
    /// <param name="buildingTypeString">Строка с выбранным типом постройки. Должна совпадать со значением в <see cref="BuildingSystem.BuildingType"/></param>
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
    /// Обновляет значения ресурсов на главной панели
    /// </summary>
    public void UpdateResourceTexts()
    {
        foreach (KeyValuePair<ResourceManager.Resource, TMPro.TextMeshProUGUI> keyValuePair in resourceTexts)
        {
            keyValuePair.Value.text = ResourceManager.Instance.resourceStorage[keyValuePair.Key].ToString();
        }
    }

    /// <summary>
    /// <i><b>DEPRECATED. Нигде не используется.</b></i>
    /// Обновляет значение количества домов на панели
    /// </summary>
    public void UpdateHousesAmountText()
    {
        otherTexts["housesAmountText"].text = BuildingSystem.Instance.buildingAmounts[BuildingSystem.BuildingType.LivingHouse].ToString();
    }

    /// <summary>
    /// Обновляет значения рабочих на главной панели
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
