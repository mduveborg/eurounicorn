using EurounicornAPI.CouchDB;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EurounicornAPI.Authentication
{

    public class TokenService
    {
        readonly CouchDBService db;
        readonly TimeSpan expiration;
        public TokenService(CouchDBService db)
        {
            this.db = db;
            expiration = TimeSpan.FromDays(30);
        }
        public IUserIdentity FindUser(string token)
        {
            var matches = db.FindByToken<TokenDto>(PasswordHash.CreateHash(token));
            foreach (var match in matches)
                return new UserIdentity() { UserName = match.Username };
            return null;
        }

        private IUserIdentity Validate(TokenDto token)
        {
            if (DateTime.UtcNow - token.Created > expiration)
            {
                db.Delete(token);
                return null;
            }
            return new UserIdentity
            {
                UserName = token.Username
            };
        }

        public string Login(string username)
        {
            var token = Cryptography.GenerateToken();
            var tokenDto = new TokenDto
            {
                Token = PasswordHash.CreateHash(token),
                Created = DateTime.UtcNow,
                Username = username
            };
            db.Set<TokenDto>(tokenDto);
            return token;
        }
    }
}