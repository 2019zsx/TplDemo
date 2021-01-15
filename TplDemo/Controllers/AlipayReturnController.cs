﻿using Essensoft.AspNetCore.Payment.Alipay;
using Essensoft.AspNetCore.Payment.Alipay.Notify;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TplDemo.Controllers
{
    /// <summary>支付宝回调</summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AlipayReturnController : Controller
    {
        private readonly IAlipayNotifyClient _client;
        private readonly IOptions<AlipayOptions> _optionsAccessor;

        public AlipayReturnController(IAlipayNotifyClient client, IOptions<AlipayOptions> optionsAccessor)
        {
            _client = client;
            _optionsAccessor = optionsAccessor;
        }

        /// <summary>电脑网站支付 - 同步跳转</summary>
        [Route("pagepay")]
        [HttpGet]
        public async Task<IActionResult> PagePay()
        {
            try
            {
                var notify = await _client.ExecuteAsync<AlipayTradePagePayReturn>(Request, _optionsAccessor.Value);
                ViewData["response"] = "支付成功";
                return View();
            }
            catch
            {
                ViewData["response"] = "出现错误";
                return View();
            }
        }

        /// <summary>手机网站支付 - 同步跳转</summary>
        [HttpGet]
        [Route("wappay")]
        public async Task<IActionResult> WapPay()
        {
            try
            {
                var notify = await _client.ExecuteAsync<AlipayTradeWapPayReturn>(Request, _optionsAccessor.Value);
                ViewData["response"] = "支付成功";
                return View();
            }
            catch
            {
                ViewData["response"] = "出现错误";
                return View();
            }
        }
    }
}