using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IoT_Lib.CustomClass
{
    public class DHT22
    {
        public DHT22()
        {

        }
        public DHT22(double temp,double hum)
        {
            this.Temp = temp;
            this.Hum = hum;
        }
        public double Temp { get; set; }
        public double Hum { get; set; }
    }
}
