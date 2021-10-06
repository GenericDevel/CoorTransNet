
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;//与PointF 联系起来了


namespace CoorTransNet //这里改成和您的namespace 名字一样即可
{
    public class Point
    {
        private double x;    //coordinate
        private double y;
        private double z;
       
        public double X
        { get { return x; } set { x = value; } }
        public double Y
        { get { return y; } set { y = value; } }
        public double Z
        { get { return z; } set { z = value; } }

        //The constructor
        public Point(double x = 0, double y = 0, double z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

    }

}

