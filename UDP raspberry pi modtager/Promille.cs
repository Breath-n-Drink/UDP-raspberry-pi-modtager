// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace UDP_raspberry_pi_modtager
{
    public partial class Promille
    {
        public int Id { get; set; }
        public int DrinkerId { get; set; }
        public double Promille1 { get; set; }
        public DateTime Time { get; set; }

        public virtual Drinkers Drinker { get; set; }
    }
}