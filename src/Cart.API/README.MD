# Cart.API

Service quản lý giỏ hàng trong hệ thống eShop.

## 📋 Mô tả

Cart.API chịu trách nhiệm quản lý giỏ hàng của người dùng, bao gồm thêm, xóa, cập nhật sản phẩm trong giỏ hàng và lưu trữ tạm thời thông tin giỏ hàng.

## 🛠️ Công nghệ

- **.NET Core**: Framework chính
- **Redis**: Lưu trữ cache giỏ hàng
- **JWT**: Xác thực người dùng
- **Docker**: Containerization

## 🔧 Cấu hình

### Biến môi trường

```env
REDIS_HOST=redis
REDIS_PORT=6379
REDIS_PASSWORD=
```

### Cấu hình Redis

Service sử dụng Redis để lưu trữ tạm thời thông tin giỏ hàng với key format: `cart:{userId}`

## 🚀 API Endpoints

### Lấy giỏ hàng
```
GET /api/cart
Authorization: Bearer {token}
```

### Thêm sản phẩm vào giỏ hàng
```
POST /api/cart/items
Authorization: Bearer {token}
Content-Type: application/json

{
  "productId": "string",
  "quantity": number,
  "price": number
}
```

### Cập nhật số lượng sản phẩm
```
PUT /api/cart/items/{productId}
Authorization: Bearer {token}
Content-Type: application/json

{
  "quantity": number
}
```

### Xóa sản phẩm khỏi giỏ hàng
```
DELETE /api/cart/items/{productId}
Authorization: Bearer {token}
```

### Xóa toàn bộ giỏ hàng
```
DELETE /api/cart
Authorization: Bearer {token}
```

## 🏃‍♂️ Chạy Local

```bash
cd src/Cart.API
dotnet run
```

Service sẽ chạy trên:
- HTTP: `http://localhost:5001`
- HTTPS: `https://localhost:5501`

## 🐳 Docker

```bash
docker build -t cartapi -f src/Cart.API/Dockerfile .
docker run -p 5001:80 cartapi
```

## 🔗 Dependencies

- Redis server phải được khởi động trước
- Identity.API để xác thực JWT token

## 📊 Monitoring

- Health check endpoint: `/health`
- Metrics endpoint: `/metrics`