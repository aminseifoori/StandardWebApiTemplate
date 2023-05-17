using AutoMapper;
using Domain.ConfigurationModels;
using Domain.Exceptions;
using Domain.Models;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;
using Shared.Dtos.Users;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserAccountService : IUserAccountService
    {
        private readonly ILoggerManager logger;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly IOptions<JwtConfiguration> iOptionJWTconfiguration; // to be replaced with the below line
        private readonly IConfiguration configuration;
        private User? user;
        //To use configuration binding we add the below line.
        private readonly JwtConfiguration _jwtConfiguration;

        public UserAccountService(ILoggerManager _logger,
            IMapper _mapper, UserManager<User> _userManager,
            IConfiguration _configuration,
            IOptions<JwtConfiguration> _IOptionJWTconfiguration)
        {
            logger = _logger;
            mapper = _mapper;
            userManager = _userManager;
            iOptionJWTconfiguration = _IOptionJWTconfiguration;

            configuration = _configuration;

            //to use configuration binding
            //_jwtConfiguration = new JwtConfiguration();
            //_configuration.Bind(_jwtConfiguration.Section, _jwtConfiguration); //Bind needs Microsoft.Extensions.Configuration.Binder

            // as we desicded to use IOption configuration we need to comment the above lines 

            _jwtConfiguration = _IOptionJWTconfiguration.Value;

        }

        public async Task<IdentityResult> RegisterUser(CreateUserDto createUserDto)
        {
            var user = mapper.Map<User>(createUserDto);

            var result = await userManager.CreateAsync(user, createUserDto.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRolesAsync(user, createUserDto.Roles);
            }

            return result;
        }

        //To support refresh token the below changes should be added.
        //public async Task<string> CreateToken()
        //{
        //    var signingCredentials = GetSigningCredentials();
        //    var claims = await GetClaims();
        //    var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        //    return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        //}

        public async Task<TokenDto> CreateToken(bool populateExp)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            if (populateExp)
            {
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            }
            await userManager.UpdateAsync(user); 
            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return new TokenDto 
            { 
               AccessToken = accessToken,
               RefreshToken = refreshToken 
            };
        }

        public async Task<bool> ValidateUser(LoginDto userForAuth)
        {
            user = await userManager.FindByNameAsync(userForAuth.UserName);
            var result = (user != null && await userManager.CheckPasswordAsync(user, userForAuth.Password));
            if (!result)
                logger.LogWarning($"{nameof(ValidateUser)}: Authentication failed. Wrong user name or password.");
            return result;
        }

        public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        { 
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var _user = await userManager.FindByNameAsync(principal.Identity.Name);
            if (_user == null || _user.RefreshToken != tokenDto.RefreshToken || _user.RefreshTokenExpiryTime <= DateTime.Now)
                throw new RefreshTokenBadRequest();
            user = _user;
            return await CreateToken(populateExp: false);
        }

        private SigningCredentials GetSigningCredentials()
        {
            //var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET2"));
            var key = Encoding.UTF8.GetBytes(configuration.GetSection("JwtSecters:JwtSecters").Value);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        { 
            //var jwtSettings = configuration.GetSection("JwtSettings"); for using configuration binding we can remove this line
            var tokenOptions = new JwtSecurityToken
                (
                    //issuer: jwtSettings["validIssuer"], for using configuration binding we can remove this line
                    //audience: jwtSettings["validAudience"], for using configuration binding we can remove this line
                    issuer: _jwtConfiguration.ValidIssuer, 
                    audience: _jwtConfiguration.ValidAudience,
                    claims: claims,
                    //expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"]) for using configuration binding we can remove this line
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
                    signingCredentials: signingCredentials
                ); 
            return tokenOptions; 
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            //var jwtSettings = configuration.GetSection("JwtSettings"); this can be removed when we added the configuration binding
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtSecters:JwtSecters").Value)),
                ValidateLifetime = true, //if you want to allow the refresh token functionality for the expired token as well, set this property to false.
                //ValidIssuer = jwtSettings["validIssuer"], this can be removed when we added the configuration binding
                //ValidAudience = jwtSettings["validAudience"] this can be removed when we added the configuration binding
                ValidIssuer = _jwtConfiguration.ValidIssuer,
                ValidAudience = _jwtConfiguration.ValidAudience
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }
    }
}
