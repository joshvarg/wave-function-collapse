using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VargheseJoshua.Lab6
{
    public class Constraint
    {
        public Globals.superpositions Name;
        public bool isPossible;

        public Constraint(Globals.superpositions n, bool isp) {
            Name = n;
            isPossible = isp;
        }
    }
}
