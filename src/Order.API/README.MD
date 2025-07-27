# Order.API

Service xử lý đơn hàng trong hệ thống eShop.

## 📋 Mô tả

Order.API chịu trách nhiệm tạo, quản lý và theo dõi trạng thái đơn hàng. Service này tích hợp với Catalog.API để kiểm tra tồn kho và với Payment.API để xử lý thanh toán.

## 🛠️ Công nghệ

- **.NET Core**: Framework chính
- **MySQL**: Cơ sở dữ liệu
- **Entity Framework Core**: ORM
- **RabbitMQ**: Message broker
- **HTTP Client**: Gọi API external services
- **JWT**: Xác thực người dùng

## 🔧 Cấu hình

### Biến môi trường

```env
# Database
DB_HOST=mysql
DB_PORT=3306
DB_DATABASE=orderdb
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

# External APIs
CATALOG_API_URL=http://catalogapi
PAYMENT_API_URL=http://paymentapi
```

## 🗄️ Database Schema

### Orders Table
- Id (Primary Key)
- UserId
- Status (Pending, Confirmed, Shipped, Delivered, Cancelled)
- TotalAmount
- ShippingAddress
- CreatedAt
- UpdatedAt

### OrderItems Table
- Id (Primary Key)
- OrderId (Foreign Key)
- ProductId
- ProductName
- Quantity
- UnitPrice
- TotalPrice

## 🚀 API Endpoints

### Đơn hàng

#### Lấy danh sách đơn hàng của người dùng
```
GET /api/orders
Authorization: Bearer {token}
```

#### Lấy chi tiết đơn hàng
```
GET /api/orders/{id}
Authorization: Bearer {token}
```

#### Tạo đơn hàng mới
```
POST /api/orders
Authorization: Bearer {token}
Content-Type: application/json

{
  "items": [
    {
      "productId": "string",
      "quantity": number
    }
  ],
  "paymentMethod": number
}
```

#### Cập nhật trạng thái đơn hàng (Admin only)
```
PUT /api/orders/{id}/status
Authorization: Bearer {token}
Content-Type: application/json

{
  "status": "Confirmed|Shipped|Delivered|Cancelled"
}
```

#### Hủy đơn hàng
```
DELETE /api/orders/{id}
Authorization: Bearer {token}
```

### Admin endpoints

#### Lấy tất cả đơn hàng (Admin only)
```
GET /api/admin/orders?page=1&pageSize=10&status=Pending
Authorization: Bearer {token}
```

## 🔄 Order Flow

1. **Tạo đơn hàng**: Validate sản phẩm từ Catalog.API
2. **Xác nhận**: Kiểm tra tồn kho và reserve sản phẩm
3. **Thanh toán**: Gọi Payment.API để xử lý thanh toán
4. **Hoàn tất**: Cập nhật trạng thái và gửi notification

## 🏃‍♂️ Chạy Local

```bash
cd src/Order.API
dotnet run
```

Service sẽ chạy trên:
- HTTP: `http://localhost:5004`
- HTTPS: `https://localhost:5504`

## 📡 Events

Service lắng nghe events:
- **PaymentCompleted**: Cập nhật trạng thái đơn hàng
- **PaymentFailed**: Hủy đơn hàng

Service publish events:
- **OrderCreated**: Khi tạo đơn hàng mới
- **OrderStatusChanged**: Khi thay đổi trạng thái
- **OrderCancelled**: Khi hủy đơn hàng

## 🔗 Dependencies

- MySQL database
- RabbitMQ message broker
- Catalog.API để kiểm tra sản phẩm
- Payment.API để xử lý thanh toán
- Identity.API để xác thực JWT token