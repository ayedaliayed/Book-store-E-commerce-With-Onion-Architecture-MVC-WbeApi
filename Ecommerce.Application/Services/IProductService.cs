using Ecommerce.Dtos.Product;
using Ecommerce.Dtos.ViewResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public interface IProductService
    {
        Task<ResultView<CreateOrUpdateProductDTO>> Create(CreateOrUpdateProductDTO product);
        Task<ResultView<CreateOrUpdateProductDTO>> Update(CreateOrUpdateProductDTO product);
        Task<ResultView<CreateOrUpdateProductDTO>> HardDelete(CreateOrUpdateProductDTO product);
        Task<ResultView<CreateOrUpdateProductDTO>> SoftDelete(CreateOrUpdateProductDTO product);
        Task<ResultDataList<GetAllProductDTO>> GetAllPagination(int items, int pagenumber);
        Task<ResultView<CreateOrUpdateProductDTO>> GetOne(int id);
    }
}
