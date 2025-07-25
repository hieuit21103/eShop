# Payment.API

Service xử lý thanh toán trong hệ thống eShop.

## 📋 Mô tả

Payment.API chịu trách nhiệm xử lý các giao dịch thanh toán, tích hợp với các nhà cung cấp dịch vụ thanh toán và quản lý trạng thái giao dịch.

## 🛠️ Công nghệ

- **.NET Core**: Framework chính
- **MySQL**: Cơ sở dữ liệu
- **Entity Framework Core**: ORM
- **RabbitMQ**: Message broker
- **JWT**: Xác thực người dùng

## 🔧 Cấu hình

### Biến môi trường

```env
# Database
DB_HOST=mysql
DB_PORT=3306
DB_DATABASE=paymentdb
DB_USERNAME=root
DB_PASSWORD=1234

# JWT
JWT_KEY=your_jwt_secret_key
JWT_ISSUER=eShop
JWT_AUDIENCE=eShop

# RabbitMQ
RABBITMQ_HOST=rabbitmq
RABBITMQ_PORT=5672
RABBITMQ_USERNAME=guest
RABBITMQ_PASSWORD=guest

# Payment Providers
STRIPE_SECRET_KEY=your_stripe_secret_key
PAYPAL_CLIENT_ID=your_paypal_client_id
PAYPAL_CLIENT_SECRET=your_paypal_secret
```

## 🗄️ Database Schema

### Payments Table
- Id (Primary Key)
- OrderId
- UserId
- Amount
- Currency
- Method (CreditCard, PayPal, BankTransfer)
- Status (Pending, Processing, Completed, Failed, Refunded)
- TransactionId
- ProviderResponse
- CreatedAt
- UpdatedAt

### PaymentMethods Table
- Id (Primary Key)
- UserId
- Type (CreditCard, PayPal)
- Details (Encrypted JSON)
- IsDefault
- CreatedAt

## 🚀 API Endpoints

### Thanh toán

#### Tạo thanh toán
```
POST /api/payments
Authorization: Bearer {token}
Content-Type: application/json

{
  "orderId": "string",
  "amount": number,
  "currency": "USD",
  "method": "CreditCard|PayPal|BankTransfer",
  "paymentDetails": {
    "cardNumber": "string",
    "expiryMonth": number,
    "expiryYear": number,
    "cvv": "string"
  }
}
```

#### Lấy trạng thái thanh toán
```
GET /api/payments/{id}
Authorization: Bearer {token}
```

#### Lấy lịch sử thanh toán
```
GET /api/payments/user
Authorization: Bearer {token}
```

#### Hoàn tiền
```
POST /api/payments/{id}/refund
Authorization: Bearer {token}
Content-Type: application/json

{
  "amount": number,
  "reason": "string"
}
```

### Phương thức thanh toán

#### Lấy phương thức thanh toán của user
```
GET /api/payment-methods
Authorization: Bearer {token}
```

#### Thêm phương thức thanh toán
```
POST /api/payment-methods
Authorization: Bearer {token}
Content-Type: application/json

{
  "type": "CreditCard|PayPal",
  "details": {
    "cardNumber": "string",
    "holderName": "string",
    "expiryMonth": number,
    "expiryYear": number
  },
  "isDefault": boolean
}
```

#### Xóa phương thức thanh toán
```
DELETE /api/payment-methods/{id}
Authorization: Bearer {token}
```

## 🔄 Payment Flow

1. **Initiate**: Tạo payment record với trạng thái Pending
2. **Process**: Gọi payment provider để xử lý
3. **Callback**: Nhận response từ provider
4. **Update**: Cập nhật trạng thái và publish event
5. **Notify**: Thông báo cho Order.API về kết quả

## 🏃‍♂️ Chạy Local

```bash
cd src/Payment.API
dotnet run
```

Service sẽ chạy trên:
- HTTP: `http://localhost:5005`
- HTTPS: `https://localhost:5505`

## 📡 Events

Service lắng nghe events:
- **OrderCreated**: Chuẩn bị xử lý thanh toán

Service publish events:
- **PaymentCompleted**: Thanh toán thành công
- **PaymentFailed**: Thanh toán thất bại
- **PaymentRefunded**: Hoàn tiền

## 🔒 Security

- Thông tin thẻ được mã hóa trước khi lưu
- PCI DSS compliance cho xử lý thẻ tín dụng
- Rate limiting cho API endpoints
- Audit logs cho tất cả giao dịch

## 🔗 Dependencies

- MySQL database
- RabbitMQ message broker
- PaymentServiceProvider
- Identity.API để xác thực JWT token