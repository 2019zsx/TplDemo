using Essensoft.AspNetCore.Payment.WeChatPay;
using Essensoft.AspNetCore.Payment.WeChatPay.V2;
using Essensoft.AspNetCore.Payment.WeChatPay.V2.Notify;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TplDemo.Controllers
{
    /// <summary>微信回调</summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WeChatPayNotifyController : Controller
    {
        private readonly IWeChatPayNotifyClient _client;
        private readonly IOptions<WeChatPayOptions> _optionsAccessor;

        public WeChatPayNotifyController(IWeChatPayNotifyClient client, IOptions<WeChatPayOptions> optionsAccessor)
        {
            _client = client;
            _optionsAccessor = optionsAccessor;
        }

        /// <summary>统一下单支付结果通知</summary>
        [Route("unifiedorder")]
        [HttpPost]
        public async Task<IActionResult> Unifiedorder()
        {
            try
            {
                Request.EnableBuffering();

                Request.Body.Seek(0, SeekOrigin.Begin);
                var notify = await _client.ExecuteAsync<WeChatPayUnifiedOrderNotify>(Request, _optionsAccessor.Value);
                if (notify.ReturnCode == WeChatPayCode.Success)
                {
                    if (notify.ResultCode == WeChatPayCode.Success)
                    {
                        Console.WriteLine("OutTradeNo: " + notify.OutTradeNo);

                        return WeChatPayNotifyResult.Success;
                    }
                }

                return WeChatPayNotifyResult.Failure;
            }
            catch
            {
                return WeChatPayNotifyResult.Failure;
            }
        }

        /// <summary>退款结果通知</summary>
        [Route("refund")]
        [HttpPost]
        public async Task<IActionResult> Refund()
        {
            try
            {
                Request.EnableBuffering();

                var notify = await _client.ExecuteAsync<WeChatPayRefundNotify>(Request, _optionsAccessor.Value);
                if (notify.ReturnCode == WeChatPayCode.Success)
                {
                    if (notify.RefundStatus == WeChatPayCode.Success)
                    {
                        Console.WriteLine("OutTradeNo: " + notify.OutTradeNo);
                        return WeChatPayNotifyResult.Success;
                    }
                }

                return WeChatPayNotifyResult.Failure;
            }
            catch
            {
                return WeChatPayNotifyResult.Failure;
            }
        }
    }
}