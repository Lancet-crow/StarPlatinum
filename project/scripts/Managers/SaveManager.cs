using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
	public string saveCode;
	public string resourceString;
	public SerializedDictionary<ResourceManager.WorkerState, int> workersState;
	public string[] howManyBuildingsString;

	public static SaveManager Instance { get; private set; }
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

	/// <summary>
	/// ���������� ��� ����������� �������� � ���� ����������.
	/// <list type="bullet">
	/// <listheader>����������� ��������:</listheader>
	/// <item>��� ���� � ��������� ����� � �� (<see cref="saveCode"/>)</item>
	/// <item>������ �������� ������ � ���� (<see cref="EncodeResourceString"/>)</item>
	/// <item>������ ��������� �������(��������� � �������) (<see cref="ResourceManager.workersAmount"/>)</item>
	/// <item>������ � ����������� ��������� �������� � ���� (<see cref="BuildingSystem.buildingAmounts"/>)</item>
	/// </list>
	/// </summary>
	/// <remarks>
	/// ���� ���� ��� ����������, �� ���������������� ������
	/// </remarks>
	/// <param name="id">�����, ��� ������� ����� ������� ����(��������: ����� ����� ����������)</param>
	public void SaveFile(int id)
	{
		string destination = Application.persistentDataPath + $"/save{id}.dat";
        using StreamWriter sw = File.CreateText(destination);
        sw.WriteLine(saveCode);
		sw.WriteLine(EncodeResourceString());
		sw.WriteLine($"{ResourceManager.Instance.workersAmount[ResourceManager.WorkerState.Free]} {ResourceManager.Instance.workersAmount[ResourceManager.WorkerState.Busy]}");
		var howManyBuildings = string.Join(" ", BuildingSystem.Instance.buildingAmounts.Values);
		sw.WriteLine(howManyBuildings);
		sw.Close();
    }
	/// <summary>
	/// ��������� ���� ������������ ���������� � �������������� ��� � ����������. ��� ����������, ����������� � ����, �������� ���: <seealso cref="SaveFile"/>
	/// </summary>
	/// <param name="id">�����, �� �������� ����� �������� ����(��������: ����� ����� ����������)</param>
	public void LoadFile(int id)
	{
		string destination = Application.persistentDataPath +  $"/save{id}.dat";

		if (!File.Exists(destination)) {
			Debug.LogWarning($"Save File with id {id} not found");
			saveCode = "";
			return;
		}
        else
        {
			using StreamReader sr = new(destination);
			saveCode = sr.ReadLine();
			resourceString = sr.ReadLine();
			var workers = sr.ReadLine().Split(" ");
			workersState[ResourceManager.WorkerState.Free] = int.Parse(workers[0]);
			workersState[ResourceManager.WorkerState.Busy] = int.Parse(workers[1]);
			howManyBuildingsString = sr.ReadLine().Split();
			sr.Close();
		}
	}
	/// <summary>
	/// ������� ���� ���������� ��� ������� <paramref name="id"/>.
	/// <para>���� ID ���������� �� ��� ������, ��� ����� � ����� ID �� ����������, ��������� ������� � ���� ��������������� Warning.</para>
	/// </summary>
	/// <param name="id">�����, �� �������� ����� �������� ����(��������: ����� ����� ����������). �� ��������� ����� -1, ��� �������������� ��������� ��������</param>
	public void DeleteFile(int id = -1)
    {
		if (id == -1)
        {
			Debug.LogWarning($"Id has not been specified");
			return;
        }
		string destination = Application.persistentDataPath + $"/save{id}.dat";

		if (!File.Exists(destination))
		{
			Debug.LogWarning($"Save File with id {id} not found");
			saveCode = "";
			return;
		}
		else
		{
			File.Delete(destination);
			if (!File.Exists(destination))
            {
				Debug.Log($"Save file with path {destination} has been deleted");
            }
		}
	}

	/// <summary>
	/// ������� ��� ���������� ��� �������� ������ ����������.
	/// <list type="bullet">
	/// <listheader>��������� ����������:</listheader>
	/// <item><see cref="saveCode"/></item>
	/// <item><see cref="resourceString"/></item>
	/// <item><see cref="howManyBuildingsString"/></item>
	/// <item><see cref="workersState"/></item>
	/// </list>
	/// </summary>
	public void ClearAllVariables()
    {
		howManyBuildingsString = null;
		resourceString = "";
		saveCode = "";
		workersState = new();
	}

	/// <summary>
	/// ����������� �������� ���������� �������� ������, ���������� � <see cref="ResourceManager"/>, � ������ ��� ������������ ����������
	/// </summary>
	/// <returns>������ �� ���������� �������� ������</returns>
	public string EncodeResourceString()
    {
		string resourceString = "";
		foreach(KeyValuePair<ResourceManager.Resource, int> keyValuePair in ResourceManager.Instance.resourceStorage)
        {
			resourceString += $"{keyValuePair.Key}={keyValuePair.Value};";
        }
		return resourceString;
    }

	/// <summary>
	/// ����������� <see cref="resourceString"/> � �������� ���������� <see cref="ResourceManager"/> ��� ����������� �������������
	/// </summary>
	public void DecodeResourceString()
    {
		if (resourceString.Length > 0)
        {
			var split_resources = resourceString.Split(";");
			foreach (string keyValuePair in split_resources)
			{
				if (keyValuePair == "")
				{
					break;
				}
				var split_pair = keyValuePair.Split("=");
				var resource = (ResourceManager.Resource)System.Enum.Parse(typeof(ResourceManager.Resource), split_pair[0]);
				var amount = int.Parse(split_pair[1]);
				ResourceManager.Instance.resourceStorage[resource] = amount;
			}
		}
    }
}
