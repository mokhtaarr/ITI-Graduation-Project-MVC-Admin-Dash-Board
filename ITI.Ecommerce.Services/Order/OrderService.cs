using DTOs;
using Microsoft.EntityFrameworkCore;

namespace ITI.Ecommerce.Services
{
    public class OrderService : IOrderService
    {

        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;
        public OrderService(ApplicationDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public async Task<int> add(OrderDto orderDto)
        {

            Order order = new Order()
            {

                CustomerId = orderDto.CustomerId,
                PaymentId = orderDto.PaymentId,
                OrderDate = orderDto.OrderDate,
                IsDeleted = false,
                status = "Pending",

            };
            order.OrderDate = DateTime.Now;
            await _context.Orders.AddAsync(order);
            foreach (var prd in orderDto.ProductList)
            {
                var orderProduct = new OrderProduct()
                {
                    OrderId = order.ID,
                    ProductId = prd.ID,
                    Quantity = prd.Quantity,
                };
                await _context.OrderProducts.AddAsync(orderProduct);
            }

            _context.SaveChanges();
            return order.ID;
        }



        public void Delete(int id)
        {
            var OrderDto = _context.Orders.FirstOrDefault(o => o.ID == id);
            if (OrderDto != null)
            {
                OrderDto.IsDeleted = true;
                _context.Update(OrderDto);
                _context.SaveChanges();
            }

        }

        public async Task<IEnumerable<OrderDto>> GetAll()
        {
            List<OrderDto> orderList = new List<OrderDto>();


            var orders = await _context.Orders.Where(o => o.IsDeleted == false).ToListAsync();
            foreach (var order in orders)
            {
                var orderProductIds = await _context.OrderProducts.Where(op => op.OrderId == order.ID).Select(op => op.ProductId).ToListAsync();

                OrderDto orderDto = new OrderDto();
                orderDto.ID = order.ID;
                orderDto.CustomerId = order.CustomerId;
                orderDto.PaymentId = order.PaymentId;
                orderDto.OrderDate = order.OrderDate;
                orderDto.Status = order.status;

                var prdDtoList = new List<ProductDto>();
                var orderprds = await _context.OrderProducts.Where(op => op.OrderId == order.ID).ToListAsync();
                foreach (var item in orderprds)
                {
                    var prd = await _productService.GetById(item.ProductId);
                    prd.Quantity = item.Quantity;
                    prdDtoList.Add(prd);
                }
                orderDto.ProductList = prdDtoList;
                orderList.Add(orderDto);
            }
            return orderList;
        }

        public async Task<OrderDto> GetById(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(o => o.ID == id);
            var orderProductIds = await _context.OrderProducts.Where(op => op.OrderId == id).Select(op => op.ProductId).ToListAsync();
            if (order == null)
            {
                throw new Exception("this order is not found");
            }
            else
            {
                OrderDto orderDto = new OrderDto()
                {
                    ID = order.ID,
                    CustomerId = order.CustomerId,
                    PaymentId = order.PaymentId,
                    OrderDate = order.OrderDate,
                    Status = order.status
                };

                var prdDtoList = new List<ProductDto>();

                var orderprds = await _context.OrderProducts.Where(op => op.OrderId == order.ID).ToListAsync();
                foreach (var item in orderprds)
                {
                    var prd = await _productService.GetById(item.ProductId);
                    prd.Quantity = item.Quantity;
                    prdDtoList.Add(prd);
                }
                orderDto.ProductList = prdDtoList;
                
                return orderDto;
            }
        }


        public async Task<IEnumerable<OrderDto>> GetByCustomerId(string CustomerId)
        {
            List<OrderDto> orderDtoList = new List<OrderDto>();

            var orders = await _context.Orders.Where(o => o.IsDeleted == false && o.CustomerId == CustomerId).ToListAsync();

            foreach (var order in orders)
            {
                var orderProductIds = await _context.OrderProducts.Where(op => op.OrderId == order.ID).Select(op => op.ProductId).ToListAsync();

                OrderDto orderDto = new OrderDto()
                {
                    ID = order.ID,
                    CustomerId = order.CustomerId,
                    OrderDate = order.OrderDate,
                    PaymentId = order.PaymentId,
                    Status = order.status

                };

                var prdDtoList = new List<ProductDto>();

                var orderprds = await _context.OrderProducts.Where(op => op.OrderId == order.ID).ToListAsync();
                foreach (var item in orderprds)
                {
                    var prd = await _productService.GetById(item.ProductId);
                    prd.Quantity = item.Quantity;
                    prdDtoList.Add(prd);
                }
                orderDto.ProductList = prdDtoList;
                orderDtoList.Add(orderDto);

            }

            return orderDtoList;
        }


        public async Task Update(OrderDto orderDto)
        {
            var order =  _context.Orders.FirstOrDefault(o => o.ID == orderDto.ID);
            var productInOrder = await _context.OrderProducts.Where(op => op.OrderId == orderDto.ID).ToListAsync();

            foreach (var orderPrd in productInOrder)
            {
                _context.OrderProducts.Attach(orderPrd);
                _context.OrderProducts.Remove(orderPrd);
            }

            order.CustomerId = orderDto.CustomerId;
            order.PaymentId = orderDto.PaymentId;
            order.OrderDate = orderDto.OrderDate;
            foreach (var prd in orderDto.ProductList)
            {
                var orderProduct = new OrderProduct()
                {
                    OrderId = order.ID,
                    ProductId = prd.ID
                };
               order.OrderProducts.Add(orderProduct);
            }
            _context.SaveChanges();
        }
        public void ChangeToDelivered(int orderid)
        {
            var order = _context.Orders.FirstOrDefault(o => o.ID == orderid);
            order.status = "Delivered";
            _context.SaveChanges();
        }


        public async Task<IEnumerable<OrderDto>> GetAllDelivered()
        {
            List<OrderDto> orderList = new List<OrderDto>();


            var orders = await _context.Orders.Where(o => o.IsDeleted == false && o.status == "Delivered").ToListAsync();
            foreach (var order in orders)
            {
                var orderProductIds = await _context.OrderProducts.Where(op => op.OrderId == order.ID).Select(op => op.ProductId).ToListAsync();

                OrderDto orderDto = new OrderDto();
                orderDto.ID = order.ID;
                orderDto.CustomerId = order.CustomerId;
                orderDto.PaymentId = order.PaymentId;
                orderDto.OrderDate = order.OrderDate;
                orderDto.IsDeleted = order.IsDeleted;
                orderDto.Status = order.status;



                var prdDtoList = new List<ProductDto>();

                foreach (var PrdId in orderProductIds)
                {
                    var prd = await _productService.GetById(PrdId);
                    prdDtoList.Add(prd);
                }
                orderDto.ProductList = prdDtoList;
                orderList.Add(orderDto);
            }
            return orderList;
        }

        public async Task<IEnumerable<OrderDto>> GetAllPending()
        {
            List<OrderDto> orderList = new List<OrderDto>();
            var orders = await _context.Orders.Where(o => o.IsDeleted == false && o.status == "Pending").ToListAsync();
            foreach (var order in orders)
            {
                var orderProductIds = await _context.OrderProducts.Where(op => op.OrderId == order.ID).Select(op => op.ProductId).ToListAsync();

                OrderDto orderDto = new OrderDto();
                orderDto.ID = order.ID;
                orderDto.CustomerId = order.CustomerId;
                orderDto.PaymentId = order.PaymentId;
                orderDto.OrderDate = order.OrderDate;
                orderDto.IsDeleted = order.IsDeleted;
                orderDto.Status = order.status;
               
               var prdDtoList = new List<ProductDto>();

                foreach (var PrdId in orderProductIds)
                {
                    var prd = await _productService.GetById(PrdId);
                    prdDtoList.Add(prd);
                }
                orderDto.ProductList= prdDtoList;
                orderList.Add(orderDto);
            }
            return orderList;
        }
    }
}
