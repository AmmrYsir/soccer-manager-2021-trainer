using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class ES2
{
	public static bool initialised;

	public static void Init()
	{
		Object.Instantiate(Resources.Load("ES2/ES2Init"));
	}

	public static void Save<T>(T param, string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save<T>(T param, string identifier, bool alloc = true)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag, alloc);
		eS2Writer.Save();
	}

	public static void Save<T>(T[] param, string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save<T>(T[] param, string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save<T>(T[,] param, string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save<T>(T[,] param, string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save2DArray<T>(T[,] param, string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save2DArray<T>(T[,] param, string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save<T>(T[,,] param, string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save<T>(T[,,] param, string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save3DArray<T>(T[,,] param, string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save3DArray<T>(T[,,] param, string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save<TKey, TValue>(Dictionary<TKey, TValue> param, string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save<TKey, TValue>(Dictionary<TKey, TValue> param, string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save<T>(List<T> param, string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save<T>(HashSet<T> param, string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save<T>(Queue<T> param, string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save<T>(Stack<T> param, string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save<T>(List<T> param, string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save<T>(HashSet<T> param, string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save<T>(Queue<T> param, string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void Save<T>(Stack<T> param, string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.Write(param, eS2Settings.filenameData.tag);
		eS2Writer.Save();
	}

	public static void SaveRaw(string param, string identifier)
	{
		SaveRaw(Encoding.UTF8.GetBytes(param), identifier);
	}

	public static void SaveRaw(string param, string identifier, ES2Settings settings)
	{
		SaveRaw(Encoding.UTF8.GetBytes(param), identifier, settings);
	}

	public static void SaveRaw(byte[] param, string identifier)
	{
		using ES2Writer eS2Writer = ES2Writer.Create(new ES2Settings(identifier));
		eS2Writer.WriteRaw(param);
		eS2Writer.Save(checkForOverwrite: false);
	}

	public static void SaveRaw(byte[] param, string identifier, ES2Settings settings)
	{
		using ES2Writer eS2Writer = ES2Writer.Create(settings.Clone(identifier));
		eS2Writer.WriteRaw(param);
		eS2Writer.Save(checkForOverwrite: false);
	}

	public static void SaveRaw(TextAsset param, string identifier)
	{
		using ES2Writer eS2Writer = ES2Writer.Create(new ES2Settings(identifier));
		eS2Writer.WriteRaw(param.bytes);
		eS2Writer.Save(checkForOverwrite: false);
	}

	public static void SaveRaw(TextAsset param, string identifier, ES2Settings settings)
	{
		using ES2Writer eS2Writer = ES2Writer.Create(settings.Clone(identifier));
		eS2Writer.WriteRaw(param.bytes);
		eS2Writer.Save(checkForOverwrite: false);
	}

	public static void AppendRaw(string param, string identifier)
	{
		AppendRaw(Encoding.UTF8.GetBytes(param), identifier);
	}

	public static void AppendRaw(string param, string identifier, ES2Settings settings)
	{
		AppendRaw(Encoding.UTF8.GetBytes(param), identifier, settings);
	}

	public static void AppendRaw(byte[] param, string identifier)
	{
		using ES2Writer eS2Writer = ES2Writer.Create(new ES2Settings(identifier)
		{
			fileMode = ES2Settings.ES2FileMode.Append
		});
		eS2Writer.WriteRaw(param);
		eS2Writer.Save(checkForOverwrite: false);
	}

	public static void AppendRaw(byte[] param, string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		eS2Settings.fileMode = ES2Settings.ES2FileMode.Append;
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.WriteRaw(param);
		eS2Writer.Save(checkForOverwrite: false);
	}

	public static void AppendRaw(TextAsset param, string identifier)
	{
		using ES2Writer eS2Writer = ES2Writer.Create(new ES2Settings(identifier)
		{
			fileMode = ES2Settings.ES2FileMode.Append
		});
		eS2Writer.WriteRaw(param.bytes);
		eS2Writer.Save(checkForOverwrite: false);
	}

	public static void AppendRaw(TextAsset param, string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		eS2Settings.fileMode = ES2Settings.ES2FileMode.Append;
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.WriteRaw(param.bytes);
		eS2Writer.Save(checkForOverwrite: false);
	}

	public static void SaveImage(Texture2D tex, string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		if (eS2Settings.filenameData.extension.ToLower() != ".png")
		{
			Debug.LogError("Easy Save 2 Error: ES2.SaveImage can only be used to save PNG files. Please change your path extension to PNG.");
		}
		using ES2Writer eS2Writer = ES2Writer.Create(eS2Settings);
		eS2Writer.WriteRaw(tex.EncodeToPNG());
		eS2Writer.Save(checkForOverwrite: false);
	}

	public static byte[] LoadRaw(string identifier)
	{
		using ES2Reader eS2Reader = ES2Reader.Create(new ES2Settings(identifier));
		return eS2Reader.ReadRaw();
	}

	public static byte[] LoadRaw(string identifier, ES2Settings settings)
	{
		using ES2Reader eS2Reader = ES2Reader.Create(settings.Clone(identifier));
		return eS2Reader.ReadRaw();
	}

	public static object LoadObject(string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.ReadObject(eS2Settings.filenameData.tag);
	}

	public static object LoadObject(string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.ReadObject(eS2Settings.filenameData.tag);
	}

	public static T Load<T>(string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.Read<T>(eS2Settings.filenameData.tag);
	}

	public static T Load<T>(string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.Read<T>(eS2Settings.filenameData.tag);
	}

	public static void Load<T>(string identifier, T c) where T : class
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		eS2Reader.Read(eS2Settings.filenameData.tag, c);
	}

	public static void Load<T>(string identifier, T c, ES2Settings settings) where T : class
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		eS2Reader.Read(eS2Settings.filenameData.tag, c);
	}

	public static T[] LoadArray<T>(string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.ReadArray<T>(eS2Settings.filenameData.tag);
	}

	public static T[] LoadArray<T>(string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.ReadArray<T>(eS2Settings.filenameData.tag);
	}

	public static void LoadArray<T>(string identifier, T[] c) where T : class
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		eS2Reader.ReadArray(eS2Settings.filenameData.tag, c);
	}

	public static void LoadArray<T>(string identifier, T[] c, ES2Settings settings) where T : class
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		eS2Reader.ReadArray(eS2Settings.filenameData.tag, c);
	}

	public static T[,] Load2DArray<T>(string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.Read2DArray<T>(eS2Settings.filenameData.tag);
	}

	public static T[,] Load2DArray<T>(string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.Read2DArray<T>(eS2Settings.filenameData.tag);
	}

	public static T[,,] Load3DArray<T>(string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.Read3DArray<T>(eS2Settings.filenameData.tag);
	}

	public static T[,,] Load3DArray<T>(string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.Read3DArray<T>(eS2Settings.filenameData.tag);
	}

	public static Dictionary<TKey, TValue> LoadDictionary<TKey, TValue>(string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.ReadDictionary<TKey, TValue>(eS2Settings.filenameData.tag);
	}

	public static Dictionary<TKey, TValue> LoadDictionary<TKey, TValue>(string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.ReadDictionary<TKey, TValue>(eS2Settings.filenameData.tag);
	}

	public static List<T> LoadList<T>(string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.ReadList<T>(eS2Settings.filenameData.tag);
	}

	public static List<T> LoadList<T>(string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.ReadList<T>(eS2Settings.filenameData.tag);
	}

	public static void LoadList<T>(string identifier, List<T> c) where T : class
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		eS2Reader.ReadList(eS2Settings.filenameData.tag, c);
	}

	public static void LoadList<T>(string identifier, List<T> c, ES2Settings settings) where T : class
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		eS2Reader.ReadList(eS2Settings.filenameData.tag, c);
	}

	public static HashSet<T> LoadHashSet<T>(string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.ReadHashSet<T>(eS2Settings.filenameData.tag);
	}

	public static HashSet<T> LoadHashSet<T>(string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.ReadHashSet<T>(eS2Settings.filenameData.tag);
	}

	public static Queue<T> LoadQueue<T>(string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.ReadQueue<T>(eS2Settings.filenameData.tag);
	}

	public static Queue<T> LoadQueue<T>(string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.ReadQueue<T>(eS2Settings.filenameData.tag);
	}

	public static Stack<T> LoadStack<T>(string identifier)
	{
		ES2Settings eS2Settings = new ES2Settings(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.ReadStack<T>(eS2Settings.filenameData.tag);
	}

	public static Stack<T> LoadStack<T>(string identifier, ES2Settings settings)
	{
		ES2Settings eS2Settings = settings.Clone(identifier);
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return eS2Reader.ReadStack<T>(eS2Settings.filenameData.tag);
	}

	public static ES2Data LoadAll(string path)
	{
		using ES2Reader eS2Reader = ES2Reader.Create(new ES2Settings(path));
		return eS2Reader.ReadAll();
	}

	public static ES2Data LoadAll(string identifier, ES2Settings settings)
	{
		using ES2Reader eS2Reader = ES2Reader.Create(settings.Clone(identifier));
		return eS2Reader.ReadAll();
	}

	public static Texture2D LoadImage(string path)
	{
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			Debug.LogError("Easy Save 2 Error: You cannot use LoadImage with WebGL");
		}
		ES2Settings eS2Settings = new ES2Settings(path);
		if (!eS2Settings.IsImageFile)
		{
			Debug.LogError("ES2.LoadImage can only be used to load JPG and PNG files.\nThe exception thrown is as follows:\n");
		}
		using ES2Reader eS2Reader = ES2Reader.Create(eS2Settings);
		return LoadImage(eS2Reader.ReadRaw());
	}

	public static Texture2D LoadImage(byte[] bytes)
	{
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.LoadImage(bytes);
		return texture2D;
	}

	public static AudioClip LoadAudio(string path)
	{
		Debug.LogError("ES2.LoadAudio was deprecated in Unity 2018. Please use Easy Save 3's ES3.LoadAudio method instead.");
		return null;
	}

	public static void CacheFile(string identifier)
	{
		using ES2Reader eS2Reader = ES2Reader.Create(new ES2Settings(identifier));
		eS2Reader.CacheFile();
	}

	public static bool Exists(string identifier)
	{
		return ES2File.Exists(new ES2Settings(identifier));
	}

	public static bool Exists(string identifier, ES2Settings settings)
	{
		return ES2File.Exists(settings.Clone(identifier));
	}

	public static void Delete(string identifier)
	{
		ES2File.Delete(new ES2Settings(identifier));
	}

	public static void Delete(string identifier, ES2Settings settings)
	{
		ES2File.Delete(settings.Clone(identifier));
	}

	public static void DeleteDefaultFolder()
	{
		if (ES2GlobalSettings.defaultSaveLocation == ES2Settings.SaveLocation.PlayerPrefs)
		{
			PlayerPrefs.DeleteAll();
		}
		else
		{
			Delete(ES2FilenameData.GetPersistentPath() + "/");
		}
	}

	public static void Rename(string identifier, string newIdentifier)
	{
		ES2File.Rename(new ES2Settings(identifier), new ES2Settings(newIdentifier));
	}

	public static void Rename(string identifier, string newIdentifier, ES2Settings settings)
	{
		ES2File.Rename(settings.Clone(identifier), settings.Clone(newIdentifier));
	}

	public static string[] GetFiles(string identifier)
	{
		return ES2File.GetFiles(new ES2Settings(identifier));
	}

	public static string[] GetFiles(string identifier, ES2Settings settings)
	{
		return ES2File.GetFiles(settings.Clone(identifier));
	}

	public static string[] GetFiles(string identifier, string extension)
	{
		return ES2File.GetFiles(new ES2Settings(identifier), extension);
	}

	public static string[] GetFolders(string identifier)
	{
		return ES2File.GetFolders(new ES2Settings(identifier));
	}

	public static string[] GetFolders(string identifier, ES2Settings settings)
	{
		return ES2File.GetFolders(settings.Clone(identifier));
	}

	public static string[] GetTags(string identifier)
	{
		return ES2File.GetTags(new ES2Settings(identifier));
	}

	public static string[] GetTags(string identifier, ES2Settings settings)
	{
		return ES2File.GetTags(settings.Clone(identifier));
	}
}
