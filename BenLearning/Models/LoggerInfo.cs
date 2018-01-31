using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenLearning.Models
{
    class LoggerInfo
    {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string LoggerName { get; set; }

        public string LoggerSerial { get; set; }

        public string Password { get; set; }



    }
}