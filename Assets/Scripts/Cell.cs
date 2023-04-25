using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace VargheseJoshua.Lab6
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] public string splist;
        [SerializeField] private ScriptManager sc;
        public List<Constraint> sp;
        public bool stable = false;
        public bool atEdge = true;
        public int entropy;
        private GameObject tile;
        public (int x, int z) position;
        public Cell north;
        public Cell south;
        public Cell west;
        public Cell east;
        [SerializeField]
        public Globals.superpositions currentState;
        //void Awake()
        public void Init()
        {
            entropy = Globals.MAX_STATES;
            stable = false;
            sp = new List<Constraint>(Globals.MAX_STATES);
            currentState = Globals.superpositions.super;
            InitializeConstraints();
            tile = this.gameObject;
            sc = GameObject.Find("Manager").GetComponent<ScriptManager>();
        }

        public void Collapse()
        {
            sc.UpdateConstraints(this);

            if (position == (32, 36)) { 
                Debug.Log("stop"); 
            }
            int rand = Globals.RNG.Next(0, sp.Count);
            currentState = sp[rand].state;
            var x = sp[rand];
            //entropy = 6;
            sp.Clear();
            sp.Add(x);

            stable = true;

            var obj = Resources.Load("Prefabs/" + currentState);
            Instantiate(obj, tile.transform);
            sc.UpdateConstraints(this);
        }


        private void InitializeConstraints()
        {
            for(int i = 0; i < Globals.MAX_STATES; i++)
            {
                sp.Add(new Constraint((Globals.superpositions)i));
            }
        }
        public void ResetNeighbors(int i)
        {
            if (!north.IsUnityNull())
            {
                north.Init();
                if (i<Globals.RESET_DEPTH)
                {
                    north.ResetNeighbors(++i);
                }
            }
            if (!south.IsUnityNull())
            {
                south.Init();
                if (i< Globals.RESET_DEPTH)
                {
                    south.ResetNeighbors(++i);
                }
            }
            if (!east.IsUnityNull())
            {
                east.Init();
                if (i< Globals.RESET_DEPTH)
                {
                    east.ResetNeighbors(++i);
                }
            }
            if (!west.IsUnityNull())
            {
                west.Init();
                if (i<Globals.RESET_DEPTH)
                {
                    west.ResetNeighbors(++i);
                }
            }
        }
        public int StableNeighbors()
        {
            int num = 0;
            if (!north.IsUnityNull() && north.stable)
            {
                num++;
            }
            if (!south.IsUnityNull() && south.stable)
            {
                num++;
            }
            if (!east.IsUnityNull() && east.stable)
            {
                num++;
            }
            if (!west.IsUnityNull() && west.stable)
            {
                num++;
            }
            return num;
        }
    }
}
