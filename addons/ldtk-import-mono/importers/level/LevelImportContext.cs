using Godot;
using LDtkImport.Json;

namespace LDtkImport.Importers
{
    public record LevelImportContext
    {
        public WorldJson.Root WorldJson { get; init; }
        public LevelJson.Root LevelJson { get; init; }
    }
}
