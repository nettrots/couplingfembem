using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SbB.Diploma.Base.Solvers.FastSolver
{
    public interface IRow
    {

        // записывает значение value в ячейку с индексом num

        void setValue(int num, double value);



        // добавляет value к существующему значению в ячейке num 

        void addValue(int num, double value);



        // возвращает значение ячейки num

        double getValue(int num);



        // возвращает все ненулевые ячейки строки/столбца: 

        // индексы ячеек - в indexes, значения в values

        void getValues(ref int[] indexes, ref double[] values);

    }  

}
