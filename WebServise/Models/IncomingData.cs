using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServise.Models
{
    /// <summary>
    /// Данные переданные через GET
    /// </summary>
    public class IncomingData
    {

        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string Mode { get; set; }
    }
    /// <summary>
    /// Распарсенные данные
    /// </summary>
    public class DataValue
    {
        public string code { get; set; }
        public DateTime date { get; set; }
        public double rate { get; set; }
    }
}