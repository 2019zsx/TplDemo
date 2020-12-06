using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using TplDemo.Common.TokenModel;

namespace TplDemo.Common.HttpContextUser
{
    public interface IUser
    {
        string username { get; }
        int uid { get; }
        int role { get; }

        bool IsAuthenticated();

        IEnumerable<Claim> GetClaimsIdentity();

        List<string> GetClaimValueByType(string ClaimType);

        string GetToken();

        List<string> GetUserInfoFromToken(string ClaimType);

        Userinfo GetSerializeJwt(string token, bool isgetauthorizationtoke = false);
    }
}