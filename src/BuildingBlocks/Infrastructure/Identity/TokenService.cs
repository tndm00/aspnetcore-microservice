﻿using Contracts.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared.Configurations;
using Shared.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public TokenResponse GetToken(TokenRequest request)
        {
            var token = GenerateJwt();
            var result = new TokenResponse(token);
            return result;
        }

        private string GenerateJwt() => GenerateEncrytedToken(GetSigningCredential());

        private string GenerateEncrytedToken(SigningCredentials signingCredentials)
        {
            var token = new JwtSecurityToken(
                signingCredentials: signingCredentials
                );
            var tokenHanler = new JwtSecurityTokenHandler();
            return tokenHanler.WriteToken(token);
        }

        private SigningCredentials GetSigningCredential()
        {
            byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }
    }
}
