using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SbB.Diploma.Base.Solvers.FastSolver
{
    public class Array : IRow
    {

        // массив индексов (номера ненулевых ячеек)

        private ArrayList indexes = new ArrayList();



        // массив значений ячеек

        private ArrayList values = new ArrayList();





        public void setValue(int num, double value)
        {

            int i = indexes.IndexOf(num);



            // если записывается 0-вое значение, то удаляем данную ячейку

            if (value == 0)
            {

                if (i != -1)
                {

                    indexes.RemoveAt(i);

                    values.RemoveAt(i);

                }

                return;

            }



            // если значение не 0-вое, то перезаписываем или добавляем ячейку

            if (i != -1)
            {

                values[i] = value;

            }

            else
            {

                indexes.Add(num);

                values.Add(value);

            }

        }





        public void addValue(int num, double value)
        {

            int i = indexes.IndexOf(num);

            if (i != -1)
            {

                values[i] = (double)values[i] + value;

            }

            else
            {

                indexes.Add(num);

                values.Add(value);

            }

        }



        public double getValue(int num)
        {

            int i = indexes.IndexOf(num);

            if (i != -1) return (double)values[i];

            return 0;

        }



        public void getValues(ref int[] indexes, ref double[] values)
        {

            indexes = (int[])this.indexes.ToArray(System.Type.GetType("System.Int32"));

            values = (double[])this.values.ToArray(System.Type.GetType("System.Double"));

        }

    }

}
