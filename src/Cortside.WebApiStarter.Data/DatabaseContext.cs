using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cortside.DomainEvent.EntityFramework;
using Cortside.WebApiStarter.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cortside.WebApiStarter.Data {
    public partial class DatabaseContext : DbContext {
        private readonly IHttpContextAccessor httpContextAccessor;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IHttpContextAccessor httpContextAccessor) : base(options) {
            this.httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Domain.Widget> WebApiStarter { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        public Task<int> SaveChangesAsync() {
            SetAuditableEntityValues();
            return base.SaveChangesAsync(default(System.Threading.CancellationToken));
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            SetAuditableEntityValues();
            return (await base.SaveChangesAsync(true, cancellationToken).ConfigureAwait(false));
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken)) {
            SetAuditableEntityValues();
            return (await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAwait(false));
        }

        public override int SaveChanges() {
            SetAuditableEntityValues();
            return base.SaveChanges();
        }

        private void SetAuditableEntityValues() {
            // check for subject in subjects set and either create or get to attach to AudibleEntity
            var updatingSubject = GetCurrentSubject();
            ChangeTracker.DetectChanges();
            var modified = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified || x.State == EntityState.Added);
            var added = ChangeTracker.Entries().Where(x => x.State == EntityState.Added);

            foreach (var item in modified) {
                if (item.Entity is AuditableEntity entity) {
                    ((AuditableEntity)(item.Entity)).LastModifiedSubject = updatingSubject;
                    ((AuditableEntity)(item.Entity)).LastModifiedDate = DateTime.Now.ToUniversalTime();
                }
            }

            foreach (var item in added) {
                if (item.Entity is AuditableEntity entity) {
                    ((AuditableEntity)(item.Entity)).CreatedSubject = updatingSubject;
                    ((AuditableEntity)(item.Entity)).CreatedDate = DateTime.Now.ToUniversalTime();
                }
            }
        }

        /// <summary>
        /// Gets or creates the current subject record.
        /// </summary>
        /// <returns></returns>
        private Subject GetCurrentSubject() {
            var currentUser = GetCurrentUser();
            Guid subjectId = currentUser != null ? Guid.Parse(currentUser) : Guid.Empty;

            var subject = Subjects.Local.FirstOrDefault(s => s.SubjectId == subjectId);
            if (subject == null) {
                subject = Subjects.FirstOrDefault(s => s.SubjectId == subjectId);
            }
            if (subject == null) {
                subject = new Subject {
                    CreatedDate = DateTime.Now.ToUniversalTime(),
                    SubjectId = subjectId
                };
                if (currentUser != null) {
                    if (httpContextAccessor != null) {
                        var httpContext = httpContextAccessor.HttpContext;
                        if (httpContext != null) {
                            subject.GivenName = httpContext.User.FindFirst("given_name")?.Value;
                            subject.Name = httpContext.User.FindFirst("name")?.Value;
                            subject.FamilyName = httpContext.User.FindFirst("family_name")?.Value;
                            subject.UserPrincipalName = httpContext.User.FindFirst("upn")?.Value ?? httpContext.User.FindFirst("client_id")?.Value;
                        }
                    }
                }
                Subjects.Add(subject);
            }
            return subject;
        }

        private string GetCurrentUser() {
            // IHttpContextAccessor will have be be setup in IoC and injected into context constructor
            if (httpContextAccessor != null) {
                var httpContext = httpContextAccessor.HttpContext;
                if (httpContext != null) {
                    // If it returns null, even when the user was authenticated, you may try to get the value of a specific claim 
                    var authenticatedUserId = httpContext.User.FindFirst("sub")?.Value;
                    if (authenticatedUserId != null) {
                        return authenticatedUserId;
                    }
                }
            }
            //If not authenticated return null.
            return null;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Domain.Widget>()
                .HasOne(p => p.CreatedSubject);
            modelBuilder.Entity<Domain.Widget>()
                .HasOne(p => p.LastModifiedSubject);
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.AddDomainEventOutbox();

            SetDateTime(modelBuilder);
            SetCascadeDelete(modelBuilder);
        }

        private void SetDateTime(ModelBuilder builder) {
            // 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM
            var min = new DateTime(1753, 1, 1, 0, 0, 0);
            var max = new DateTime(9999, 12, 31, 23, 59, 59);

            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v < min ? min : v > max ? max : v,
                v => v < min ? min : v > max ? max : v);

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? v < min ? min : v > max ? max : v : v,
                v => v.HasValue ? v < min ? min : v > max ? max : v : v);

            foreach (var entityType in builder.Model.GetEntityTypes()) {
                foreach (var property in entityType.GetProperties()) {
                    if (property.ClrType == typeof(DateTime)) {
                        property.SetValueConverter(dateTimeConverter);
                    } else if (property.ClrType == typeof(DateTime?)) {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }
        }

        public void SetCascadeDelete(ModelBuilder builder) {
            var fks = builder.Model.GetEntityTypes().SelectMany(t => t.GetDeclaredForeignKeys());
            foreach (var fk in fks) {
                fk.DeleteBehavior = DeleteBehavior.NoAction;
            }
        }
    }
}
