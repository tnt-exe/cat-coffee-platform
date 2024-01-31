using DAO.Helper;
using DTO.CatDTO;

namespace Repository.Interface
{
    public interface ICatRepo
    {
        Task<OperationResult<IEnumerable<CatDto>>> GetCats();
        Task<OperationResult<CatDto>> GetCatById(int id);
        Task<OperationResult<CatCreate>> CreateCat(CatCreate catCreate);
        Task<OperationResult<CatUpdate>> UpdateCat(CatUpdate catUpdate);
        Task<OperationResult<object>> DeleteCat(int id);
    }
}
