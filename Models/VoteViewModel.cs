using AspNetIdentitySample.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetIdentitySample.Models
{
    public class VoteViewModel
    {
        public List<Candidate> Candidates { get; set; }
        [Required(ErrorMessage = "Proszę zaznaczyć kandydata")]
        public long? CandidateId { get; set; }
    }
}
