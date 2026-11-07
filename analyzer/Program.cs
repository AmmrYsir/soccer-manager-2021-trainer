using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.Metadata;
using ICSharpCode.Decompiler.TypeSystem;

namespace Analyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseDir = Directory.Exists("analyzer") ? Directory.GetCurrentDirectory() : Path.GetFullPath("..");
            string dllPath = Path.Combine(baseDir, "Assembly-CSharp.dll");
            string firstPassPath = Path.Combine(baseDir, "Assembly-CSharp-firstpass.dll");
            string managedDir = @"D:\SteamLibrary\steamapps\common\Soccer Manager 2021\SM21_Data\Managed";

            var resolver = new UniversalAssemblyResolver(dllPath, false, ".NETFramework,Version=v4.0");
            resolver.AddSearchDirectory(managedDir);
            resolver.AddSearchDirectory(Path.GetDirectoryName(dllPath)!);

            var settings = new DecompilerSettings();
            var decompiler = new CSharpDecompiler(dllPath, resolver, settings);
            var firstpassDecompiler = File.Exists(firstPassPath) ? new CSharpDecompiler(firstPassPath, resolver, settings) : null;
            var allTypes = decompiler.TypeSystem.GetAllTypeDefinitions()
                .Concat(firstpassDecompiler != null ? firstpassDecompiler.TypeSystem.GetAllTypeDefinitions() : Enumerable.Empty<ITypeDefinition>())
                .ToList();

            string outDir = Path.Combine(baseDir, "decompiled");
            Directory.CreateDirectory(outDir);

            if (args.Length > 0)
            {
                foreach (var name in args)
                {
                    var matches = allTypes.Where(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && t.DeclaringTypeDefinition == null).ToList();
                    if (!matches.Any())
                    {
                        matches = allTypes.Where(t => t.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0 && t.DeclaringTypeDefinition == null).Take(5).ToList();
                    }
                    foreach (var type in matches)
                    {
                        string filePath = Path.Combine(outDir, $"{type.Name}.cs");
                        try
                        {
                            var activeDecompiler = type.ParentModule.AssemblyName.Contains("firstpass") ? firstpassDecompiler : decompiler;
                            string code = activeDecompiler!.DecompileTypeAsString(type.FullTypeName);
                            File.WriteAllText(filePath, code);
                            Console.WriteLine($"Saved: {filePath}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error decompiling {type.Name}: {ex.Message}");
                        }
                    }
                }
                return;
            }

        }
    }
}
