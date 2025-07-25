# Catalog.API

Service quản lý sản phẩm và danh mục trong hệ thống eShop.

## 📋 Mô tả

Catalog.API chịu trách nhiệm quản lý thông tin sản phẩm, danh mục, tìm kiếm và phân trang sản phẩm. Service này cũng publish events khi có thay đổi về sản phẩm.

## 🛠️ Công nghệ

- **.NET Core**: Framework chính
- **MySQL**: Cơ sở dữ liệu
- **Entity Framework Core**: ORM
- **RabbitMQ**: Message broker
- **JWT**: Xác thực và ủy quyền

## 🔧 Cấu hình

### Biến môi trường

```env
# Database
DB_HOST=mysql
DB_PORT=3306
DB_DATABASE=catalogdb
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
```

## 🗄️ Database Schema

### Products Table
- Id (Primary Key)
- Name
- Description
- Price
- StockQuantity
- CategoryId (Foreign Key)
- ImageUrl
- CreatedAt
- UpdatedAt

### Categories Table
- Id (Primary Key)
- Name
- Description
- CreatedAt
- UpdatedAt

## 🚀 API Endpoints

### Sản phẩm

#### Lấy danh sách sản phẩm
```
GET /api/products?page=1&pageSize=10&categoryId=1&search=keyword
```

#### Lấy chi tiết sản phẩm
```
GET /api/products/{id}
```

#### Tạo sản phẩm mới (Admin only)
```
POST /api/products
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "string",
  "description": "string",
  "price": number,
  "stockQuantity": number,
  "categoryId": number,
  "imageUrl": "string"
}
```

#### Cập nhật sản phẩm (Admin only)
```
PUT /api/products/{id}
Authorization: Bearer {token}
Content-Type: application/json
```

#### Xóa sản phẩm (Admin only)
```
DELETE /api/products/{id}
Authorization: Bearer {token}
```

### Danh mục

#### Lấy danh sách danh mục
```
GET /api/categories
```

#### Tạo danh mục mới (Admin only)
```
POST /api/categories
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "string",
  "description": "string"
}
```

## 🏃‍♂️ Chạy Local

```bash
cd src/Catalog.API
dotnet run
```

Service sẽ chạy trên:
- HTTP: `http://localhost:5002`
- HTTPS: `https://localhost:5502`

## 🐳 Docker

```bash
docker build -t catalogapi -f src/Catalog.API/Dockerfile .
docker run -p 5002:80 catalogapi
```

## 📡 Events

Service publish các events sau:

- **ProductCreated**: Khi tạo sản phẩm mới
- **ProductUpdated**: Khi cập nhật sản phẩm
- **ProductDeleted**: Khi xóa sản phẩm
- **StockUpdated**: Khi cập nhật kho

## 🔗 Dependencies

- MySQL database
- RabbitMQ message broker
- Identity.API để xác thực JWT token