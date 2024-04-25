using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.UserHistoryDTOs;
using Microsoft.AspNetCore.Http;

namespace Domain.Concrete
{
    internal class UserHistoryDomain : DomainBase, IUserHistoryDomain
    {
        public UserHistoryDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        {
        }

        private IUserHistoryRepository userHistoryRepository => _unitOfWork.GetRepository<IUserHistoryRepository>();

        public IEnumerable<UserHistoryDTO> GetHistory()
        {
            var userHistoryList = userHistoryRepository.GetAll();
            return _mapper.Map<IEnumerable<UserHistoryDTO>>(userHistoryList);
        }
    }
}
