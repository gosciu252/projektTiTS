using AspNetIdentitySample.Data;
using AspNetIdentitySample.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetIdentitySample.Services
{
    /// <summary>
    /// Klasa służąca do operacji na kandydatach do głosowania w bazie danych.
    /// </summary>
    public class CandidatesService
    {
        private readonly ApplicationDbContext _context;

        public CandidatesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Candidate> GetList()
        {
            return _context.Candidates.ToList();
        }

        public void IncreaseCandidateVote(long candidateId)
        {
            var candidate = _context.Candidates.FirstOrDefault(c => c.ID == candidateId);
            candidate.VotesCount++;
            _context.SaveChanges();
        }
    }
}
