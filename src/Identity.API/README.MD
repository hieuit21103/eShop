
```
Identity.API
├─ Application
│  ├─ DTOs
│  │  ├─ Auth
│  │  │  ├─ AuthResponse.cs
│  │  │  ├─ ChangePasswordRequest.cs
│  │  │  ├─ GoogleLoginRequest.cs
│  │  │  ├─ LoginRequest.cs
│  │  │  ├─ RegisterRequest.cs
│  │  │  └─ ResetPasswordRequest.cs
│  │  ├─ User
│  │  │  ├─ ApplicationUserRequest.cs
│  │  │  ├─ ApplicationUserResponse.cs
│  │  │  └─ ApplicationUserUpdateRequest.cs
│  │  ├─ UserAddress
│  │  │  ├─ UserAddressRequest.cs
│  │  │  ├─ UserAddressResponse.cs
│  │  │  └─ UserAddressUpdateRequest.cs
│  │  └─ UserProfile
│  │     ├─ UserProfileRequest.cs
│  │     ├─ UserProfileResponse.cs
│  │     └─ UserProfileUpdateRequest.cs
│  ├─ Interfaces
│  │  ├─ IApplicationUserService.cs
│  │  ├─ IAuthService.cs
│  │  ├─ IJwtService.cs
│  │  ├─ IUserAddressService.cs
│  │  └─ IUserProfileService.cs
│  ├─ Mappings
│  │  └─ MappingProfile.cs
│  └─ Services
│     ├─ ApplicationUserService.cs
│     ├─ AuthService.cs
│     ├─ UserAddressService.cs
│     └─ UserProfileService.cs
├─ appsettings.Development.json
├─ appsettings.json
├─ Dockerfile
├─ Domain
│  ├─ Entities
│  │  ├─ ApplicationRole.cs
│  │  ├─ ApplicationUser.cs
│  │  ├─ ApplicationUserRole.cs
│  │  ├─ RefreshToken.cs
│  │  ├─ UserAddress.cs
│  │  └─ UserProfile.cs
│  ├─ Enums
│  │  └─ Gender.cs
│  ├─ Filters
│  │  ├─ ApplicationUserFilterParams.cs
│  │  ├─ FilterParams.cs
│  │  ├─ RoleFilterParams.cs
│  │  ├─ UserAddressFilterParams.cs
│  │  └─ UserProfileFilterParams.cs
│  ├─ Interfaces
│  │  └─ IRepository.cs
│  └─ Specifications
│     ├─ RefreshTokenByTokenSpecification.cs
│     ├─ RefreshTokenByUserIdSpecification.cs
│     ├─ UserAddressByUserIdSpecification.cs
│     ├─ UserAddressDefaultSpecification.cs
│     ├─ UserAddressFilterSpecification.cs
│     ├─ UserProfileByUserIdSpecification.cs
│     ├─ UserProfileFilterSpecification.cs
│     └─ UserWithDetailFilterSpecification.cs
├─ dotnet-tools.json
├─ GlobalUsing.cs
├─ Identity.API.csproj
├─ Identity.API.http
├─ Identity.API.sln
├─ Infrastructure
│  ├─ Data
│  │  ├─ ApplicationDbContext.cs
│  │  ├─ Configurations
│  │  │  ├─ ApplicationRoleConfiguration.cs
│  │  │  ├─ ApplicationUserConfiguration.cs
│  │  │  ├─ ApplicationUserRoleConfiguration.cs
│  │  │  ├─ RefreshTokenConfiguration.cs
│  │  │  ├─ UserAddressConfiguration.cs
│  │  │  └─ UserProfileConfiguration.cs
│  │  ├─ Migrations
│  │  │  ├─ 20260105082417_InitialCreate.cs
│  │  │  ├─ 20260105082417_InitialCreate.Designer.cs
│  │  │  ├─ 20260105101932_UpdateUserProfileTable.cs
│  │  │  ├─ 20260105101932_UpdateUserProfileTable.Designer.cs
│  │  │  └─ ApplicationDbContextModelSnapshot.cs
│  │  └─ Seeders
│  │     ├─ SeedRole.cs
│  │     ├─ SeedUser.cs
│  │     └─ SeedUserRole.cs
│  ├─ Repostitories
│  │  └─ Repository.cs
│  └─ Services
│     └─ JwtService.cs
├─ IntegrationEvents
├─ Presentation
│  ├─ Controllers
│  │  ├─ ApplicationUserController.cs
│  │  ├─ AuthController.cs
│  │  ├─ UserAddressController.cs
│  │  └─ UserProfileController.cs
│  ├─ Extensions
│  │  └─ ClaimPrincipalExtensions.cs
│  └─ Middlewares
│     └─ GlobalExceptionHandler.cs
├─ Program.cs
├─ Properties
│  └─ launchSettings.json
└─ Protos
   └─ file.proto

```