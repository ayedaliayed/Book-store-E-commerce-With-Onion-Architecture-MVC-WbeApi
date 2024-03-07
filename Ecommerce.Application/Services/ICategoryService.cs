using Ecommerce.Dtos.Categories;
using Ecommerce.Dtos.ViewResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public interface ICategoryService
    {
        Task<ResultView<CategoryDTO>> Create(CategoryDTO product);
        Task<ResultView<CategoryDTO>> Update(CategoryDTO product);
        Task<ResultView<CategoryDTO>> HardDelete(CategoryDTO product);
        Task<ResultView<CategoryDTO>> SoftDelete(CategoryDTO product);
        Task<List<CategoryDTO>> GetAllPagination();
        Task<ResultView<CategoryDTO>> GetOne(int id);
    }
}
