using System.ComponentModel;
using UnityEngine;

public sealed class ES2Settings
{
	public enum OptimizeMode
	{
		Fast,
		LowMemory
	}

	public enum SaveLocation
	{
		PlayerPrefs,
		File,
		Resources,
		Memory
	}

	public enum Format
	{
		Binary
	}

	public enum EncryptionType
	{
		AES128,
		Obfuscate
	}

	public enum ES2FileMode
	{
		Create,
		Append,
		Open
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public ES2FilenameData filenameData;

	public SaveLocation saveLocation = ES2GlobalSettings.defaultSaveLocation;

	public OptimizeMode optimizeMode = ES2GlobalSettings.defaultOptimizeMode;

	public Format format = ES2GlobalSettings.defaultFormat;

	public bool encrypt = ES2GlobalSettings.defaultEncrypt;

	public string encryptionPassword = ES2GlobalSettings.defaultEncryptionPassword;

	public EncryptionType encryptionType = ES2GlobalSettings.defaultEncryptionType;

	public string webUsername = ES2GlobalSettings.defaultWebUsername;

	public string webPassword = ES2GlobalSettings.defaultWebPassword;

	public string webFilename = ES2GlobalSettings.defaultWebFilename;

	public bool saveNormals = true;

	public bool saveUV = true;

	public bool saveUV2 = true;

	public bool saveTangents = true;

	public bool saveSubmeshes = true;

	public bool saveSkinning = true;

	public bool saveColors = true;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public byte meshSettingsCount = 7;

	public string name = "ES2Settings";

	[EditorBrowsable(EditorBrowsableState.Never)]
	public ES2FileMode fileMode;

	public int bufferSize = ES2GlobalSettings.defaultBufferSize;

	public string tag
	{
		get
		{
			return filenameData.tag;
		}
		set
		{
			filenameData.tag = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool IsImageFile
	{
		get
		{
			switch (filenameData.extension.ToLower())
			{
			case ".jpg":
			case ".png":
			case ".jpeg":
			case ".bytes":
				return true;
			default:
				return false;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Init()
	{
		if (!ES2.initialised)
		{
			ES2.Init();
		}
		saveLocation = ES2GlobalSettings.defaultSaveLocation;
		optimizeMode = ES2GlobalSettings.defaultOptimizeMode;
		format = ES2GlobalSettings.defaultFormat;
		encrypt = ES2GlobalSettings.defaultEncrypt;
		encryptionPassword = ES2GlobalSettings.defaultEncryptionPassword;
		encryptionType = ES2GlobalSettings.defaultEncryptionType;
		webUsername = ES2GlobalSettings.defaultWebUsername;
		webPassword = ES2GlobalSettings.defaultWebPassword;
		webFilename = ES2GlobalSettings.defaultWebFilename;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public ES2Settings Clone()
	{
		return new ES2Settings
		{
			filenameData = filenameData,
			saveLocation = saveLocation,
			optimizeMode = optimizeMode,
			encrypt = encrypt,
			encryptionPassword = encryptionPassword,
			encryptionType = encryptionType,
			webUsername = webUsername,
			webPassword = webPassword,
			webFilename = webFilename,
			saveNormals = saveNormals,
			saveColors = saveColors,
			saveUV = saveUV,
			saveUV2 = saveUV2,
			saveTangents = saveTangents,
			saveSubmeshes = saveSubmeshes,
			saveSkinning = saveSkinning,
			meshSettingsCount = meshSettingsCount,
			name = name,
			fileMode = fileMode
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public ES2Settings Clone(string identifier)
	{
		ES2Settings eS2Settings = Clone();
		eS2Settings.filenameData = new ES2FilenameData(identifier, eS2Settings, useParameters: true);
		return eS2Settings;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public byte[] MeshSettingsToByteArray()
	{
		return new byte[8]
		{
			meshSettingsCount,
			(byte)(saveNormals ? 1 : 0),
			(byte)(saveUV ? 1 : 0),
			(byte)(saveUV2 ? 1 : 0),
			(byte)(saveTangents ? 1 : 0),
			(byte)(saveSubmeshes ? 1 : 0),
			(byte)(saveSkinning ? 1 : 0),
			(byte)(saveColors ? 1 : 0)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void MeshSettingsFromByteArray(byte[] bytes)
	{
		if (bytes.Length >= 4)
		{
			saveNormals = bytes[0] != 0;
			saveUV = bytes[1] != 0;
			saveUV2 = bytes[2] != 0;
			saveTangents = bytes[3] != 0;
		}
		if (bytes.Length >= 5)
		{
			saveSubmeshes = bytes[4] != 0;
		}
		if (bytes.Length >= 6)
		{
			saveSkinning = bytes[5] != 0;
		}
		if (bytes.Length == 7)
		{
			saveColors = bytes[6] != 0;
		}
		else if (bytes.Length > 7)
		{
			Debug.LogError("Byte array containing mesh settings is too long. Length: " + bytes.Length + ".");
		}
	}

	public ES2Settings(string identifier)
	{
		Init();
		filenameData = new ES2FilenameData(identifier, this);
	}

	public ES2Settings(SaveLocation saveLocation)
	{
		Init();
		this.saveLocation = saveLocation;
	}

	public ES2Settings()
	{
		Init();
	}
}
