using DAO.Helper;
using DTO.AreaDTO;

namespace Repository.Interface
{
    public interface IAreaRepo
    {
        Task<OperationResult<IEnumerable<AreaDto>>> GetAreas();
        Task<OperationResult<AreaDto>> GetAreaById(int id);
        Task<OperationResult<AreaCreate>> CreateArea(AreaCreate areaCreate);
        Task<OperationResult<AreaUpdate>> UpdateArea(AreaUpdate areaUpdate);
        IQueryable<AreaDto> GetDbSet();
    }
}
