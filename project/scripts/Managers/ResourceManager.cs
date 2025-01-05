using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������, �������������� ��� �������������� � ��������� � ����
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
        // ���� � ���������� ���� ������ � ��������, �� ��������� ���� �������� ������� ������
        if (SaveManager.Instance.workersState.Count > 0)
        {
            workersAmount = SaveManager.Instance.workersState;
        }
    }

    /// <summary>
    /// ���� �������� � ����
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
    /// ��������� ��������, ������� ���� � ������
    /// </summary>
    public SerializedDictionary<Resource, int> resourceStorage;
    
    /// <summary>
    /// ��������� ������� � ����
    /// </summary>
    public enum WorkerState
    {
        Free,
        Busy
    }
    /// <summary>
    /// �������-���������, �������� ���������� �������, ����������� � ����� �� ������� ���������
    /// </summary>
    public SerializedDictionary<WorkerState, int> workersAmount;

    /// <summary>
    /// ��������� ��������� ������� � ���������
    /// </summary>
    /// <param name="amount">���������� ����������� �������</param>
    public void AddWorkers(int amount)
    {
        workersAmount[WorkerState.Free] += amount;
        GameManager.Instance.UpdateWorkersText();
    }
    /// <summary>
    /// ��������� ������� �� "����������" ��������� � "�������"
    /// </summary>
    /// <param name="amount">���������� ����������� �������</param>
    public void BusyWorkers(int amount)
    {
        workersAmount[WorkerState.Free] -= amount;
        workersAmount[WorkerState.Busy] += amount;
        GameManager.Instance.UpdateWorkersText();
    }
    /// <summary>
    /// ��������� ������� �� "��������" ��������� � "���������"
    /// </summary>
    /// <param name="amount">���������� ����������� �������</param>
    public void FreeWorkers(int amount)
    {
        workersAmount[WorkerState.Free] += amount;
        workersAmount[WorkerState.Busy] -= amount;
        GameManager.Instance.UpdateWorkersText();
    }
    /// <summary>
    /// ��������� ���������� <paramref name="amount"/> ������� <paramref name="resource"/> � ��������� <see cref="resourceStorage"/>
    /// </summary>
    /// <param name="resource">��� ������������ �������</param>
    /// <param name="amount">���������� ������������ �������</param>
    public void AddAmount(Resource resource, int amount)
    {
        resourceStorage[resource] += amount;
        GameManager.Instance.UpdateResourceTexts();
    }
    /// <summary>
    /// �������� ���������� <paramref name="amount"/> ������� <paramref name="resource"/> �� ��������� <see cref="resourceStorage"/>
    /// </summary>
    /// <param name="resource">��� ����������� �������</param>
    /// <param name="amount">���������� ����������� �������</param>
    public void TakeAmount(Resource resource, int amount)
    {
        resourceStorage[resource] -= amount;
        GameManager.Instance.UpdateResourceTexts();
    }
    /// <summary>
    /// ��������������� �����, ����������� ������ ����� ��������� �������� �� ��������� �� ���� �����
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
    /// ���������, ���� �� ����������� ���������� ������� � ���������
    /// </summary>
    /// <param name="resource">��� ������� ��� ��������</param>
    /// <param name="amount">���������� ������� � ���������</param>
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
    /// ���������, ������� �� ��������� ������� ��� ��������� ���������
    /// </summary>
    /// <param name="buildingComponent">��������� ���������, ������� ������ ���� �����������</param>
    public bool HasEnoughWorkers(BuildingComponent buildingComponent)
    {
        return workersAmount[WorkerState.Free] >= buildingComponent.workersAmount;
    }
}
