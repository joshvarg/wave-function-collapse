using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace VargheseJoshua.Lab6
{
    public class ScriptManager : MonoBehaviour
    {
        [SerializeField] Grid grid;
        [SerializeField] Transform Ocam;
        [SerializeField] Transform Pcam;
        [SerializeField] Transform gridObject;
        public Cell mostRecentCollapse;
        public (int, int) contraposition = (0,0);
        public (int, int) start;
        public bool restart = false;
        private bool done = false;
        private Cell impossible;

        private void Start()
        {
            Pcam.transform.position = new Vector3(-grid.rows / 2, 15, -grid.columns / 2);
            start = (grid.rows / 2, grid.columns / 2);
            Initialize(start);
        }
        private void LateUpdate()
        {
            Pcam.RotateAround(Vector3.zero, Vector3.up, 2f * Time.deltaTime);
            if (!done)
            {
                if (restart)
                {
                    impossible.Init();
                    impossible.ResetNeighbors(0);
                    restart = false;
                }
                else
                {
                    CollapseLowestEntropy();

                }
            }
            else
            {
                Clear();
                Initialize(start);
                done = false;
            }
        }
        private void Initialize((int, int) initialcollapse)
        {
            grid.Init(gridObject);
            UpdateNeighbors(grid);
            grid.cellgrid[initialcollapse.Item1, initialcollapse.Item2].Collapse();
            mostRecentCollapse = grid.cellgrid[0, 0];
        }
        public void Clear()
        {
            Destroy(gridObject.gameObject);
            var x = new GameObject("GridObject");
            gridObject = x.transform;
        }
        private void UpdateNeighbors(Grid g)
        {
            foreach (Cell c in g.cellgrid)
            {
                if (c.position.x + 1 < grid.columns)
                {
                    c.east = grid.cellgrid[c.position.x + 1, c.position.z];
                }
                if (c.position.x - 1 >= 0)
                {
                    c.west = grid.cellgrid[c.position.x - 1, c.position.z];
                }
                if (c.position.z + 1 < grid.rows)
                {
                    c.north = grid.cellgrid[c.position.x, c.position.z + 1];
                }
                if (c.position.z - 1 >= 0)
                {
                    c.south = grid.cellgrid[c.position.x, c.position.z - 1];
                }
            }
        }
        public void UpdateConstraints(Cell c)
        {
            if (!c.north.IsUnityNull())
            {
                c.north.sp = FindAllowedConstraints(c, c.north);
                c.north.entropy = c.north.sp.Count;                
            }
            if (!c.south.IsUnityNull())
            {
                c.south.sp = FindAllowedConstraints(c, c.south);
                c.south.entropy = c.south.sp.Count;
            }
            if (!c.east.IsUnityNull())
            {
                c.east.sp = FindAllowedConstraints(c, c.east);
                c.east.entropy = c.east.sp.Count;
            }
            if (!c.west.IsUnityNull())
            {
                c.west.sp = FindAllowedConstraints(c, c.west);
                c.west.entropy = c.west.sp.Count;
            }
            c.splist = c.sp.ToCommaSeparatedString();
            c.entropy = c.sp.Count;
        }
        private List<Constraint> FindAllowedConstraints(Cell host, Cell neighbor)
        {
            if (!neighbor.stable)
            {
                var update = new List<Constraint>();
                foreach (Constraint con in host.sp)
                {
                    foreach (Constraint ncon in neighbor.sp)
                    {
                        if (Globals.Rules[con.state].L == ncon.state
                            || Globals.Rules[con.state].R == ncon.state
                            || con.state == ncon.state)// OR the same
                        {
                            update.Add(ncon);
                        }
                    }
                }
                if (update.Count <= 0)
                {
                    Debug.Log("CONTRADICTION FOUND at " + neighbor.position + " restarting!");
                    contraposition = neighbor.position;
                    impossible = neighbor;
                    restart = true;
                }
                return update.Distinct().ToList();
            }
            return neighbor.sp;

        }
        private void CollapseLowestEntropy()
        {
            var x = CollectLowestEntropy();

            if (x.Count == 0) { done = true; }

            if (!done)
            {
                var rand = Globals.RNG.Next() % x.Count;
                int iterator = 0;
                Cell chosen = x[rand];
                while (chosen.StableNeighbors() < 4 && iterator <= x.Count)
                {
                    rand = rand++ % x.Count;
                    chosen = x[rand];
                    iterator++;
                }
                if (chosen.entropy > 0)
                {
                    chosen.Collapse();
                }
                else
                {
                    impossible = chosen;
                    restart = true;
                }
            }
        }
        private List<Cell> CollectLowestEntropy()
        {
            int min = Globals.MAX_STATES;
            List<Cell> temp = new List<Cell>();
            foreach (Cell c in grid.cellgrid)
            {
                if (c.stable || c.entropy > min)
                {
                    continue;
                }
                else
                {
                    if (c.entropy < min)
                    {
                        min = c.entropy;
                        temp.Clear();
                    }
                    if (c.entropy == min)
                    {
                        temp.Add(c);
                    }
                }

            }
            return temp;
        }
    }
}
