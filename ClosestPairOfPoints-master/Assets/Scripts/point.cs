using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using UnityEngine;

public class Point : MonoBehaviour
{



        public float y;
        public float x;
        public Point(float _x, float _y)
        {
            x = _x;
            y = _y;
        }
        public float getX()
        {
            return x;
        }
        public float getY()
        {
            return y;
        }

}
