# eShop - Hệ thống Thương mại Điện tử

Hệ thống thương mại điện tử được xây dựng theo kiến trúc microservices với .NET Core và React.

## 🏗️ Kiến trúc Hệ thống

Hệ thống bao gồm các microservices sau:

- **Identity.API**: Xác thực và quản lý người dùng
- **Catalog.API**: Quản lý sản phẩm và danh mục
- **Cart.API**: Quản lý giỏ hàng
- **Order.API**: Xử lý đơn hàng
- **Payment.API**: Xử lý thanh toán
- **PaymentServiceProvider**: Nhà cung cấp dịch vụ thanh toán
- **EventBusRabbitMQ**: Message bus sử dụng RabbitMQ
- **Shared.Event**: Thư viện chia sẻ events giữa các services
- **UserApp**: Ứng dụng người dùng (React)
- **AdminApp**: Ứng dụng quản trị

## 🛠️ Công nghệ Sử dụng

### Backend
- **.NET Core**: Framework chính cho các API services
- **MySQL**: Cơ sở dữ liệu chính
- **Redis**: Cache và session storage
- **RabbitMQ**: Message broker
- **JWT**: Xác thực và ủy quyền
- **Docker**: Containerization

### Frontend
- **React**: Framework cho UserApp
- **React Router**: Routing
- **TailwindCSS**: Styling

## 🚀 Khởi chạy Hệ thống

### Yêu cầu
- Docker & Docker Compose
- .NET Core SDK (cho development)
- Node.js (cho frontend development)

### Chạy với Docker Compose

1. Clone repository:
```bash
git clone https://github.com/hieuit21103/eShop.git
cd eShop
```

2. Tạo file `.env` với các biến môi trường cần thiết:
```env
# Frontend URL
FRONTEND_URL=http://your-frontend-url.com

# MySQL Database Configuration
MYSQL_DEFAULT_PASSWORD=your-db-password

# Payment and Catalog API URLs
PAYMENT_API_URL=http://paymentapi:80
CATALOG_API_URL=http://catalogapi:80

# MySQL Database
DB_HOST=mysql
DB_PORT=3306
DB_USERNAME=root
DB_PASSWORD=your-db-password

# JWT Settings
JWT_KEY=your-jwt-secret-key
JWT_ISSUER=your-jwt-issuer
JWT_AUDIENCE=your-jwt-audience

# SMTP (Email)
SMTP_SERVER=smtp.example.com
SMTP_PORT=587
SMTP_USERNAME=your-email@example.com
SMTP_PASSWORD=your-smtp-password

# RabbitMQ
RABBITMQ_HOST=rabbitmq
RABBITMQ_PORT=5672
RABBITMQ_USERNAME=your-rabbitmq-username
RABBITMQ_PASSWORD=your-rabbitmq-password

# Redis
REDIS_HOST=redis
REDIS_PORT=6379
REDIS_PASSWORD=your-redis-password

# VNPAY Configuration
VNPAY_TMN_CODE=your-vnpay-tmn-code
VNPAY_HASH_SECRET=your-vnpay-hash-secret

# Configuration
# MySQL Database
MYSQL_DEFAULT_PASSWORD=your-mysql-default-password

# RabbitMQ
RABBITMQ_DEFAULT_USER=guest
RABBITMQ_DEFAULT_PASS=guest
```

3. Khởi chạy hệ thống:
```bash
docker-compose up -d
```

### Cổng Dịch vụ

| Service | HTTP Port | HTTPS Port |
|---------|-----------|------------|
| Cart.API | 5001 | 5501 |
| Catalog.API | 5002 | 5502 |
| Identity.API | 5003 | 5503 |
| Order.API | 5004 | 5504 |
| MySQL | 3306 | - |
| Redis | 6379 | - |
| RabbitMQ | 5672 | - |
| RabbitMQ UI | 15672 | - |

## 📁 Cấu trúc Thư mục

```
src/
├── AdminApp/                 # Ứng dụng quản trị
├── Cart.API/                # Service quản lý giỏ hàng
├── Catalog.API/             # Service quản lý sản phẩm
├── Identity.API/            # Service xác thực
├── Order.API/               # Service đơn hàng
├── Payment.API/             # Service thanh toán
├── PaymentServiceProvider/  # Nhà cung cấp thanh toán
├── EventBusRabbitMQ/       # Message bus
├── Shared.Event/           # Events chia sẻ
└── UserApp/                # Ứng dụng người dùng
```

## 🔧 Development

### Chạy từng service riêng lẻ
```bash
cd src/[ServiceName]
dotnet run
```

### Chạy frontend
```bash
cd src/UserApp
npm install
npm run dev
```

## 🤝 Đóng góp

1. Fork repository
2. Tạo feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Tạo Pull Request

## 📄 License

Distributed under the MIT License. See `LICENSE` for more information.

## 📞 Liên hệ

- GitHub: [@hieuit21103](https://github.com/hieuit21103)
- Project Link: [https://github.com/hieuit21103/eShop](https://github.com/hieuit21103/eShop)