using AspNetIdentitySample.Data;
using AspNetIdentitySample.Models;
using AspNetIdentitySample.Models.Database;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetIdentitySample.Services
{
    /// <summary>
    /// Klasa służąca do operacji na tokenach w bazie danych.
    /// </summary>
    public class TokensService
    {
        private ApplicationDbContext _context;
        private readonly SHA512Encryption _sha512;

        public TokensService(ApplicationDbContext context, SHA512Encryption sha512)
        {
            _context = context;
            _sha512 = sha512;
        }

        public void AddTokenForUser(string token)
        {
            var tokenHash = _sha512.Hash(token);

            _context.Tokens.Add(new Token
            {
                TokenHash = tokenHash
            });

            _context.SaveChanges();
        }

        public Token FindTokenByValue(string token)
        {
            var tokenHash = _sha512.Hash(token);

            return _context.Tokens.FirstOrDefault(t => t.TokenHash == tokenHash);
        }
        
        public void RemoveToken(long id)
        {
            var toRemove = _context.Tokens.FirstOrDefault(t => t.ID == id);
            if (toRemove != null)
            {
                _context.Tokens.Remove(toRemove);
                _context.SaveChanges();
            }
        }

        public UserTokenInfo GetTokenInfoForUser(string userId)
        {
            var user = _context.Users.First(u => u.Id == userId);

            return new UserTokenInfo
            {
                UserId = user.Id,
                TokenGenerated = user.TokenGenerated
            };
        }
    }
}
