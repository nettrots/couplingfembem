using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace SbB.Diploma.Base.Solvers.FastSolver
{
    public class Hash : IRow
    {

        // хэш-таблица в формате номер_ненулевой_ячейки = значение_ячейки

        private Hashtable hash = new Hashtable();



        public void setValue(int num, double value)
        {

            // если значение 0-вое, то удаляем данную ячейку

            if (value == 0)
            {

                if (hash.ContainsKey(num)) hash.Remove(num);

                return;

            }



            // если значение не 0-вое, то перезаписываем или добавляем ячейку

            hash[num] = value;

        }



        public void addValue(int num, double value)
        {

            if (hash.ContainsKey(num)) hash[num] = (double)hash[num] + value;

            else hash[num] = value;

        }



        public double getValue(int num)
        {

            if (hash.ContainsKey(num)) return (double)hash[num];

            return 0;

        }



        public void getValues(ref int[] indexes, ref double[] values)
        {

            System.Collections.ICollection keys = hash.Keys;

            int count = keys.Count;

            indexes = new int[count];

            values = new double[count];

            int i = 0;

            for (IEnumerator it = keys.GetEnumerator(); it.MoveNext(); )
            {

                indexes[i] = (int)it.Current;

                values[i++] = (double)hash[it.Current];

            }

        }

    }

}
