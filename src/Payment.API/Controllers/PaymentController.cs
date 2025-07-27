using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;

namespace Payment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController(IPaymentService<PaymentService> paymentService, IPaymentServiceProvider paymentServiceProvider, IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IPaymentService<PaymentService> _paymentService = paymentService;

    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetAllPayments(int page = 1, int pageSize = 10)
    {
        var payments = await _paymentService.GetAllAsync(page, pageSize);
        return Ok(payments);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetPaymentInfo(Guid orderId)
    {
        var userTokenId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userTokenId))
        {
            return Unauthorized("User token ID is missing.");
        }
        var paymentInfo = await _paymentService.GetByOrderIdAsync(orderId);
        if (paymentInfo.UserId != Guid.Parse(userTokenId) && !User.IsInRole("Admin"))
        {
            return Forbid("You do not have permission to access this payment information.");
        }
        if (paymentInfo == null)
        {
            return NotFound();
        }
        return Ok(paymentInfo);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] PaymentInfo paymentInfo)
    {
        if (paymentInfo == null || paymentInfo.Amount <= 0)
        {
            return BadRequest("Invalid payment request.");
        }
        switch (paymentInfo.PaymentMethod)
        {
            case PaymentMethod.VNPay:
                paymentServiceProvider = new VnPayServiceProvider();
                break;
            case PaymentMethod.MoMo:
                break;
            case PaymentMethod.ZaloPay:
                break;
            case PaymentMethod.COD:
                paymentInfo.PaymentUrl = "Cash on Delivery";
                paymentInfo.Status = PaymentStatus.Pending;
                break;
        }
        var clientIp = GetClientIpAddress();
        var returnUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") + "payment/return";
        if(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("FRONTEND_URL")))
        {
            returnUrl = "http://localhost:5005/api/payment/vnpay/return";
        }
        paymentInfo.PaymentUrl = paymentServiceProvider.GeneratePaymentUrl(paymentInfo.OrderId, paymentInfo.Amount, returnUrl, clientIp);
        var result = await _paymentService.CreateAsync(paymentInfo);
        if (!result)
        {
            return StatusCode(500, "Failed to create payment information.");
        }
        return CreatedAtAction(nameof(GetPaymentInfo), new { orderId = paymentInfo.OrderId }, paymentInfo);
    }

    [HttpGet("vnpay/return")]
    public async Task<IActionResult> ReturnFromPayment([FromQuery] VNPayReturn query)
    {
        try
        {
            var vnpayData = new Dictionary<string, string>();
            var properties = typeof(VNPayReturn).GetProperties();
            foreach (var prop in properties)
            {
                var value = prop.GetValue(query)?.ToString();
                if (!string.IsNullOrEmpty(value) && prop.Name != "vnp_SecureHash")
                {
                    vnpayData.Add(prop.Name, value);
                }
            }
            string vnpHashSecret = Environment.GetEnvironmentVariable("VNPAY_HASH_SECRET") ?? "";
            if (string.IsNullOrEmpty(vnpHashSecret))
            {
                return BadRequest("VNPAY_HASH_SECRET is not set.");
            }
            string vnpSecureHash = query.vnp_SecureHash;
            paymentServiceProvider = new VnPayServiceProvider();
            bool isValidSignature = paymentServiceProvider.ValidateSignature(vnpayData, vnpSecureHash, vnpHashSecret);
            if (isValidSignature)
            {
                if(query.vnp_ResponseCode == "00")
                {
                    var paymentInfo = _paymentService.GetByOrderIdAsync(Guid.Parse(query.vnp_TxnRef)).Result;
                    if (paymentInfo == null)
                    {
                        return NotFound("Payment information not found.");
                    }
                    paymentInfo.TransactionId = query.vnp_TransactionNo;
                    paymentInfo.Status = PaymentStatus.Completed;
                    await _paymentService.UpdateAsync(paymentInfo);
                    return Ok(new PaymentResponse
                    {
                        OrderId = paymentInfo.OrderId,
                        Amount = paymentInfo.Amount,
                        TransactionId = query.vnp_TransactionNo,
                        ResponseCode = query.vnp_ResponseCode,
                        Message = EnumsHelper.GetDescription(PaymentStatusCode.Success)
                    });
                }
                else
                {
                    return BadRequest(new PaymentResponse
                    {
                        OrderId = Guid.Parse(query.vnp_TxnRef),
                        Amount = 0,
                        TransactionId = null,
                        ResponseCode = query.vnp_ResponseCode,
                        Message = EnumsHelper.GetDescription((PaymentStatusCode)Enum.Parse(typeof(PaymentStatusCode), query.vnp_ResponseCode))
                    });
                }
            }
            else
            {
                return BadRequest("Invalid signature.");
            }
        }
        catch
        {
            return StatusCode(500, "Internal server error while processing payment return.");
        }
    }

    private string GetClientIpAddress()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return "127.0.0.1";

        // Thử các header khác nhau để lấy IP thực
        var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(',')[0].Trim();
        }

        var realIp = httpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIp))
        {
            return realIp;
        }

        var remoteIp = httpContext.Connection.RemoteIpAddress?.ToString();
        if (!string.IsNullOrEmpty(remoteIp) && remoteIp != "::1")
        {
            return remoteIp;
        }

        return "127.0.0.1"; // Fallback
    }
}