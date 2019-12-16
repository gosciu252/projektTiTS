using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetIdentitySample.Models.Database
{
    public class Candidate
    {
        public long? ID { get; set; }
        public string FullName { get; set; }
        public int VotesCount { get; set; }
    }
}
