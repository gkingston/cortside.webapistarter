using System.Threading.Tasks;
using Acme.WebApiStarter.Dto;

namespace Acme.WebApiStarter.DomainService {
    public interface ISubjectService {
        Task Save(SubjectDto subject);
    }
}
