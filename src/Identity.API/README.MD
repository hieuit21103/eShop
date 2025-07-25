# Identity.API

Service xác thực và quản lý người dùng trong hệ thống eShop.

## 📋 Mô tả

Identity.API chịu trách nhiệm xác thực người dùng, quản lý tài khoản, phân quyền và cung cấp JWT tokens cho các services khác trong hệ thống.

## 🛠️ Công nghệ

- **.NET Core**: Framework chính
- **MySQL**: Cơ sở dữ liệu
- **Entity Framework Core**: ORM
- **JWT Bearer**: Xác thực token
- **ASP.NET Core Identity**: Quản lý người dùng
- **SMTP**: Gửi email xác nhận

## 🔧 Cấu hình

### Biến môi trường

```env
# Database
DB_HOST=mysql
DB_PORT=3306
DB_DATABASE=identitydb
DB_USERNAME=root
DB_PASSWORD=your-db-password

# JWT
JWT_KEY=your-jwt-secret-key
JWT_ISSUER=your-jwt-issuer
JWT_AUDIENCE=your-jwt-audience

# SMTP (Email)
SMTP_SERVER=smtp.example.com
SMTP_PORT=587
SMTP_USERNAME=your-email@example.com
SMTP_PASSWORD=your-smtp-password
```

## 🗄️ Database Schema

### Users Table
- Id (Primary Key)
- Username
- Email
- PasswordHash
- EmailConfirmed
- PhoneNumber
- TwoFactorEnabled
- CreatedAt
- UpdatedAt

### Roles Table
- Id (Primary Key)
- Name
- Description

### UserRoles Table
- UserId (Foreign Key)
- RoleId (Foreign Key)

## 🔐 Xác thực & Ủy quyền

### JWT Configuration
- **Issuer**: Định danh hệ thống phát hành token
- **Audience**: Định danh đối tượng sử dụng token
- **Secret Key**: Khóa bí mật để sign/verify token
- **Expiration**: Thời hạn token (mặc định 24h)

### Policies
- **AdminOnly**: Yêu cầu role "Admin"
- **UserOnly**: Yêu cầu role "User"
- **CustomerService**: Yêu cầu role "Admin" hoặc "CustomerService"

## 🚀 API Endpoints

### Đăng ký

```
POST /api/auth/register
Content-Type: application/json

{
  "username": "user123",
  "password": "Password@123",
  "email": "user@example.com",
  "phoneNumber": "0123456789"
}
```

**Response:**
```json
{
  "message": "Registration successful. Please check your email to confirm your account.",
  "userId": "guid"
}
```

### Xác nhận email

```
GET /api/auth/confirm-email/{userId}/{token}
```

**Response:**
```json
{
  "message": "Email confirmed successfully"
}
```

### Đăng nhập

```
POST /api/auth/login
Content-Type: application/json

{
  "username": "user123",
  "password": "Password@123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2025-07-26T09:25:28Z",
  "user": {
    "id": "guid",
    "username": "user123",
    "email": "user@example.com",
    "roles": ["User"]
  }
}
```

### Đăng xuất

```
POST /api/auth/logout
Authorization: Bearer {token}
```

**Response:**
```json
{
  "message": "Logged out successfully"
}
```

### Lấy thông tin người dùng hiện tại

```
GET /api/auth/me
Authorization: Bearer {token}
```

**Response:**
```json
{
  "id": "guid",
  "username": "user123",
  "email": "user@example.com",
  "phoneNumber": "0123456789",
  "emailConfirmed": true,
  "roles": ["User"],
  "createdAt": "2025-07-25T09:25:28Z"
}
```

### Gửi email đặt lại mật khẩu

```
GET /api/auth/forgot-password/{email}
Authorization: Bearer {token}
```

**Response:**
```json
{
  "message": "Reset password email sent successfully"
}
```

### Lấy thông tin reset password

```
GET /api/auth/reset-password/{userId}/{token}
Authorization: Bearer {token}
```

**Response:**
```json
{
  "message": "Reset token is valid",
  "userId": "guid"
}
```

### Đặt lại mật khẩu

```
PUT /api/auth/reset-password/{userId}/{token}
Authorization: Bearer {token}
Content-Type: application/json

{
  "password": "NewPassword@123",
  "confirmPassword": "NewPassword@123"
}
```

**Response:**
```json
{
  "message": "Password reset successfully"
}
```

### Admin Endpoints

#### Lấy danh sách người dùng (Admin only)
```
GET /api/admin/users?page=1&pageSize=10&search=keyword
Authorization: Bearer {token}
```

#### Cập nhật role người dùng (Admin only)
```
PUT /api/admin/users/{userId}/roles
Authorization: Bearer {token}
Content-Type: application/json

{
  "roles": ["User", "CustomerService"]
}
```

#### Khóa/Mở khóa tài khoản (Admin only)
```
PUT /api/admin/users/{userId}/lock
Authorization: Bearer {token}
Content-Type: application/json

{
  "isLocked": true,
  "reason": "Violation of terms"
}
```

## 🔄 Authentication Flow

1. **Registration**: Tạo tài khoản → Gửi email xác nhận
2. **Email Confirmation**: Xác nhận email → Kích hoạt tài khoản
3. **Login**: Đăng nhập → Nhận JWT token
4. **Authorization**: Sử dụng token để truy cập protected endpoints
5. **Token Refresh**: Token hết hạn → Đăng nhập lại

## 🏃‍♂️ Chạy Local

```bash
cd src/Identity.API
dotnet run
```

Service sẽ chạy trên:
- HTTP: `http://localhost:5003`
- HTTPS: `https://localhost:5503`

## 🐳 Docker

```bash
docker build -t identityapi -f src/Identity.API/Dockerfile .
docker run -p 5003:80 identityapi
```

## 📧 Email Templates

Service sử dụng các email templates:
- **Email Confirmation**: Xác nhận đăng ký tài khoản
- **Password Reset**: Đặt lại mật khẩu
- **Account Locked**: Thông báo tài khoản bị khóa

## 🔒 Security Features

- **Password Policy**: Yêu cầu password mạnh
- **Account Lockout**: Khóa tài khoản sau nhiều lần đăng nhập sai
- **Two-Factor Authentication**: Hỗ trợ 2FA (tùy chọn)
- **JWT Security**: Token có thời hạn và signed
- **Rate Limiting**: Giới hạn request để tránh brute force

## 📊 Monitoring

- Authentication success/failure rates
- Token generation metrics
- Email sending status
- Account creation statistics

## 🔗 Dependencies

- MySQL database
- SMTP server để gửi email
- Shared.Event library (cho events)

## 🚀 Data Seeding

Khi chạy lần đầu, hệ thống sẽ tự động seed:

### Default Roles
- **Admin**: Quản trị viên hệ thống
- **User**: Người dùng thông thường
- **CustomerService**: Nhân viên chăm sóc khách hàng

### Default Admin User
- **Username**: admin
- **Email**: admin@eshop.com
- **Password**: Admin@123
- **Role**: Admin

## 📝 Validation Rules

### Password Requirements
- Tối thiểu 8 ký tự
- Ít nhất 1 chữ hoa
- Ít nhất 1 chữ thường
- Ít nhất 1 số
- Ít nhất 1 ký tự đặc biệt

### Username Requirements
- Từ 3-50 ký tự
- Chỉ chứa chữ cái, số và dấu gạch dưới
- Không được trùng lặp

### Email Requirements
- Định dạng email hợp lệ
- Không được trùng lặp
- Phải được xác nhận trước khi sử dụng đầy đủ tính năng