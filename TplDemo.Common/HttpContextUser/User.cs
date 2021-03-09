using Microsoft.AspNetCore.Http;
using SqlSugar;
using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TplDemo.Common.IsWhatExtenions;
using TplDemo.Common.TokenModel;

namespace TplDemo.Common.HttpContextUser
{
    public class User : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public User(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string username => GetSerializeJwt("", true).username;

        public int uid => GetSerializeJwt("", true).uid.ObjToInt();
        public int role => GetSerializeJwt("", true).roleid.ObjToInt();

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public string GetToken()
        {
            return _accessor.HttpContext.Request.Headers["Authorization"].ObjToString().Replace("Bearer ", "");
        }

        public List<string> GetUserInfoFromToken(string ClaimType)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            if (!string.IsNullOrEmpty(GetToken()))
            {
                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(GetToken());
                return (from item in jwtToken.Claims
                        where item.Type == ClaimType
                        select item.Value).ToList();
            }
            else
            {
                return new List<string>() { };
            }
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            var data = _accessor.HttpContext.User.Claims;
            return data;
        }

        public List<string> GetClaimValueByType(string ClaimType)
        {
            return (from item in GetClaimsIdentity()
                    where item.Type == ClaimType
                    select item.Value).ToList();
        }

        #region 解析 token 获取用户信息

        /// <summary>解析 token 获取用户信息</summary>
        /// <param name="token"></param>
        /// <param name="isgetauthorizationtoke">默认是自己传入 ，true 是 Headers 获取</param>
        /// <returns></returns>
        public Userinfo GetSerializeJwt(string token, bool isgetauthorizationtoke = false)
        {
            if (isgetauthorizationtoke)
            {
                token = GetToken();
            }
            Userinfo userinfo = new Userinfo();
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(token.replaceBearer());
            object uid;
            object roleid;
            object username;
            try
            {
                jwtToken.Payload.TryGetValue(JwtRegisteredClaimNames.Jti, out uid);
                jwtToken.Payload.TryGetValue(JwtRegisteredClaimNames.Sub, out roleid);
                jwtToken.Payload.TryGetValue(ClaimTypes.Name, out username);

                #region 填充用户信息

                userinfo.uid = uid.ObjToInt();
                userinfo.roleid = roleid.ObjToInt();
                userinfo.username = username.ToString();

                #endregion 填充用户信息

                return userinfo;
            }
            catch (Exception e)
            {
                return userinfo;
            }
        }

        #endregion 解析 token 获取用户信息
    }
}