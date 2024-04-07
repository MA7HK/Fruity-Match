using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class SaveData
{
	public bool[] _isActive;
	public int[] _highScores;
	public int[] _stars;
}

public class GameData : MonoBehaviour
{
	public static GameData gameData;
	public SaveData _saveData;

	void Awake()
	{
		if (gameData == null) 
		{
			gameData = this;
			DontDestroyOnLoad(this.gameObject);
		} 
		else 
		{
			Destroy(this.gameObject);
		}
		Load();
	}

	public void Save()
	{
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath + "/gamedata.ma7hk", FileMode.OpenOrCreate);

		SaveData data = new SaveData();
		data = _saveData;

		formatter.Serialize(file, data);
		
		file.Close();
		print("Saved");
	}

	public void Load()
	{
		if (File.Exists(Application.persistentDataPath + "/gamedata.ma7hk"))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/gamedata.ma7hk", FileMode.Open);

			_saveData = formatter.Deserialize(file) as SaveData;
			file.Close();
			print("Loaded");
		}
		else 
		{
			_saveData = new SaveData();
			_saveData._isActive = new bool[36];
			_saveData._stars = new int[36];
			_saveData._highScores = new int[36];
			_saveData._isActive[0] = true;
		}
	}

    private void OnApplicationQuit() => Save();
    private void OnDisable() => Save();
}
