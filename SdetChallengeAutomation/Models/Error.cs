using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SdetChallengeAutomation.Models
{
    public class Error
    {
        public string code { get; set; }
        public bool error  { get; set; }
        public string message { get; set; }
        public Data data { get; set; }

    }
    public class Data
    {
        public string query { get; set; }
        public object position { get; set; }
    }
}
