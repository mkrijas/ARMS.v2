using ArmsModels.BaseModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ARMS.JwtHelpers
{
    internal static class JwtHelpers
    {
        public static IEnumerable<Claim> GetClaims(this UserModel userAccounts, Guid Id)
        {
            IEnumerable<Claim> claims = new Claim[]
                    {
                new Claim("UserID",userAccounts.UserID.ToString()),
                new Claim(ClaimTypes.Name, userAccounts.UserName),
                new Claim(ClaimTypes.Email, userAccounts.Email),
                new Claim(ClaimTypes.NameIdentifier,Id.ToString()),
                new Claim(ClaimTypes.Expiration,DateTime.UtcNow.AddDays(1).ToString("MMM ddd dd yyyy HH:mm:ss tt") )
                    };
            return claims;
        }
        public static IEnumerable<Claim> GetClaims(this UserModel userAccounts, out Guid Id)
        {
            Id = Guid.NewGuid();
            return GetClaims(userAccounts, Id);
        }
        public static UserModel GenTokenkey(UserModel model, JwtSettings jwtSettings)
        {
            try
            {
                var UserToken = new UserModel();
                if (model == null) throw new ArgumentException(nameof(model));

                // Get secret key
                var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings.IssuerSigningKey);
                Guid Id = Guid.Empty;
                DateTime expireTime = DateTime.UtcNow.AddDays(1);
                UserToken.Validity = expireTime.TimeOfDay;
                var JWToken = new JwtSecurityToken(
                    issuer: jwtSettings.ValidIssuer,
                    audience: jwtSettings.ValidAudience,
                    claims: GetClaims(model, out Id),
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(expireTime).DateTime,
                    signingCredentials: new SigningCredentials
                    (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                );

                UserToken.Token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                var idRefreshToken = Guid.NewGuid();


                UserToken.UserName = model.UserName;
                UserToken.UserID = model.UserID;
                //UserToken.GuidId = Id;
                var refreshToken = new UserModel
                {
                    Token = Guid.NewGuid().ToString(),
                    UserName = UserToken.UserName,
                    Email = UserToken.Email,
                    UserID = UserToken.UserID,
                    ExpiredTime = DateTime.UtcNow.AddMonths(6)
                };

                UserToken.RefreshToken = refreshToken.Token;
                return UserToken;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
