using UnityEngine;

namespace VargheseJoshua.Lab6
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] public int rows = 3;
        [SerializeField] public int columns = 2;
        public Transform gridobject;
        public Cell[,] cellgrid;
        public void Init(Transform gO)
        {
            gridobject = gO;
            cellgrid = new Cell[columns, rows];
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    var c = new GameObject("x:"+i+", z:"+j);
                    c.transform.SetParent(gridobject);
                    c.transform.position = new Vector3(i, 0, j);
                    var tile = c.AddComponent<Cell>();
                    tile.position = (i, j);
                    tile.Init();
                    cellgrid[i, j] = tile;
                }
            }
            gO.Translate(new Vector3(-rows/2, 0, -columns/2));
        }
    }
}
