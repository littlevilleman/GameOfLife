using Core;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "BoardConfig", menuName = "Config/BoardConfig", order = 1)]
    public class BoardConfig : ScriptableObject, IBoardConfig
    {
        [field: SerializeField] public Vector2Int Size { get; private set; } = new Vector2Int(250, 250);
        [field: SerializeField] public Color AliveColor { get; private set; } = Color.white;
        [field: SerializeField] public Color DeadColor { get; private set; } = Color.black;
        [field: SerializeField] public float ViewportSpeed { get; private set; } = 100f;

        public Vector2Int Resolution => new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
    }
    public static class TerrainUtils
    {
        public static Vector2 GetNoiseCoord(float noiseScale, Vector2Int size, Vector2 location, Vector2 offset)
        {
            return new Vector2(location.x / size.x, location.y / size.y) * noiseScale + offset;
        }

        public static float GetNoiseFactor(float density, Vector2 coords)
        {
            float sample = Mathf.PerlinNoise(coords.x, coords.y);
            return Mathf.Pow(sample, density);
        }

        public static float GetGradientShapeFactor(float density, Vector2 location, Vector2 source, Vector2 radius)
        {
            float sample = Mathf.Pow((location.x - source.x) / source.x / radius.x, 2) + Mathf.Pow((location.y - source.y) / source.y / radius.y, 2);
            return Mathf.Pow(sample, density);
        }
        public static float GetLinearShapeFactor(float density, Vector2 location, Vector2 source)
        {
            float sample = Mathf.Pow(location.x - source.x, 2) + Mathf.Pow(location.y - source.y, 2);
            return Mathf.Pow(sample, density);
        }
    }
}