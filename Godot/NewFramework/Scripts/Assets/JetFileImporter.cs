using System.IO;
using System.Linq;
using System.Text.Json;
using Godot;
using Ionic.Zip;

namespace BloonsTD5Rewritten.Godot.NewFramework.Scripts.Assets;

public partial class JetFileImporter : Node
{
	private static JetFileImporter? _instance;
	public static JetFileImporter Instance() => _instance!;
	
	private Node? _assetImporterConfig;
	private ZipFile? _jetFile;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_assetImporterConfig = GetNode<Node>("/root/AssetImporterConfig");
		var jetFile = _assetImporterConfig.Get("jet_file").ToString();
		
		var zipFile = new ZipFile(jetFile);
		zipFile.Password = "Q%_{6#Px]]";
		_jetFile = zipFile;

		_instance = this;
	}

	public ZipEntry? GetFileEntry(string path)
	{
		return _jetFile?.Entries.FirstOrDefault(entry => entry.FileName == path);
	}

	public string GetFileContent(string path)
	{
		var entry = GetFileEntry(path);
		var stream = new MemoryStream();
		entry?.Extract(stream);

		stream.Seek(0, SeekOrigin.Begin);
		var reader = new StreamReader(stream);
		return reader.ReadToEnd();
	}
	
	public Variant GetJsonEntry(string path)
	{
		var data = GetFileContent(path);
		return Json.ParseString(data);
	}

	public JsonElement GetJsonParsed(string path)
	{
		var data = GetFileContent(path);
		
		return JsonSerializer.Deserialize<JsonElement>(data);
	}
}