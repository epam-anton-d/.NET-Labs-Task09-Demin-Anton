using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Three_Layers
{
    public class Client
    {
        /// <summary>
        /// Свойства клиента: ФИО, дата родения, банк.
        /// </summary>
        public string Fio { set; get; }
        public DateTime Birthday { set; get; }
        public string BankName { set; get; }
    }
}
