using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Скрипт, обрабатывающий все взаимодействия с ресурсами в игре
/// </summary>
public class ResourceManager: MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
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
        SaveManager.Instance.DecodeResourceString();
        // Если в сохранении была строка с рабочими, то программа берёт значения рабочих оттуда
        if (SaveManager.Instance.workersState.Count > 0)
        {
            workersAmount = SaveManager.Instance.workersState;
        }
    }

    /// <summary>
    /// Типы ресурсов в игре
    /// </summary>
    public enum Resource
    {
        HoneyIngot,
        Fish,
        Wood,
        Steel,
        Electricity
    }
    /// <summary>
    /// Хранилище ресурсов, которые есть у игрока
    /// </summary>
    public SerializedDictionary<Resource, int> resourceStorage;
    
    /// <summary>
    /// Состояния рабочих в игре
    /// </summary>
    public enum WorkerState
    {
        Free,
        Busy
    }
    /// <summary>
    /// Словарь-хранилище, хранящий количество рабочих, находящихся в одном из игровых состояний
    /// </summary>
    public SerializedDictionary<WorkerState, int> workersAmount;

    /// <summary>
    /// Добавляет свободных рабочих в хранилище
    /// </summary>
    /// <param name="amount">Количество добавляемых рабочих</param>
    public void AddWorkers(int amount)
    {
        workersAmount[WorkerState.Free] += amount;
        GameManager.Instance.UpdateWorkersText();
    }
    /// <summary>
    /// Переводит рабочих из "свободного" состояния в "занятое"
    /// </summary>
    /// <param name="amount">Количество переводимых рабочих</param>
    public void BusyWorkers(int amount)
    {
        workersAmount[WorkerState.Free] -= amount;
        workersAmount[WorkerState.Busy] += amount;
        GameManager.Instance.UpdateWorkersText();
    }
    /// <summary>
    /// Переводит рабочих из "занятого" состояния в "свободное"
    /// </summary>
    /// <param name="amount">Количество переводимых рабочих</param>
    public void FreeWorkers(int amount)
    {
        workersAmount[WorkerState.Free] += amount;
        workersAmount[WorkerState.Busy] -= amount;
        GameManager.Instance.UpdateWorkersText();
    }
    /// <summary>
    /// Добавляет количество <paramref name="amount"/> ресурса <paramref name="resource"/> в хранилище <see cref="resourceStorage"/>
    /// </summary>
    /// <param name="resource">Тип добавляемого ресурса</param>
    /// <param name="amount">Количество добавляемого ресурса</param>
    public void AddAmount(Resource resource, int amount)
    {
        resourceStorage[resource] += amount;
        GameManager.Instance.UpdateResourceTexts();
    }
    /// <summary>
    /// Отнимает количество <paramref name="amount"/> ресурса <paramref name="resource"/> из хранилища <see cref="resourceStorage"/>
    /// </summary>
    /// <param name="resource">Тип отнимаемого ресурса</param>
    /// <param name="amount">Количество отнимаемого ресурса</param>
    public void TakeAmount(Resource resource, int amount)
    {
        resourceStorage[resource] -= amount;
        GameManager.Instance.UpdateResourceTexts();
    }
    /// <summary>
    /// Вспомогательный метод, позволяющий отнять сразу несколько ресурсов из хранилища за один вызов
    /// </summary>
    /// <param name="resourceDictionary"></param>
    public void TakeAmount(Dictionary<Resource, int> resourceDictionary)
    {
        if (HasEnoughResources(resourceDictionary))
        {
            foreach (KeyValuePair<Resource, int> keyValuePair in resourceDictionary)
            {
                TakeAmount(keyValuePair.Key, keyValuePair.Value);
            }
        }
    }

    /// <summary>
    /// Проверяет, есть ли необходимое количество ресурса в хранилище
    /// </summary>
    /// <param name="resource">Тип ресурса для проверки</param>
    /// <param name="amount">Количество ресурса в хранилище</param>
    public bool HasEnoughResources(Resource resource, int amount)
    {
        return resourceStorage[resource] >= amount;
    }

    public bool HasEnoughResources(Dictionary<Resource, int> resourceDictionary)
    {
        bool hasEnoughResources = true;
        foreach (KeyValuePair<Resource, int> keyValuePair in resourceDictionary)
        {
            if (resourceStorage[keyValuePair.Key] < keyValuePair.Value)
            {
                hasEnoughResources = false;
                break;
            }
        }
        return hasEnoughResources;
    }
    /// <summary>
    /// Проверяет, хватает ли свободных рабочих для установки постройки
    /// </summary>
    /// <param name="buildingComponent">Компонент постройки, которая должна быть установлена</param>
    public bool HasEnoughWorkers(BuildingComponent buildingComponent)
    {
        return workersAmount[WorkerState.Free] >= buildingComponent.workersAmount;
    }
}
