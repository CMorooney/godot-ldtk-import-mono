using Godot;
using Newtonsoft.Json;
using System.Collections.Generic;
using LDtkImport.Json.Converters;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

#pragma warning disable IDE0051

namespace LDtkImport.Json
{
    public class LevelJson : BaseJson<LevelJson.Root>
    {
        public class Root
        {
            [JsonProperty("identifier")]
            public string Identifier { get; private set; }

            [JsonProperty("uid")]
            public int Uid { get; private set; }

            [JsonProperty("worldX")]
            public int WorldX { get; private set; }

            [JsonProperty("worldY")]
            public int WorldY { get; private set; }

            public Vector2 WorldPos { get; private set; }

            [JsonProperty("pxWid")]
            public int PxWidth { get; private set; }

            [JsonProperty("pxHei")]
            public int PxHeight { get; private set; }

            public Vector2 PxSize { get; private set; }

            [JsonProperty("__bgColor")]
            [JsonConverter(typeof(ColorConverter))]
            public Color? BgColor { get; private set; }

            [JsonProperty("bgRelPath")]
            public string? BgRelPath { get; private set; }

            [JsonProperty("externalRelPath")]
            public string ExternalRelPath { get; private set; }

            [JsonProperty("layerInstances")]
            public IReadOnlyList<LayerInstance> LayerInstances { get; private set; }

            [JsonProperty("__neighbours")]
            public IReadOnlyList<Neighbour> Neighbours { get; private set; }

            [OnDeserialized]
            private void Init(StreamingContext context)
            {
                WorldPos = new Vector2(WorldX, WorldY);
                PxSize = new Vector2(PxWidth, PxHeight);
            }
        }

        public class Neighbour
        {
            [JsonProperty("levelUid")]
            public int LevelUid { get; private set; }

            [JsonProperty("dir")]
            public string Dir { get; private set; }
        }

        public class IntGridValue
        {
            [JsonProperty("coordId")]
            public int CoordId { get; private set; }

            [JsonProperty("v")]
            public int GridValue { get; private set; }
        }

        public class EntityTile
        {
            [JsonProperty("tilesetUid")]
            public int TilesetUid { get; private set; }

            [JsonProperty("srcRect")]
            [JsonConverter(typeof(Rect2Converter))]
            public Rect2 SrcRect { get; private set; }
        }

        public class AutoLayerTile
        {
            [JsonProperty("px")]
            [JsonConverter(typeof(Vector2Converter))]
            public Vector2 LayerPxCoords { get; private set; }

            [JsonProperty("src")]
            [JsonConverter(typeof(Vector2Converter))]
            public Vector2 TileSetPxCoords { get; private set; }

            [JsonProperty("f")]
            public int FlipBits { get; private set; }

            [JsonProperty("t")]
            public int TileId { get; private set; }

            [JsonProperty("d")]
            public IReadOnlyList<int> InternalEditorData { get; private set; }
        }

        public class EntityFieldInstance
        {
            [JsonProperty("__identifier")]
            public string Identifier { get; private set; }

            [JsonProperty("__value")]
            public object Value { get; private set; }

            [JsonProperty("__type")]
            public string Type { get; private set; }

            [JsonProperty("defUid")]
            public string DefUid { get; private set; }
        }

        public class EntityInstance
        {
            [JsonProperty("__identifier")]
            public string Identifier { get; private set; }

            [JsonProperty("__grid")]
            [JsonConverter(typeof(Vector2Converter))]
            public Vector2 Grid { get; private set; }

            [JsonProperty("__pivot")]
            [JsonConverter(typeof(Vector2Converter))]
            public Vector2 Pivot { get; private set; }

            [JsonProperty("__tile")]
            public EntityTile? Tile { get; private set; }

            [JsonProperty("defUid")]
            public int DefUid { get; private set; }

            [JsonProperty("px")]
            [JsonConverter(typeof(Vector2Converter))]
            public Vector2 PxCoords { get; private set; }

            [JsonProperty("fieldInstances")]
            public IReadOnlyList<EntityFieldInstance> FieldInstances { get; private set; }
        }

        public class LayerInstance
        {
            [JsonProperty("__identifier")]
            public string Identifier { get; private set; }

            [JsonProperty("__type")]
            [JsonConverter(typeof(StringEnumConverter))]
            public WorldJson.LayerType Type { get; private set; }

            [JsonProperty("__cWid")]
            public int CellsWidth { get; private set; }

            [JsonProperty("__cHei")]
            public int CellsHeight { get; private set; }

            [JsonProperty("__gridSize")]
            public int GridSize { get; private set; }

            public Vector2 GridSizeV { get; private set; }

            [JsonProperty("__opacity")]
            public int Opacity { get; private set; }

            [JsonProperty("__pxTotalOffsetX")]
            public int PxTotalOffsetX { get; private set; }

            [JsonProperty("__pxTotalOffsetY")]
            public int PxTotalOffsetY { get; private set; }

            public Vector2 PxTotalOffset { get; private set; }

            [JsonProperty("__tilesetDefUid")]
            public int? TilesetDefUid { get; private set; }

            [JsonProperty("__tilesetRelPath")]
            public string TilesetRelPath { get; private set; }

            [JsonProperty("levelId")]
            public int LevelId { get; private set; }

            [JsonProperty("layerDefUid")]
            public int LayerDefUid { get; private set; }

            [JsonProperty("pxOffsetX")]
            public int PxOffsetX { get; private set; }

            [JsonProperty("pxOffsetY")]
            public int PxOffsetY { get; private set; }

            public Vector2 PxOffset { get; private set; }

            [JsonProperty("intGrid")]
            public IReadOnlyList<IntGridValue> IntGrid { get; private set; }

            [JsonProperty("autoLayerTiles")]
            public IReadOnlyList<AutoLayerTile> AutoLayerTiles { get; private set; }

            [JsonProperty("seed")]
            public int Seed { get; private set; }

            [JsonProperty("gridTiles")]
            public IReadOnlyList<object> GridTiles { get; private set; }

            [JsonProperty("entityInstances")]
            public IReadOnlyList<EntityInstance> EntityInstances { get; private set; }

            [OnDeserialized]
            private void Init(StreamingContext context)
            {
                PxTotalOffset = new Vector2(PxTotalOffsetX, PxTotalOffsetY);
                PxOffset = new Vector2(PxOffsetX, PxOffsetY);
                GridSizeV = new Vector2(GridSize, GridSize);
            }
        }
    }
}