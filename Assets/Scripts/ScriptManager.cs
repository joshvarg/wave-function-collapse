using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VargheseJoshua.Lab6
{
    public class ScriptManager : MonoBehaviour
    {
        [SerializeField] Grid grid;
        // Start is called before the first frame update
        void Start()
        {
            grid.Init();
            grid.cellgrid[0, 0].Collapse();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
