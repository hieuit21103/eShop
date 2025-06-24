# Identity.API

API xác thực và quản lý người dùng cho hệ thống eShop.

## Môi trường/Khởi tạo

- Sử dụng MySQL, các biến môi trường cho cấu hình DB và JWT:
  - `DB_HOST`, `DB_PORT`, `DB_NAME`, `DB_USER`, `DB_PASSWORD`
  - `JWT_ISSUER`, `JWT_AUDIENCE`, `JWT_KEY`
- Khi chạy lần đầu sẽ tự động seed role, user mẫu.

## Xác thực & Ủy quyền

- Sử dụng JWT Bearer.
- Policy:
  - `AdminOnly`: yêu cầu role "Admin"
  - `UserOnly`: yêu cầu role "User"

## Các API chính

### Đăng ký

```
POST /api/auth/register
```
Body (JSON):
```json
{
  "username": "user",
  "password": "password",
  "email": "user@email.com"
}
```

### Xác nhận email

```
GET /api/auth/confirm-email/{userId}/{token}
```

### Đăng nhập

```
POST /api/auth/login
```
Body (JSON):
```json
{
  "username": "user",
  "password": "password"
}
```
Response: `{ "Token": "..." }`

### Đăng xuất

```
POST /api/auth/logout
```
> Yêu cầu user phải đăng nhập.

### Lấy thông tin người dùng hiện tại

```
GET /api/auth/me
```
> Yêu cầu user phải đăng nhập.

### Gửi email đặt lại mật khẩu

```
GET /api/auth/forgot-password/{email}
```
> Yêu cầu Bearer Token và userId lấy từ email trùng với user token.

### Lấy thông tin reset password

```
GET /api/auth/reset-password/{userId}/{token}
```
> Yêu cầu Bearer Token và userId trùng với user token.

### Đặt lại mật khẩu

```
PUT /api/auth/reset-password/{userId}/{token}
```
Body (JSON):
```json
{
  "password": "newpassword",
  "confirmPassword": "newpassword"
}
```
> Yêu cầu Bearer Token và userId trùng với user token.

## Chạy thử local

```bash
cd src/Identity.API
dotnet run
```

API docs (Swagger UI) có thể được bật ở chế độ Development tại `/swagger` hoặc theo cấu hình `app.MapOpenApi()`.

---

## Liên hệ & Đóng góp

Vui lòng tạo issue hoặc pull request nếu muốn đóng góp hoặc gặp lỗi.