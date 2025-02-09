﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.JWT
{
    public class TokenOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int AccessTokenExpiration { get; set; }
        public string SecurityKey { get; set; }
        public int RefreshTokenTTL { get; set; } // in days

        public TokenOptions()
        {
            Audience = string.Empty;
            Issuer = string.Empty;
            SecurityKey = string.Empty;
        }

        public TokenOptions(string audience, string issuer, int accessTokenExpiration, string securityKey, int refreshTokenTTL)
        {
            Audience = audience;
            Issuer = issuer;
            SecurityKey = securityKey;
            RefreshTokenTTL = refreshTokenTTL;
            AccessTokenExpiration = accessTokenExpiration;
        }
    }
}
