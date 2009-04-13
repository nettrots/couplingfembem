using System;
using System.Collections;
using System.Collections.Generic;

namespace SbB.Diploma
{
    public class Vector: List<double>, ICloneable//, IEnumerable, IEnumerator
    {
        #region Constructors
        public Vector() { }
        public Vector(int size): base(new double[size]) {}
        public Vector(int size, double element):base(size)
        {
            for (int i = 0; i < size; i++)
                this.Add(element);
        }
        public Vector(double[] elements): base(elements){}
        #endregion

        #region Properties
        public int Length
        {
            get { return this.Count; }
            set 
            {
                for (int i = this.Count; i <= value; i++)
                    this.Add(0.0);
                for (int i = this.Count; i > value; i--)
                    this.RemoveAt(i - 1);
            }
        }
        #endregion

        #region Operators
        //унарні
        public static Vector operator +(Vector v)
        {
            return (Vector) v.Clone();
        }
        public static Vector operator -(Vector v)
        {
            Vector temp = new Vector(v.Length);
            for (int i = 0; i < v.Length; i++)
                temp[i] = -v[i];
            return temp;
        }
        //бінарні
        public static Vector operator +(Vector vLeft, Vector vRight)
        {
            if (vLeft.Length != vRight.Length)
                throw new Exception("Different length");
            else
            {
                Vector rez = new Vector(vLeft.Length);
                for (int i = 0; i < vLeft.Length; i++)
                    rez[i] = vLeft[i] + vRight[i];
                return rez;
            }
        }
        public static Vector operator -(Vector vLeft, Vector vRight)
        {
            if (vLeft.Length != vRight.Length)
                throw new Exception("Different length");
            else
            {
                Vector rez = new Vector(vLeft.Length);
                for (int i = 0; i < vLeft.Length; i++)
                    rez[i] = vLeft[i] - vRight[i];
                return rez;
            }
        }
        //скалярне множення векторів
        public static double operator *(Vector vLeft, Vector vRight)
        {
            if (vLeft.Length != vRight.Length)
                throw new Exception("Different length");
            else
            {
                double rez = 0.0;
                for (int i = 0; i < vLeft.Length; i++)
                    rez += vLeft[i]*vRight[i];
                return rez;
            }
        }
        //множення на константу
        public static Vector operator *(double k, Vector vRight)
        {
            Vector rez = new Vector(vRight.Length);
            for (int i = 0; i < vRight.Length; i++)
                rez[i] = k*(vRight[i]);
            return rez;
        }
        public static Vector operator *(Vector vLeft, double k)
        {
            Vector rez = new Vector(vLeft.Length);
            for (int i = 0; i < vLeft.Length; i++)
                rez[i] = k * vLeft[i];
            return rez;
        }
        //ділення на константу
        public static Vector operator /(Vector vLeft, double k)
        {
            Vector rez = new Vector(vLeft.Length);
            for (int i = 0; i < vLeft.Length; i++)
                rez[i] = vLeft[i]/k;
            return rez;
        }
        //оператори відношення
        public static bool operator ==(Vector vLeft, Vector vRight)
        {
            if ((object)vLeft==null || (object)vRight==null) 
                return (object)vLeft == (object)vRight;
            if (vLeft.Length != vRight.Length)
                throw new Exception("Can't be compare");
            for (int i = 0; i < vLeft.Length; i++)
                if (vLeft[i] != vRight[i])
                    return false;
            return true;
        }
        public static bool operator !=(Vector vLeft, Vector vRight)
        {
            return !(vLeft == vRight);
        }
        #endregion

        #region Methods
        //занулення всіх елементів
        public void ZeroIn()
        {
            for (int i = 0; i < this.Count; i++)
                this[i] = 0.0;
        }
        //знаходження мінімуму в векторі
        public double Min()
        {
            double m = this[0];
            for (int i = 1; i < Length; i++)
                if (m > this[i])
                    m = this[i];
            return m;
        }
        //знаходження максимуму в векторі
        public double Max()
        {
            double m = this[0];
            for (int i = 1; i < Length; i++)
                if (m < this[i])
                    m = this[i];
            return m;
        }
        // норма
        public double Norm(Norma v)
        {
            if (v == Norma.Maximum)
            {
                double m = Math.Abs(this[0]);
                for (int i = 1; i < this.Count; i++)
                    m = m < Math.Abs(this[i]) ? Math.Abs(this[i]) : m;
                return m;
            }

            int deg = (int) v;
            double rez = 0;
            for (int i = 0; i < this.Count; i++)
                rez += Math.Pow(Math.Abs(this[i]), deg);
            rez = Math.Pow(rez, 1.0/deg);
            return rez;
        }
        public double Norm()
        {
            return Norm(Norma.Euclidean);
        }
        //символьне представлення (перевантажена від System.Object)
        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < this.Count; i++)
            {
                str += this[i].ToString() + " "; 
            }
            return str;
        }
        // порівняння об'єктів
        public override bool Equals(object obj)
        {
            return this == (Vector)obj;
        }
        // хеш-код
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        #endregion

        #region Interfaces
        //ICloneable
        public object Clone()
        {
            return new Vector(this.ToArray());
        }
        #endregion
    }
}
