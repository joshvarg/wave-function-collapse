using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VargheseJoshua.Lab6
{
    public class Cell : MonoBehaviour
    {
        public List<Constraint> sp = new List<Constraint>(5);

        public int entropy = 5;

        private GameObject tile;

        public (int x, int z) position;

        public Cell north;
        public Cell south;
        public Cell west;
        public Cell east;

        [SerializeField]
        public Globals.superpositions currentState = Globals.superpositions.super;
        //void Awake()
        public void Init()
        {
            InitializeConstraints();
            Debug.Log("spcount"+ sp.Count);
            for (int i = 0; i < sp.Count; i++)
            {
                sp[i].isPossible = true;
                sp[i].Name = (Globals.superpositions)i;
            }
            tile = this.gameObject;
        }

        public void Collapse()
        {
            List<Constraint> choose = new List<Constraint>();
            foreach(Constraint c in sp)
            {
                if (c.isPossible) { choose.Add(c); }
            }

            int rand = Globals.RNG.Next(0, choose.Count);
            Debug.Log(rand);
            currentState = choose[rand].Name;
            entropy = 6;

            tile.GetComponent<Renderer>().material.SetColor("_Color", Globals.Colors[currentState]);
        }

        public void UpdateEntropy()
        {
            foreach(Constraint c in sp)
            {
                if (!c.isPossible)
                {
                    entropy--;
                }
            }
        }

        private void InitializeConstraints()
        {
            for(int i = 0; i < 5; i++)
            {
                sp.Add(new Constraint((Globals.superpositions)i, true));
            }
        }
    }
}
