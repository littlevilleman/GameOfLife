using Core;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "BoardConfig", menuName = "Config/BoardConfig", order = 1)]
    public class BoardConfig : ScriptableObject, IBoardConfig
    {
        [SerializeField] private int size = 250;

        [field: SerializeField] public Color AliveColor { get; private set; } = Color.green;
        [field: SerializeField] public Color DeadColor { get; private set; } = Color.red;

        public Vector2Int Size => new Vector2Int(size, size);
    }
}