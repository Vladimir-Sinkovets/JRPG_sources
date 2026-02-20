using UnityEngine.UI;
using UnityEngine;

namespace Assets.Game.Scripts.Common.UI
{
    public static class UIExtensions
    {
        public static int GetColumnsCount(this GridLayoutGroup grid)
        {
            if (grid.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
            {
                return grid.constraintCount;
            }

            if (grid.constraint == GridLayoutGroup.Constraint.FixedRowCount)
            {
                int childCount = grid.transform.childCount;
                int rows = grid.constraintCount;
                return Mathf.CeilToInt((float)childCount / rows);
            }

            return CalculateColumnsInFlexibleMode(grid);
        }

        private static int CalculateColumnsInFlexibleMode(GridLayoutGroup grid)
        {
            RectTransform rect = grid.GetComponent<RectTransform>();
            float totalWidth = rect.rect.width - grid.padding.horizontal;
            float cellWidth = grid.cellSize.x + grid.spacing.x;

            int columns = Mathf.FloorToInt((totalWidth + grid.spacing.x) / cellWidth);
            return Mathf.Max(1, columns);
        }
    }
}
