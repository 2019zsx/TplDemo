using Microsoft.IdentityModel.Tokens;
using SqlSugar;
using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TplDemo.Common.Config;
using TplDemo.Common.IsWhatExtenions;
using TplDemo.Common.TokenModel;
using TplDemo.Model.ViewModel;

namespace TplDemo.Extensions.Authorization
{
    public class JwtHelper : BaseConfigModel
    {
        /// <summary>获取Token</summary>
        /// <param name="uid">用户id</param>
        /// <param name="requesttype">类型（web或app）</param>
        /// <returns></returns>
        public static ViewToken GetToken(Userinfo userinfomodel, string requesttype)
        {
            ViewToken viewToken = new ViewToken();
            var requestTypedata = GetAppSettings(requesttype);
            if (!requestTypedata.success)
            {
                viewToken.success = false;
                viewToken.msg = "当前选择登录类型有误";
                return viewToken;
            }
            DateTime time = DateTime.Now;
            //验证过期时间
            DateTime Expires = time.AddMinutes(int.Parse(requestTypedata.Tokenexp.ToString()));
            //刷新过期时间
            DateTime frefreshExpires = time.AddMinutes(int.Parse(requestTypedata.refreshTokenexp.ToString()));
            viewToken.access_token = "Bearer " + Generate(Expires, userinfomodel);
            viewToken.expires_in = Expires;
            viewToken.refreshtoken = "Bearer " + Generate(frefreshExpires, new Userinfo() { roleid = 0, uid = 0, username = "" });
            viewToken.success = true;
            return viewToken;
        }

        /// <summary>生成token</summary>
        /// <param name="Expires"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static string Generate(DateTime Expires, Userinfo userinfomodel)
        {
            var claims = new List<Claim>(new[] {
                          new Claim(JwtRegisteredClaimNames.Sub,userinfomodel.roleid.ToString()),
                          new Claim(JwtRegisteredClaimNames.Jti,userinfomodel.uid.ToString()),
                          new Claim(ClaimTypes.Name,userinfomodel.username) });
            var jwtConfig = BaseConfigModel.jwtConfig;
            var tokenHandler = new JwtSecurityTokenHandler();
            //var tokenDescriptor = new JwtSecurityToken
            //{
            //    //载体相关的内容
            //    claims = Claimsarr.ToArray(),
            //    Expires = Expires,//过期时间
            //    Issuer = jwtConfig.Issuer,//发行人
            //    Audience = jwtConfig.Audience,
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.JWTSecretKey)), SecurityAlgorithms.HmacSha256Signature)
            //};

            var jwt = new JwtSecurityToken(
               issuer: jwtConfig.Issuer,//发行人
               audience: jwtConfig.Audience,
               claims: claims,
               notBefore: DateTime.Now,
               expires: Expires,
               signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.JWTSecretKey)), SecurityAlgorithms.HmacSha256Signature)
           );
            string token = tokenHandler.WriteToken(jwt);
            return token;
        }

        /// <summary>
        ///获取对应的过期时间
        /// </summary>
        /// <param name="strrequestType"></param>
        /// <returns></returns>
        public static RequestTypeModel GetAppSettings(string strrequestType)
        {
            var jwtConfig = BaseConfigModel.jwtConfig;

            RequestTypeModel requestTypeModel = new RequestTypeModel();
            strrequestType = strrequestType.ToLower();  //大写转小写
            switch (strrequestType)
            {
                case "app":
                    requestTypeModel.Tokenexp = jwtConfig.AppExp.ObjToInt();
                    requestTypeModel.refreshTokenexp = jwtConfig.AppRefresh.ObjToInt();
                    requestTypeModel.success = true;
                    break;

                case "web":
                    requestTypeModel.Tokenexp = jwtConfig.WebExp.ObjToInt();
                    requestTypeModel.refreshTokenexp = jwtConfig.WebRefresh.ObjToInt();
                    requestTypeModel.success = true;
                    break;
            }
            return requestTypeModel;
        }

        /// <summary>验证Token信息</summary>
        /// <param name="token"></param>
        /// <param name="Principal"></param>
        /// <returns></returns>
        public static bool ValidateToken(string token)
        {
            var principal = GetPrincipal(token);
            var identity = principal?.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return false;
            }
            if (!identity.IsAuthenticated)
            {
                return false;
            }
            return true;
        }

        /// <summary>验证token</summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var jwtConfig = BaseConfigModel.jwtConfig;
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token.replaceBearer()) as JwtSecurityToken;
                if (jwtToken == null)
                {
                    return null;
                }
                var validationParams = new TokenValidationParameters()
                {
                    ValidAudience = jwtConfig.Audience,
                    ValidateAudience = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.JWTSecretKey)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                SecurityToken securityToken;
                var pincipal = tokenHandler.ValidateToken(token.replaceBearer(), validationParams, out securityToken);
                return pincipal;
            }
            catch (SecurityTokenValidationException ex)
            {
                //if (ex is SecurityTokenInvalidLifetimeException)
                //{
                //    return null;
                //}

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}