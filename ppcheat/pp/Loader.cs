using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace pp
{
    public class Loader
    {
        public static GameObject Load;

        public static void Init()
        {
            Load = new GameObject();

            Load.AddComponent<UI>();
            UnityEngine.Object.DontDestroyOnLoad(Load);
        }

        public static void Unload()
        {
            UnityEngine.Object.Destroy(Load);
        }
    }
}
