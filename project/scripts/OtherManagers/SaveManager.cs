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
	/// Записывает все необходимые значения в файл сохранения.
	/// <list type="bullet">
	/// <listheader>Сохраняемые значения:</listheader>
	/// <item>Сид мира и изменённые тайлы в нём (<see cref="saveCode"/>)</item>
	/// <item>Строка ресурсов игрока в мире (<see cref="EncodeResourceString"/>)</item>
	/// <item>Строка состояния рабочих(свободных и занятых) (<see cref="ResourceManager.workersAmount"/>)</item>
	/// <item>Строка с количеством различных построек в мире (<see cref="BuildingSystem.buildingAmounts"/>)</item>
	/// </list>
	/// </summary>
	/// <remarks>
	/// Если файл уже существует, он перезаписывается заново
	/// </remarks>
	/// <param name="id">Номер, под которым будет сохранён файл(стандарт: номер слота сохранения)</param>
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
	/// Загружает файл необходимого сохранения и расшифровывает его в переменные. Все переменные, сохраняемые в файл, смотреть тут: <seealso cref="SaveFile"/>
	/// </summary>
	/// <param name="id">Номер, по которому будет искаться файл(стандарт: номер слота сохранения)</param>
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
	/// Удаляет файл сохранения под номером <paramref name="id"/>.
	/// <para>Если ID сохранения не был указан, или файла с таким ID не существует, программа запишет в логи соответствующий Warning.</para>
	/// </summary>
	/// <param name="id">Номер, по которому будет искаться файл(стандарт: номер слота сохранения). По умолчанию равен -1, для предотвращения случайных удалений</param>
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
	/// Очищает все переменные для создания нового сохранения.
	/// <list type="bullet">
	/// <listheader>Очищаемые переменные:</listheader>
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
	/// Преобразует значения переменных ресурсов игрока, хранящихся в <see cref="ResourceManager"/>, в строку для последующего сохранения
	/// </summary>
	/// <returns>Строка со значениями ресурсов игрока</returns>
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
	/// Преобразует <see cref="resourceString"/> в значения переменных <see cref="ResourceManager"/> для дальнейшего использования
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
