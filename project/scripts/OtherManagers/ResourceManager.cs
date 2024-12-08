using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

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
        if (SaveManager.Instance.workersState.Count > 0)
        {
            workersAmount = SaveManager.Instance.workersState;
        }
    }
    public enum Resource
    {
        HoneyIngot,
        Fish,
        Wood,
        Steel,
        Electricity
    }

    public SerializedDictionary<Resource, int> resourceStorage;
    
    public enum WorkerState
    {
        Free,
        Busy
    }

    public SerializedDictionary<WorkerState, int> workersAmount;

    public void AddWorkers(int amount)
    {
        workersAmount[WorkerState.Free] += amount;
        UIManager.Instance.UpdateWorkersText();
    }

    public void BusyWorkers(int amount)
    {
        workersAmount[WorkerState.Free] -= amount;
        workersAmount[WorkerState.Busy] += amount;
        UIManager.Instance.UpdateWorkersText();
    }

    public void FreeWorkers(int amount)
    {
        workersAmount[WorkerState.Free] += amount;
        workersAmount[WorkerState.Busy] -= amount;
        UIManager.Instance.UpdateWorkersText();
    }

    public void AddAmount(Resource resource, int amount)
    {
        resourceStorage[resource] += amount;
        UIManager.Instance.UpdateResourceTexts();
    }

    public void TakeAmount(Resource resource, int amount)
    {
        resourceStorage[resource] -= amount;
        UIManager.Instance.UpdateResourceTexts();
    }

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

    public bool HasEnoughWorkers(BuildingComponent buildingComponent)
    {
        return workersAmount[WorkerState.Free] >= buildingComponent.workersAmount;
    }
}
