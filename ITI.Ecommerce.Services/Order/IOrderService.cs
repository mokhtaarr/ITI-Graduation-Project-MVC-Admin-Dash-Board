using DTOs;

namespace ITI.Ecommerce.Services
{
    public interface IOrderService
    {
        Task<int> add(OrderDto orderDto);
        Task<IEnumerable<OrderDto>> GetAll();
        Task<IEnumerable<OrderDto>> GetByCustomerId(string CustomerId);
        Task<OrderDto> GetById(int id); 
         void Delete(int id);

        Task Update(OrderDto orderDto);

        Task<IEnumerable<OrderDto>> GetAllPending();
        Task<IEnumerable<OrderDto>> GetAllDelivered();
        public void ChangeToDelivered(int orderid);
    }
}
