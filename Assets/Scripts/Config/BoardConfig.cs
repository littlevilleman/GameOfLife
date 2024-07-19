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
}