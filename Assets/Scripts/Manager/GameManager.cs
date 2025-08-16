using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Collections;
namespace Game
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
    }
    public class GameManager : Singleton<GameManager>
    {
        public List<CellConfig> CellConfigs = new List<CellConfig>();

        public Grid Grid;

        public int SizeX;
        public int SizeY;

        public Dictionary<Direction, Vector2Int> Directions = new Dictionary<Direction, Vector2Int>()
        {
            { Game.Direction.Up, new Vector2Int(0, 1) },
            { Game.Direction.Right, new Vector2Int(1, 0) },
            { Game.Direction.Down, new Vector2Int(0, -1) },
            { Game.Direction.Left, new Vector2Int(-1, 0) },
        };

        [Button]
        public void SpawnMap()
        {
            var listSocketData = new List<CellData>();

            foreach(var i in CellConfigs)
            {
                listSocketData.Add(i.CellData);
            }
            var cells = new CellOption[SizeX, SizeY];
            for(int x = 0; x < SizeX; x++)
            {
                for(int y = 0; y < SizeY; y++)
                {
                    cells[x, y] = new CellOption(new Vector2Int(x, y), listSocketData, false);
                }
            }

            var start = cells[0, 0];
            start.IsCollapse = true;
            start.CellData = start.GetRandCellData();
            var visited = new Stack<CellOption>();
            visited.Push(start);

            CheckCell(start);
            StartCoroutine(IESpawn());

            IEnumerator IESpawn()
            {
                for (int y = 0; y < SizeY; y++)
                {
                    for (int x = 0; x < SizeX; x++)
                    {
                        var cell = cells[x, y];
                        if (cell.CellData == null) continue;
                        var item = SimplePool.Spawn(cell.CellData.CellPrefab, Grid.CellToWorld(new Vector3Int(x, 0, y)), Quaternion.identity);
                        yield return new WaitForSeconds(.1f);
                    }
                }
            }

            //for(int y = 0; y < SizeY; y++)
            //{
            //    for (int x = 0; x < SizeX; x++)
            //    {
            //        var cell = cells[x, y];
            //        if (cell.CellData == null) continue;
            //        var item = SimplePool.Spawn(cell.CellData.CellPrefab, Grid.CellToWorld(new Vector3Int(x, 0, y)), Quaternion.identity);
            //    }
            //}


            void CheckCell(CellOption start)
            {
                if (visited.Count >= SizeX * SizeY)
                {
                    return;
                }
                var min = int.MaxValue;
                var cacheCell = start;
                var tmp = 0;
                for (int i = 0; i < Directions.Count; i++)
                {
                    var element = Directions.ElementAt(i);
                    var coord = start.Pos + element.Value;
                    if (coord.x < 0 || coord.x >= SizeX || coord.y < 0 || coord.y >= SizeY) continue;
                    var next = cells[coord.x, coord.y];
                    if (next == null) continue;
                    if (next.IsCollapse) continue;
                    if (next.CellDatas.Count <= 0)
                    {
                        next.IsCollapse = true;
                        if (!visited.Contains(next)) visited.Push(next);
                        continue;
                    }
                    CheckCellOptions(start, next, element.Key);
                    if (next.CellDatas.Count < min)
                    {
                        cacheCell = next;
                    }
                    tmp++;
                }
                if (tmp <= 0 && visited.Count > 0)
                {
                    cacheCell = visited.Peek();
                }
                cacheCell.IsCollapse = true;
                cacheCell.CellData = cacheCell.GetRandCellData();
                if(!visited.Contains(cacheCell)) visited.Push(cacheCell);
                CheckCell(cacheCell);
            }

            void CheckCellOptions(CellOption start, CellOption to, Direction dir)
            {
                var removeDatas = new List<CellData>();
                if (start.CellData == null) return;
                foreach(var i in to.CellDatas)
                {
                    if (dir == Game.Direction.Up && start.CellData.SocketData.Up != i.SocketData.Down)
                    {
                        removeDatas.Add(i);
                    }
                    if (dir == Game.Direction.Right && start.CellData.SocketData.Right != i.SocketData.Left)
                    {
                        removeDatas.Add(i);
                    }
                    if (dir == Game.Direction.Down && start.CellData.SocketData.Down != i.SocketData.Up)
                    {
                        removeDatas.Add(i);
                    }
                    if (dir == Game.Direction.Left && start.CellData.SocketData.Left != i.SocketData.Right)
                    {
                        removeDatas.Add(i);
                    }
                }

                foreach(var i in removeDatas)
                {
                    if (to.CellDatas.Contains(i))
                    {
                        to.CellDatas.Remove(i);
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class CellOption
    {
        public Vector2Int Pos;
        public List<CellData> CellDatas = new List<CellData>();
        public bool IsCollapse = false;
        public CellData CellData = null;

        public CellData GetRandCellData()
        {
            if(CellDatas.Count > 0)
            {
                return CellDatas[Random.Range(0, CellDatas.Count)];
            }
            return null;
        }
        public CellOption(Vector2Int pos, List<CellData> cellDatas, bool isCollapse)
        {
            Pos = pos;
            CellDatas = new List<CellData>(cellDatas);
            IsCollapse = isCollapse;
            CellData = null;
        }
    }
}
