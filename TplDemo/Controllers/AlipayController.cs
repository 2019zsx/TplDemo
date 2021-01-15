using Essensoft.AspNetCore.Payment.Alipay;
using Essensoft.AspNetCore.Payment.Alipay.Domain;
using Essensoft.AspNetCore.Payment.Alipay.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TplDemo.Model.ViewModel.ViewPayment;

namespace TplDemo.Controllers
{
    /// <summary>支付宝下单控制器&gt;https://gitee.com/essensoft/payment(支付文档说明)</summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AlipayController : ControllerBase
    {
        /// <summary>https://gitee.com/essensoft/payment(支付文档说明)</summary>
        private readonly IAlipayClient _client;

        private readonly IOptions<AlipayOptions> _optionsAccessor;

        /// <summary></summary>
        /// <param name="client"></param>
        /// <param name="optionsAccessor"></param>
        public AlipayController(IAlipayClient client, IOptions<AlipayOptions> optionsAccessor)
        {
            _client = client;
            _optionsAccessor = optionsAccessor;
        }

        #region 当面付-扫码支付

        /// <summary>当面付-扫码支付</summary>
        [HttpPost]
        public async Task<IActionResult> PreCreate(AlipayTradePreCreateViewModel viewModel)
        {
            var model = new AlipayTradePrecreateModel
            {
                OutTradeNo = viewModel.OutTradeNo,
                Subject = viewModel.Subject,
                TotalAmount = viewModel.TotalAmount,
                Body = viewModel.Body
            };
            var req = new AlipayTradePrecreateRequest();
            req.SetBizModel(model);
            req.SetNotifyUrl(viewModel.NotifyUrl);

            var response = await _client.CertificateExecuteAsync(req, _optionsAccessor.Value);
            // ViewData["qrcode"] = response.QrCode; ViewData["response"] = response.Body;
            return Ok(response.Body);
        }

        #endregion 当面付-扫码支付

        #region 当面付-二维码/条码/声波支付

        /// <summary>当面付-二维码/条码/声波支付</summary>
        [HttpPost]
        public async Task<IActionResult> Pay(AlipayTradePayViewModel viewModel)
        {
            var model = new AlipayTradePayModel
            {
                OutTradeNo = viewModel.OutTradeNo,
                Subject = viewModel.Subject,
                Scene = viewModel.Scene,
                AuthCode = viewModel.AuthCode,
                TotalAmount = viewModel.TotalAmount,
                Body = viewModel.Body
            };
            var req = new AlipayTradePayRequest();
            req.SetBizModel(model);

            var response = await _client.CertificateExecuteAsync(req, _optionsAccessor.Value);
            return Ok(response.Body);
        }

        #endregion 当面付-二维码/条码/声波支付

        #region APP支付

        /// <summary>APP支付</summary>
        [HttpPost]
        public async Task<IActionResult> AppPay(AlipayTradeAppPayViewModel viewModel)
        {
            var model = new AlipayTradeAppPayModel
            {
                OutTradeNo = viewModel.OutTradeNo,
                Subject = viewModel.Subject,
                ProductCode = viewModel.ProductCode,
                TotalAmount = viewModel.TotalAmount,
                Body = viewModel.Body
            };
            var req = new AlipayTradeAppPayRequest();
            req.SetBizModel(model);
            req.SetNotifyUrl(viewModel.NotifyUrl);

            var response = await _client.SdkExecuteAsync(req, _optionsAccessor.Value);
            //将response.Body给 ios/android端 由其去调起支付宝APP(https://docs.open.alipay.com/204/105296/ https://docs.open.alipay.com/204/105295/)
            return Ok(response.Body);
        }

        #endregion APP支付

        /// <summary>电脑网站支付</summary>
        /// <param name="viewModel"></param>
        [HttpPost]
        public async Task<IActionResult> PagePay(AlipayTradePagePayViewModel viewModel)
        {
            var model = new AlipayTradePagePayModel
            {
                Body = viewModel.Body,
                Subject = viewModel.Subject,
                TotalAmount = viewModel.TotalAmount,
                OutTradeNo = viewModel.OutTradeNo,
                ProductCode = viewModel.ProductCode
            };
            var req = new AlipayTradePagePayRequest();
            req.SetBizModel(model);
            req.SetNotifyUrl(viewModel.NotifyUrl);
            req.SetReturnUrl(viewModel.ReturnUrl);

            var response = await _client.PageExecuteAsync(req, _optionsAccessor.Value);
            return Content(response.Body, "text/html", Encoding.UTF8);
        }

        #region 手机网站支付

        /// <summary>手机网站支付</summary>
        [HttpPost]
        public async Task<IActionResult> WapPay(AlipayTradeWapPayViewModel viewMode)
        {
            var model = new AlipayTradeWapPayModel
            {
                Body = viewMode.Body,
                Subject = viewMode.Subject,
                TotalAmount = viewMode.TotalAmount,
                OutTradeNo = viewMode.OutTradeNo,
                ProductCode = viewMode.ProductCode
            };
            var req = new AlipayTradeWapPayRequest();
            req.SetBizModel(model);
            req.SetNotifyUrl(viewMode.NotifyUrl);
            req.SetReturnUrl(viewMode.ReturnUrl);

            var response = await _client.PageExecuteAsync(req, _optionsAccessor.Value);
            return Content(response.Body, "text/html", Encoding.UTF8);
        }

        #endregion 手机网站支付

        #region 交易查询

        /// <summary>交易查询</summary>
        [HttpPost]
        public async Task<IActionResult> Query(AlipayTradeQueryViewModel viewMode)
        {
            var model = new AlipayTradeQueryModel
            {
                OutTradeNo = viewMode.OutTradeNo,
                TradeNo = viewMode.TradeNo
            };

            var req = new AlipayTradeQueryRequest();
            req.SetBizModel(model);

            var response = await _client.CertificateExecuteAsync(req, _optionsAccessor.Value);
            return Ok(response.Body);
        }

        #endregion 交易查询

        #region 交易退款

        /// <summary>交易退款</summary>
        [HttpPost]
        public async Task<IActionResult> Refund(AlipayTradeRefundViewModel viewMode)
        {
            var model = new AlipayTradeRefundModel
            {
                OutTradeNo = viewMode.OutTradeNo,
                TradeNo = viewMode.TradeNo,
                RefundAmount = viewMode.RefundAmount,
                OutRequestNo = viewMode.OutRequestNo,
                RefundReason = viewMode.RefundReason
            };

            var req = new AlipayTradeRefundRequest();
            req.SetBizModel(model);

            var response = await _client.CertificateExecuteAsync(req, _optionsAccessor.Value);
            return Ok(response.Body);
        }

        #endregion 交易退款

        #region 退款查询

        /// <summary>退款查询</summary>
        [HttpPost]
        public async Task<IActionResult> RefundQuery(AlipayTradeRefundQueryViewModel viewMode)
        {
            var model = new AlipayTradeFastpayRefundQueryModel
            {
                OutTradeNo = viewMode.OutTradeNo,
                TradeNo = viewMode.TradeNo,
                OutRequestNo = viewMode.OutRequestNo
            };

            var req = new AlipayTradeFastpayRefundQueryRequest();
            req.SetBizModel(model);

            var response = await _client.CertificateExecuteAsync(req, _optionsAccessor.Value);
            return Ok(response.Body);
        }

        #endregion 退款查询

        //#region 交易关闭

        ///// <summary>交易关闭</summary>
        //[HttpPost]
        //public async Task<IActionResult> Close(AlipayTradeCloseViewModel viewMode)
        //{
        //    var model = new AlipayTradeCloseModel
        //    {
        //        OutTradeNo = viewMode.OutTradeNo,
        //        TradeNo = viewMode.TradeNo,
        //    };

        // var req = new AlipayTradeCloseRequest(); req.SetBizModel(model); req.SetNotifyUrl(viewMode.NotifyUrl);

        //    var response = await _client.CertificateExecuteAsync(req, _optionsAccessor.Value);
        //    return Ok(response.Body);
        //}

        //#endregion 交易关闭
        //#region 统一转账

        ///// <summary>统一转账</summary>
        //[HttpPost]
        //public async Task<IActionResult> Transfer(AlipayTransferViewModel viewMode)
        //{
        //    var model = new AlipayFundTransUniTransferModel
        //    {
        //        OutBizNo = viewMode.OutBizNo,
        //        TransAmount = viewMode.TransAmount,
        //        ProductCode = viewMode.ProductCode,
        //        BizScene = viewMode.BizScene,
        //        PayeeInfo = new Participant { Identity = viewMode.PayeeIdentity, IdentityType = viewMode.PayeeIdentityType, Name = viewMode.PayeeName },
        //        Remark = viewMode.Remark
        //    };
        //    var req = new AlipayFundTransUniTransferRequest();
        //    req.SetBizModel(model);
        //    var response = await _client.CertificateExecuteAsync(req, _optionsAccessor.Value);
        //    return Ok(response.Body);
        //}
        //#endregion

        #region 查询统一转账订单

        /// <summary>查询统一转账订单</summary>
        //[HttpPost]
        //public async Task<IActionResult> TransQuery(AlipayTransQueryViewModel viewMode)
        //{
        //    var model = new AlipayFundTransCommonQueryModel
        //    {
        //        OutBizNo = viewMode.OutBizNo,
        //        OrderId = viewMode.OrderId
        //    };

        //    var req = new AlipayFundTransCommonQueryRequest();
        //    req.SetBizModel(model);
        //    var response = await _client.CertificateExecuteAsync(req, _optionsAccessor.Value);
        //    return Ok(response.Body);
        //}

        #endregion 查询统一转账订单

        #region 余额查询

        /// <summary>余额查询</summary>
        [HttpPost]
        public async Task<IActionResult> AccountQuery(AlipayAccountQueryViewModel viewModel)
        {
            var model = new AlipayFundAccountQueryModel
            {
                AlipayUserId = viewModel.AlipayUserId,
                AccountType = viewModel.AccountType
            };

            var req = new AlipayFundAccountQueryRequest();
            req.SetBizModel(model);
            var response = await _client.CertificateExecuteAsync(req, _optionsAccessor.Value);
            // ViewData["response"] = response.Body;
            return Ok(response.Body);
        }

        #endregion 余额查询
    }
}