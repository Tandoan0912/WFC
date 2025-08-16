using UnityEngine;
namespace Game
{
    public class InputSystem : Singleton<InputSystem>
    {
        [SerializeField] private Camera MainCamera;
        [SerializeField] private Grid Grid;
        [SerializeField] private GameObject Indicator;

        private void Update()
        {
            var mousePos = ConvertPositionHelper.GetMousePositionOnPlane(MainCamera);
            var gridPos = Grid.WorldToCell(mousePos);
            Indicator.transform.position = Grid.CellToWorld(gridPos);
        }
    }
}
