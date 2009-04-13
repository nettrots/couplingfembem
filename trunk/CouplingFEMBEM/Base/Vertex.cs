using System;

namespace SbB.Diploma
{
    /// <summary>
    /// Клас Vertex - вершина, двох-вимірна точка,
    /// радіус-вектор
    /// </summary>
    public class Vertex: ICloneable, IComparable
    {
        //Змінні класу
        private double x, y;
        private int number;
        private DoF dof;
        //конструктори
        /// <summary>
        /// Констуктор по замовчуванню
        /// </summary>
        public Vertex() { }
        /// <summary>
        /// Конструктор з заданням точки
        /// </summary>
        /// <param name="x">координата по x</param>
        /// <param name="y">координата по y</param>
        public Vertex(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// Конструктор з заданням точки та номером вузла
        /// </summary>
        /// <param name="x">координата по x</param>
        /// <param name="y">координата по y</param>
        /// <param name="number">номер вузла</param>
        public Vertex(double x, double y, int number)
        {
            this.x = x;
            this.y = y;
            this.number = number;
        }


        // Властивості (Properties)
        public double X
        {
            get { return x; }
            set { x = value; }
        }
        public double Y
        {
            get { return y; }
            set { y = value; }
        }
        public int Number
        {
            get { return number; }
            set { number = value; }
        }
        /// <summary>
        /// Довжина радіус-вектора
        /// </summary>
        public double Length
        {
            get { return Math.Sqrt(x * x + y * y); }
        }

        public DoF Dof
        {
            get { return dof; }
            set { dof = value; }
        }


        // оператори
        // алгебраїчні оператори
        public static Vertex operator +(Vertex pLeft, Vertex pRight)
        {
            return new Vertex(pLeft.x + pRight.x, pLeft.y + pRight.y);
        }
        public static Vertex operator -(Vertex pLeft, Vertex pRight)
        {
            return new Vertex(pLeft.x - pRight.x, pLeft.y - pRight.y);
        }
        public static Vertex operator *(double k, Vertex pRight)
        {
            return new Vertex(k * pRight.x, k * pRight.y);
        }
        public static Vertex operator *(Vertex pLeft, double k)
        {
            return new Vertex(k * pLeft.x, k * pLeft.y);
        }
        // оператори відношення
        public static bool operator ==(Vertex pLeft, Vertex pRight)
        {
            if ((object)pLeft == null || (object)pRight == null) return (object)pLeft == (object)pRight;
            return (Math.Abs(pLeft.x - pRight.x) < Constants.EPS) &&
                    (Math.Abs(pLeft.y - pRight.y) < Constants.EPS);
        }
        public static bool operator !=(Vertex pLeft, Vertex pRight)
        {
            if ((object)pLeft == null || (object)pRight == null) return (object)pLeft != (object)pRight;
            return (Math.Abs(pLeft.x - pRight.x) > Constants.EPS) ||
                    (Math.Abs(pLeft.y - pRight.y) > Constants.EPS);
        }
        public static bool operator <(Vertex pLeft, Vertex pRight)
        {
            return (pLeft.x < pRight.x) || ((pLeft.x == pRight.x) && (pLeft.y < pRight.y));
        }
        public static bool operator >(Vertex pLeft, Vertex pRight)
        {
            return (pLeft.x > pRight.x) || ((pLeft.x == pRight.x) && (pLeft.y > pRight.y));
        }
        public static bool operator <=(Vertex pLeft, Vertex pRight)
        {
            return (pLeft.x <= pRight.x) || ((pLeft.x == pRight.x) && (pLeft.y <= pRight.y));
        }
        public static bool operator >=(Vertex pLeft, Vertex pRight)
        {
            return (pLeft.x >= pRight.x) || ((pLeft.x == pRight.x) && (pLeft.y >= pRight.y));
        }


        // Методи
        // позиція точки відносно прямої
        public VertexPos classify(Vertex pa, Vertex pb)
        {
            Vertex p = new Vertex(this.x, this.y);
            Vertex a = pb - pa;
            Vertex b = p - pa;
            double sa = a.X * b.Y - b.X * a.Y;
            if (sa > 10e-10)
                return VertexPos.LEFT;
            if (sa < -10e-10)
                return VertexPos.RIGHT;
            if ((a.X * b.X < -10e-10) || (a.Y * b.Y < -10e-10))
                return VertexPos.BEHIND;
            if (a.Length < b.Length)
                return VertexPos.BEYOND;
            if (pa == p)
                return VertexPos.ORIGIN;
            if (pb == p)
                return VertexPos.DESTINATION;
            return VertexPos.BETWEEN;

        }
        // Перевантаженні методи
        // символьне представлення (перевантажена від System.Object)
        public override string ToString()
        {
            //return "{ x = " + x + ",\ty = " + y +  ";   \tn = " + number + " }";
            //return "{ x = " + x + ",\ty = " + y + /* ";   \tn = " + number +*/ " }";
            //return "{ x = " + x + ",y = " + y + /* ";   \tn = " + number +*/ " }";
            return "{ " + x + "; " + y + /* ";   \tn = " + number +*/ " }";

        }
     
        // порівняння об'єктів
        public override bool Equals(object obj)
        {
            return this == (Vertex)obj;
        }
        // хеш-код
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }


        // інтерфейси
        // ICloneable
        public object Clone()
        {
            return new Vertex(x, y, number);
        }
        // IComparable
        public int CompareTo(object o)
        {
            Vertex temp = (Vertex)o;
            if (this > temp)
                return 1;
            if (this < temp)
                return -1;
            return 0;
        }


    }

}
