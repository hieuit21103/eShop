# Identity.API

Dịch vụ quản lý xác thực và thông tin người dùng cho hệ thống eShop.

---

## 1. Xác thực & Tài khoản

### Đăng ký

```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "username": "yourusername",
  "password": "yourpassword"
}
```

### Đăng nhập

```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "yourusername",
  "password": "yourpassword"
}
```
- **Response:** `{ "token": "<JWT Token>" }`

### Lấy thông tin tài khoản hiện tại

```http
GET /api/auth/me
Authorization: Bearer <JWT Token>
```

### Đăng xuất

```http
POST /api/auth/logout
Authorization: Bearer <JWT Token>
```

---

## 2. Quản lý Profile

- **Lấy profile cá nhân**  
  ```http
  GET /api/userprofile/user/{userId}
  Authorization: Bearer <JWT Token>
  ```

- **Cập nhật profile cá nhân**  
  ```http
  PUT /api/userprofile/user/{userId}
  Authorization: Bearer <JWT Token>
  Content-Type: application/json

  {
    // Thông tin profile cập nhật, ví dụ:
    // "fullName": "Nguyen Van A",
    // "avatarUrl": "https://yourcdn.com/avatar.jpg"
  }
  ```

- **Admin:**  
  Có thể sử dụng các API CRUD tiêu chuẩn:
  - `GET /api/userprofile`
  - `GET /api/userprofile/{id}`
  - `POST /api/userprofile`
  - `PUT /api/userprofile/{id}`
  - `DELETE /api/userprofile/{id}`
  - **Yêu cầu:** Token có quyền Admin (`Policy = "AdminOnly"`)

---

## 3. Quản lý Địa chỉ người dùng

### Cấu trúc thông tin địa chỉ (UserAddress)

```json
{
  "fullName": "Nguyen Van A",
  "phone": "0912345678",
  "addressLine": "123 Đường ABC",
  "ward": "Phường 1",
  "district": "Quận 1",
  "city": "TP.HCM",
  "isDefault": true
}
```

- **Lấy tất cả địa chỉ của user**
  ```http
  GET /api/useraddress/user/{userId}
  Authorization: Bearer <JWT Token>
  ```

- **Thêm địa chỉ mới**
  ```http
  POST /api/useraddress/user/{userId}
  Authorization: Bearer <JWT Token>
  Content-Type: application/json

  {
    "fullName": "Nguyen Van A",
    "phone": "0912345678",
    "addressLine": "123 Đường ABC",
    "ward": "Phường 1",
    "district": "Quận 1",
    "city": "TP.HCM",
    "isDefault": false
  }
  ```

- **Cập nhật địa chỉ**
  ```http
  PUT /api/useraddress/user/{userId}/{addressId}
  Authorization: Bearer <JWT Token>
  Content-Type: application/json

  {
    "fullName": "Nguyen Van B",
    "phone": "0999888777",
    "addressLine": "456 Đường XYZ",
    "ward": "Phường 2",
    "district": "Quận 3",
    "city": "TP.HCM",
    "isDefault": true
  }
  ```

- **Xóa địa chỉ**
  ```http
  DELETE /api/useraddress/user/{userId}/{addressId}
  Authorization: Bearer <JWT Token>
  ```

- **Admin:**  
  Có thể sử dụng các API CRUD tiêu chuẩn như mục profile.

---

## 4. Chuẩn bị & Chạy dự án

1. Cài đặt .NET 8 SDK
2. Khởi tạo DB, cấu hình connection string
3. Chạy lệnh:
    ```bash
    dotnet restore
    dotnet run --project src/Identity.API/Identity.API.csproj
    ```

Truy cập Swagger UI tại `/swagger` để thử nghiệm API.

---

## 5. Lưu ý phân quyền

- Các API quản trị (CRUD mặc định) yêu cầu quyền Admin (bearer token với policy "AdminOnly").
- Các API truy cập/cập nhật thông tin cá nhân chỉ cho phép chính chủ user (bearer token với policy "UserOnly").

---

## 6. Đóng góp & License

- Đóng góp qua Pull Request hoặc Issues
- License: MIT