namespace Identity.API.Infrastructure.Data.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(up => up.Id);

        builder.Property(up => up.FullName)
               .IsRequired()
               .HasMaxLength(100);
        builder.Property(up => up.DateOfBirth)
               .IsRequired(false);
        builder.Property(up => up.Gender)
               .IsRequired(false);

        builder.HasOne(up => up.User)
               .WithOne(u => u.Profile)
               .HasForeignKey<UserProfile>(up => up.UserId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.ToTable("UserProfiles");
    }
}