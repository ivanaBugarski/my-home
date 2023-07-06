using Microsoft.EntityFrameworkCore;
using RealEstateApi.Model;
using RealEstateApi.Interfaces;
using RealEstateApi.Contracts.Data;
using Stripe;

namespace RealEstateApi;

public class CoreContext : DbContext
{

    private readonly IHttpContextAccessor _httpContextAccessor;
    public DbSet<Advertisement> Advertisements { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Messages> Messages { get; set; }
    public DbSet<User> Users { get; set; }

    public CoreContext(DbContextOptions options)
    : base(options)
    {
    }

    public CoreContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor)
    : base(options)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    //public int SaveUserChanges(User user)
    //{
    //    return SaveUserChanges(user, acceptAllChangesOnSuccess: true);
    //}

    //public int SaveUserChanges(User user, bool acceptAllChangesOnSuccess)
    //{
    //    SetTimestampedObjectFields();
    //    //SetUserstampedObjectFields(user);
    //    return base.SaveChanges(acceptAllChangesOnSuccess);
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Advertisement>()
            .HasQueryFilter(a => !a.IsDeleted)
            .HasOne<User>(u => u.User)
            .WithMany(r => r.Advertisements)
            .HasForeignKey(a => a.UserId);

        modelBuilder.Entity<Advertisement>()
        .HasMany<Image>(a => a.Images)
        .WithOne()
        .HasForeignKey(a => a.AdvertisementId);

        modelBuilder.Entity<Image>()
            .HasQueryFilter(m => !m.IsDeleted);
        modelBuilder.Entity<Messages>().HasQueryFilter(m => !m.IsDeleted);
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        SetTimestampedObjectFields();
        //SetUserstampedObjectFields();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        SetTimestampedObjectFields();
        //SetUserstampedObjectFields();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    private void SetTimestampedObjectFields()
    {
        var baseEntities = ChangeTracker.Entries<ITimestampedObject>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

        baseEntities.ForEach(e =>
        {
            var entity = e.Entity;
            if (e.State == EntityState.Added)
                entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
        });
    }

    //private void SetUserstampedObjectFields(User user)
    //{
    //    var baseEntities = ChangeTracker.Entries<IUserstampedObject>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

    //    baseEntities.ForEach(e =>
    //    {
    //        var entity = e.Entity;
    //        if (e.State == EntityState.Added)
    //            entity.CreatedBy = user.Id;
    //        entity.UpdatedBy = user.Id;
    //    });
    //}

    //private void SetUserstampedObjectFields()
    //{
    //    var baseEntities = ChangeTracker.Entries<IUserstampedObject>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

    //    if (baseEntities.IsNullOrEmpty())
    //        return;

    //    var userClaims = _httpContextAccessor.HttpContext.User.Claims;
    //    var path = _httpContextAccessor.HttpContext.Request.Path;

    //    if (userClaims.SingleOrDefault(x => x.Type.Equals(CommonConstants.DB_ID)) is null)
    //        throw new ClaimNullException();

    //    if (!path.Value.Contains("/SetPassword") && !path.Value.Contains("/ForgotPassword") && String.IsNullOrEmpty(userClaims.SingleOrDefault(x => x.Type.Equals(CommonConstants.DB_ID))?.Value))
    //    {
    //        throw new CurrentUserNotFoundException();
    //    }
    //    string userDbId = userClaims.SingleOrDefault(x => x.Type.Equals(CommonConstants.DB_ID))?.Value;



        //baseEntities.ForEach(e =>
        //{
        //    var entity = e.Entity;
        //    if (e.State == EntityState.Added)
        //        entity.CreatedBy = userDbId;
        //    entity.UpdatedBy = userDbId;
        //});
    //}
}