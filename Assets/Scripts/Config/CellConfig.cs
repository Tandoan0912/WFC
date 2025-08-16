using JetBrains.Annotations;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "CellConfig", menuName = "GameConfig/CellConfig")]
    public class CellConfig : ScriptableObject
    {
        public CellData CellData;
    }

    [System.Serializable]
    public class SocketData
    {
        public SocketValue Up;
        public SocketValue Right;
        public SocketValue Down;
        public SocketValue Left;
    }

    [System.Serializable]
    public class CellData
    {
        public GameObject CellPrefab;
        public SocketData SocketData;
    }
}
