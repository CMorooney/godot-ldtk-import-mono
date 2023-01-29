﻿#if TOOLS

using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Picalines.Godot.LDtkImport.Importers
{
    internal static class LevelImporter
    {
        public static void Import(LevelImportContext context)
        {
            Node levelNode = BaseSceneImporter.ImportOrCreate<Node2D>(context.ImportSettings.LevelSceneSettings);

            levelNode.Name = context.LevelJson.Identifier;
            levelNode.SetScript(ResourceLoader.Load("Scripts/TileMap/TileMapsContainer.cs"));
            levelNode.SetMeta(LDtkConstants.MetaKeys.ImportSettingsFilePath, context.ImportSettings.FilePath);

            if (levelNode is Node2D levelNode2D)
            {
                levelNode2D.ZIndex = context.LevelJson.WorldDepth;
            }

            AddLayers(context, levelNode);

            var levelFields = context.LevelJson.FieldInstances.ToDictionary(field => field.Identifier, field => field.Value);
            levelFields[LDtkConstants.SpecialFieldNames.Size] = context.LevelJson.PxSize;

            LDtkFieldAssigner.Assign(levelNode, levelFields, new());

            SaveScene(context, levelNode);
        }

        private static void AddLayers(LevelImportContext context, Node levelNode)
        {
            Node layersParent = levelNode;

            if (context.ImportSettings.LevelSceneSettings?.LayersParentNodeName is { } layersParentNodeName)
            {
                layersParent = levelNode.GetNodeOrNull(layersParentNodeName)
                    ?? new Node2D() { Name = layersParentNodeName };

                if (layersParent.Owner is null)
                {
                    levelNode.AddChild(layersParent);
                    layersParent.Owner = levelNode;
                }
            }

            foreach (var layerJson in context.LevelJson.LayerInstances!.Reverse())
            {
                if (layersParent is Node2D layer2D)
                {
                    layer2D.Position = layerJson.PxTotalOffset;
                }

                switch (layerJson.Type)
                {
                    case Json.LayerType.Entities:
                    {
                        EntityLayerImporter.Import(context, layerJson, layersParent);
                    }
                    break;

                    default:
                    {
                        TileMapImporter.Import(context, layerJson, layersParent);
                    }
                    break;
                }

                foreach (var layerOwnedNode in GetOwned(layersParent, layersParent))
                {
                    layerOwnedNode.Owner = levelNode;
                }
            }
        }

        private static void SaveScene(LevelImportContext context, Node levelNode)
        {
            var savePath = $"{context.ImportSettings.OutputDirectory}/{context.LevelJson.Identifier}.tscn";

            var packedLevelScene = new PackedScene();
            packedLevelScene.Pack(levelNode);

            if (ResourceSaver.Save(savePath, packedLevelScene) is not Error.Ok)
            {
                throw new LDtkImportException(LDtkImportMessage.FailedToImportLevel(context.LDtkFilePath, context.LevelJson.Identifier));
            }
        }

        private static IEnumerable<Node> GetOwned(Node currentNode, Node owner)
        {
            var ownedChildren = currentNode.GetChildren()
                .OfType<Node>()
                .Where(child => child.Owner == owner);

            foreach (Node child in ownedChildren)
            {
                yield return child;

                foreach (Node grandChild in GetOwned(child, owner))
                {
                    yield return grandChild;
                }
            }
        }
    }
}

#endif