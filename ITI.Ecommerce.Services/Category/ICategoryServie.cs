using DTOs;

namespace ITI.Ecommerce.Services
{
    public interface ICategoryServie
    {
        Task add(CategoryDto categoryDto);
        Task<IEnumerable<CategoryDto>> GetAll();
        Task<CategoryDto> GetById(int id);
        void Delete(CategoryDto categoryDto);
        void CDelete(int id);
        

        void Update( CategoryDto categoryDto);
    

        Task<IEnumerable<CategoryDto>> GetAllDeleted();
        void Restore(int id);
    }
}
