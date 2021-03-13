using System.Threading.Tasks;
using Cortside.WebApiStarter.Dto;

namespace Cortside.WebApiStarter.DomainService {
    public interface ISubjectService {
        Task Save(SubjectDto subject);
    }
}
