using DAO.Helper;
using DTO.TimeFrameDTO;

namespace Repository.Interface
{
    public interface ITimeFrameRepo
    {
        Task<OperationResult<IEnumerable<TimeFrameDto>>> GetTimeFrames();
        Task<OperationResult<TimeFrameDto>> GetTimeFrameById(int id);
        Task<OperationResult<TimeFrameCreate>> CreateTimeFrame(TimeFrameCreate timeFrameCreate);
        Task<OperationResult<TimeFrameUpdate>> UpdateTimeFrame(TimeFrameUpdate timeFrameUpdate);
        IQueryable<TimeFrameDto> GetDbSet();
    }
}
