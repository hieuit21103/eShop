namespace PaymentServiceProvider;

public class VnPayServiceProvider : IPaymentServiceProvider
{
    public string GeneratePaymentUrl(Guid orderId, decimal amount, string returnUrl, string clientIp = "127.0.0.1")
    {
        var vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        var vnp_TmnCode = Environment.GetEnvironmentVariable("VNPAY_TMN_CODE");
        var vnp_HashSecret = Environment.GetEnvironmentVariable("VNPAY_HASH_SECRET");

        var vnp_Version = "2.1.0";
        var vnp_Command = "pay";
        var vnp_CurrCode = "VND";
        var vnp_Locale = "vn";

        var vnp_TxnRef = orderId.ToString();
        var vnp_OrderInfo = $"Thanh toan don hang {orderId}"; // Bỏ dấu ":"
        var vnp_OrderType = "other";
        var vnp_Amount = ((long)(amount * 100)).ToString(); // Dùng long thay vì int
        var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
        var vnp_IpAddr = clientIp;

        if (string.IsNullOrWhiteSpace(vnp_TmnCode) || string.IsNullOrWhiteSpace(vnp_HashSecret))
            throw new Exception("Missing VNPAY_TMN_CODE or VNPAY_HASH_SECRET");

        var inputData = new SortedDictionary<string, string>
        {
            { "vnp_Version", vnp_Version },
            { "vnp_Command", vnp_Command },
            { "vnp_TmnCode", vnp_TmnCode },
            { "vnp_Amount", vnp_Amount },
            { "vnp_CurrCode", vnp_CurrCode },
            { "vnp_TxnRef", vnp_TxnRef },
            { "vnp_OrderInfo", vnp_OrderInfo },
            { "vnp_OrderType", vnp_OrderType },
            { "vnp_Locale", vnp_Locale },
            { "vnp_ReturnUrl", returnUrl },
            { "vnp_IpAddr", vnp_IpAddr },
            { "vnp_CreateDate", vnp_CreateDate }
        };

        // Tạo query string và hash data
        var queryBuilder = new StringBuilder();
        var hashDataBuilder = new StringBuilder();

        foreach (var kv in inputData.Where(x => !string.IsNullOrEmpty(x.Value)))
        {
            if (hashDataBuilder.Length > 0)
            {
                hashDataBuilder.Append('&');
                queryBuilder.Append('&');
            }

            hashDataBuilder.Append($"{kv.Key}={WebUtility.UrlEncode(kv.Value)}");
            queryBuilder.Append($"{kv.Key}={WebUtility.UrlEncode(kv.Value)}");
        }

        var rawHash = hashDataBuilder.ToString();
        var queryString = queryBuilder.ToString();
        var secureHash = CreateHmacSHA512(rawHash, vnp_HashSecret);
        
        var paymentUrl = $"{vnp_Url}?{queryString}&vnp_SecureHash={secureHash}";
        
        return paymentUrl;
    }

    private string CreateHmacSHA512(string input, string key)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var inputBytes = Encoding.UTF8.GetBytes(input);
        using var hmac = new HMACSHA512(keyBytes);
        var hashBytes = hmac.ComputeHash(inputBytes);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    public bool ValidateSignature(Dictionary<string, string> vnpayData, string inputHash, string secretKey)
    {
        var vnPay = new SortedDictionary<string, string>();
        
        foreach (var kv in vnpayData)
        {
            if (!string.IsNullOrEmpty(kv.Value) && kv.Key.StartsWith("vnp_"))
            {
                vnPay.Add(kv.Key, kv.Value);
            }
        }

        // Xóa vnp_SecureHash khỏi data để tính toán
        vnPay.Remove("vnp_SecureHashType");
        vnPay.Remove("vnp_SecureHash");

        var hashData = new StringBuilder();
        foreach (var kv in vnPay)
        {
            if (hashData.Length > 0)
            {
                hashData.Append('&');
            }
            hashData.Append($"{kv.Key}={WebUtility.UrlEncode(kv.Value)}");
        }

        var checkSum = CreateHmacSHA512(hashData.ToString(), secretKey);
        return checkSum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
    }
}