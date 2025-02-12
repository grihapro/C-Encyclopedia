using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LAB4
{
    //Интерфейс для абстрагирования от внутренней структуры модели
    internal interface IBIRDItem
    {
        //Все модели могут удалять из своей внутренней коллекции
        void RemoveItem(object o);
    }
}
