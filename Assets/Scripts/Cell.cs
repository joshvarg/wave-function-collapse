using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VargheseJoshua.Lab6
{
    public class Cell : MonoBehaviour
    {
        public List<Constraint> sp = new List<Constraint>(5);

        public int entropy = 5;

        [SerializeField]
        public Globals.superpositions finalState = Globals.superpositions.super;
        void Awake()
        {
            InitializeConstraints();
            Debug.Log("spcount"+ sp.Count);
            for (int i = 0; i < sp.Count; i++)
            {
                sp[i].isPossible = true;
                sp[i].Name = (Globals.superpositions)i;
            }

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
            finalState = choose[rand].Name;
            entropy = 6;
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
