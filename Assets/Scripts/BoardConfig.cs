using Core;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "BoardConfig", menuName = "Config/BoardConfig", order = 1)]
    public class BoardConfig : ScriptableObject, IBoardConfig
{
    [SerializeField] private int size = 250;
    [SerializeField][Range(1, 100f)] private float refreshRate = .25f;

    public Vector2Int Size => new Vector2Int(size, size);
    public float RefreshRate => refreshRate;
}
}