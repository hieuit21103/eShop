namespace Shared.Event.Enums;
public enum PaymentStatusCode
{
    [Description("Giao dịch thành công")]
    Success = 0, // Giao dịch thành công
    [Description("Trừ tiền thành công. Giao dịch bị nghi ngờ")]
    Suspicious = 7, // Trừ tiền thành công. Giao dịch bị nghi ngờ
    [Description("Thẻ/Tài khoản chưa đăng ký InternetBanking")]
    NotRegisteredInternetBanking = 9, // Thẻ/Tài khoản chưa đăng ký InternetBanking
    [Description("Xác thực thông tin sai quá 3 lần")]
    InvalidAuthInfo = 10, // Xác thực thông tin sai quá 3 lần
    [Description("Hết hạn chờ thanh toán")]
    PaymentTimeout = 11, // Hết hạn chờ thanh toán
    [Description("Tài khoản bị khóa")]
    AccountLocked = 12, // Tài khoản bị khóa
    [Description("Nhập sai OTP")]
    InvalidOTP = 13, // Nhập sai OTP
    [Description("Khách hàng hủy giao dịch")]
    UserCancelled = 24, // Khách hàng hủy giao dịch
    [Description("Không đủ số dư")]
    InsufficientFunds = 51, // Không đủ số dư
    [Description("Vượt quá hạn mức trong ngày")]
    ExceededDailyLimit = 65, // Vượt quá hạn mức trong ngày
    [Description("Ngân hàng đang bảo trì")]
    BankMaintenance = 75, // Ngân hàng đang bảo trì
    [Description("Nhập sai mật khẩu quá số lần quy định")]
    ExceededPasswordAttempts = 79, // Nhập sai mật khẩu quá số lần quy định
    [Description("Lỗi không xác định")]
    UnknownError = 99 // Các lỗi khác
}
