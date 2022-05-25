using System;
using System.Linq;
using System.Threading.Tasks;
using Acme.WebApiStarter.Data;
using Acme.WebApiStarter.Domain;
using Acme.WebApiStarter.Dto;

namespace Acme.WebApiStarter.DomainService {
    public class SubjectService : ISubjectService {
        private readonly DatabaseContext dbContext;

        public SubjectService(DatabaseContext dbContext) {
            this.dbContext = dbContext;
        }
        public async Task SaveAsync(SubjectDto subject) {
            var subjectRow = dbContext.Subjects.FirstOrDefault(s => s.SubjectId == subject.SubjectId);
            if (subjectRow == null) {
                dbContext.Subjects.Add(new Subject() {
                    SubjectId = subject.SubjectId,
                    FamilyName = subject.FamilyName,
                    GivenName = subject.GivenName,
                    Name = subject.Name,
                    UserPrincipalName = subject.UserPrincipalName,
                    CreatedDate = DateTime.UtcNow
                });
            } else {
                subjectRow.Name = subject.Name;
                subjectRow.GivenName = subject.GivenName;
                subjectRow.FamilyName = subject.FamilyName;
                subjectRow.UserPrincipalName = subject.UserPrincipalName;
            }
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
