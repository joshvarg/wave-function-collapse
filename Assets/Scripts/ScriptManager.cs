using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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

        private Dictionary<int, List<Cell>> entropystore;
        private List<Cell> pool = new List<Cell>();

        private Cell impossible;

        public void Clear()
        {
            Destroy(gridObject.gameObject);
            var x = new GameObject("GridObject");
            gridObject = x.transform;
        }
        // Start is called before the first frame update
        private void Start()
        {
            Pcam.transform.position= new Vector3(-grid.rows/2, 15, -grid.columns/2);
            start = (grid.rows / 2, grid.columns / 2);
            Initialize(start);
        }
        private void Initialize((int,int) initialcollapse)
        {
            grid.Init(gridObject);
            //cam.position = new Vector3(grid.columns / 2, (grid.columns+grid.rows)/2, grid.rows / 2);
            UpdateNeighbors(grid);
            grid.cellgrid[initialcollapse.Item1, initialcollapse.Item2].Collapse();
            mostRecentCollapse = grid.cellgrid[0,0];
        }

        private void LateUpdate()
        {
            Pcam.RotateAround(Vector3.zero, Vector3.up, 2f * Time.deltaTime);
            /*entropystore = new Dictionary<int, List<Cell>>() {
            { 1, new List<Cell>() }, {2, new List<Cell>() }, {3, new List<Cell>()}, {4, new List<Cell>() }, {5, new List<Cell>() }, { 6, new List<Cell>() }, {7, new List<Cell>()},
        };*/
            //Debug.Log(done);
            /*if (!done)
            {*/
            //var copy = new List<Cell>(pool);
            /*foreach (var c in grid.cellgrid.Cast<Cell>().ToArray())
            {
                *//*if(mostRecentCollapse.position == (30, 5) && c.position==(30,5)) { 
                    Debug.Log("STOP"); 
                }*//*
                //Debug.Log(c.position);
                UpdateConstraints(c);
                AddToPool(c);
            }*/
            if (!done)
            {
                if (restart)
                {
                    impossible.Init();
                    impossible.ResetNeighbors(0);
                    restart = false;
                    /*Destroy(gridObject.gameObject);
                    gridObject = new GameObject("GridObject").transform;
                    Initialize(contraposition);
                    restart = false;*/
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
            //}
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

        private List<Constraint> FindAllowedConstraints(Cell host, Cell neighbor)
        {
            if (!neighbor.stable)//added
            {
                var update = new List<Constraint>();
                foreach (Constraint con in host.sp)
                {
                    foreach (Constraint ncon in neighbor.sp)
                    {
                        if (Globals.Rules[con.state].L == ncon.state //equals left
                            || Globals.Rules[con.state].R == ncon.state// OR right
                            || con.state == ncon.state)// OR the same
                        {
                            //Debug.Log(Globals.Rules[con.state]);
                            //Debug.Log("cell state: "+con.state+"  north state: "+ncon.state);
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
            return neighbor.sp;//added
            
        }
        public void UpdateConstraints(Cell c)
        {
            if (!c.north.IsUnityNull())
            {
                c.north.sp = FindAllowedConstraints(c, c.north);
                c.north.entropy = c.north.sp.Count;
                
                //Debug.Log("n"+c.north.sp.ToCommaSeparatedString());
            }
            if (!c.south.IsUnityNull())
            {
                c.south.sp = FindAllowedConstraints(c, c.south);
                c.south.entropy = c.south.sp.Count;
                //Debug.Log("s"+c.south.sp.ToCommaSeparatedString());
            }
            if (!c.east.IsUnityNull())
            {
                c.east.sp = FindAllowedConstraints(c, c.east);
                c.east.entropy = c.east.sp.Count;
                //Debug.Log("e"+c.east.sp.ToCommaSeparatedString());
            }
            if (!c.west.IsUnityNull())
            {
                c.west.sp = FindAllowedConstraints(c, c.west);
                c.west.entropy = c.west.sp.Count;
                //Debug.Log("w"+c.west.sp.ToCommaSeparatedString());
            }
            c.splist = c.sp.ToCommaSeparatedString();
            c.entropy = c.sp.Count;
            //Debug.Log("entropy: " + c.entropy);






            //Debug.Log(north.sp.ToCommaSeparatedString());
        }

        private List<Cell> CollectLowestEntropy()
        {
            int min = Globals.MAX_STATES;
            List<Cell> temp = new List<Cell>();
            foreach(Cell c in grid.cellgrid)
            {
                if (c.stable || c.entropy > min)
                {
                    continue;
                } else
                {
                    if (c.entropy < min)
                    {
                        min = c.entropy;
                        temp.Clear();
                    }
                    if(c.entropy == min)
                    {
                        temp.Add(c);
                    }
                }

            }
            
            //Debug.Log("entropy" + temp[0].entropy);
            return temp;
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

        /*private bool CollapseLowestEntropy()
        {
            //Cell lowest = grid.cellgrid[Globals.RNG.Next(0, grid.rows), Globals.RNG.Next(0, grid.columns)];
            Cell lowest = grid.cellgrid[0, 0];
            if(pool.Count> 0) { lowest = pool[Globals.RNG.Next() % pool.Count]; }
            int min = Globals.MAX_STATES;
            //int iteration = 0;
            bool finished = true;
            foreach (Cell c in pool)
            {
                if (c.stable)
                {
                    //Debug.Log(iteration +" " + finished);
                    //iteration++;
                    continue;
                } else if (c.entropy < min) {
                    finished = finished && c.stable;
                    min = c.entropy;
                    lowest = c;
                    //UpdateEntropyStore(min, lowest);
                }
            }
            *//*if(!finished)
            {
                Debug.Log(entropystore.ToCommaSeparatedString());
                var x = entropystore[min];
                var r = Globals.RNG.Next(0, x.Count);
                var picked = x[r];
                picked.Collapse();
                mostRecentCollapse = picked;
                x.Remove(picked); 
            }*//*
            lowest.Collapse();
            mostRecentCollapse= lowest;
            //Debug.Log("end of collapse run" + finished);
            return finished;
        }*/

        /*public void AddToPool(Cell c)
        {
            if (c.stable)
            {
                *//*EAST ADDS*//*

                if(!c.east.IsUnityNull() && !c.east.stable)
                {
                    pool.Add(c.east);
                    if (!c.east.east.IsUnityNull() && !c.east.east.stable)
                    {
                        pool.Add(c.east.east);
                        if (!c.east.east.east.IsUnityNull() && !c.east.east.east.stable)
                        {
                            pool.Add(c.east.east.east);
                        }
                    }

                }
                *//*WEST ADDS*//*
                //if (c.west.IsUnityNull() && !c.west.stable)
                if (!c.west.IsUnityNull() && !c.west.stable)
                {
                    pool.Add(c.west);
                    if (!c.west.west.IsUnityNull() && !c.west.west.stable)
                    {
                        pool.Add(c.west.west);
                        if (!c.west.west.west.IsUnityNull() && !c.west.west.west.stable)
                        {
                            pool.Add(c.west.west.west);
                        }
                    }

                }
                *//*NORTH ADDS*//*

                if (!c.north.IsUnityNull() && !c.north.stable)
                {
                    pool.Add(c.north);
                    if (!c.north.north.IsUnityNull() && !c.north.north.stable)
                    {
                        pool.Add(c.north.north);
                        if (!c.north.north.north.IsUnityNull() && !c.north.north.north.stable)
                        {
                            pool.Add(c.north.north.north);
                        }
                    }
                }
                *//*SOUTH ADDS*//*

                if (!c.south.IsUnityNull() && !c.south.stable)
                {
                    pool.Add(c.south);
                    if (!c.south.south.IsUnityNull() && !c.south.south.stable)
                    {
                        pool.Add(c.south.south);
                        if (!c.south.south.south.IsUnityNull() && !c.south.south.south.stable)
                        {
                            pool.Add(c.south.south.south);
                        }
                    }
                }
            }
        }*/
        private void UpdateEntropyStore(int entropy, Cell cell)
        {
            entropystore[entropy].Add(cell);
        }
    }
}
