using AutoMapper;
using Ecommerce.Application.Contract;
using Ecommerce.Dtos.Categories;
using Ecommerce.Dtos.Product;
using Ecommerce.Dtos.ViewResult;
using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public class CategoryService:ICategoryService
    {

        private ICategoryRepository _categoryService;
        private IMapper _mapper;

        public CategoryService(ICategoryRepository categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;

        }
        public async Task<ResultView<CategoryDTO>> Create(CategoryDTO cat)
        {

            var oldcat = (await _categoryService.GetAllAsync()).Where(p => p.Name == cat.Name).FirstOrDefault();
            if (oldcat != null)
            {
                return new ResultView<CategoryDTO> { Enttiy = null, IsSuccess = false, Message = "Already Exist" };
            }
            else
            {
                var prd = _mapper.Map<Category>(cat);
                var Newcat = await _categoryService.CreateAsync(prd);
                await _categoryService.SaveChangesAsync();
                var newcatDTO = _mapper.Map<CategoryDTO>(Newcat);
                return new ResultView<CategoryDTO> { Enttiy = newcatDTO, IsSuccess = true, Message = "Created Successfully" };

            }

        }

        public async Task<ResultView<CategoryDTO>> Update(CategoryDTO category)
        {

            var oldCat = (await _categoryService.GetAllAsync()).Where(p => p.Name == category.Name && p.Id != category.Id).FirstOrDefault();
            if (oldCat != null)
            {
                return new ResultView<CategoryDTO> { Enttiy = null, IsSuccess = false, Message = "Already Exist" };
            }
            else
            {
                var prd = _mapper.Map<Category>(category);
                var NewCategory = await _categoryService.UpdateAsync(prd);
                await _categoryService.SaveChangesAsync();
                var newcategoryDTO = _mapper.Map<CategoryDTO>(NewCategory);
                return new ResultView<CategoryDTO> { Enttiy = newcategoryDTO, IsSuccess = true, Message = "Update Successfully" };

            }

        }

        public async Task<ResultView<CategoryDTO>> HardDelete(CategoryDTO category)
        {
            try
            {
                var cat = _mapper.Map<Category>(category);
                var oldcategory = _categoryService.DeleteAsync(cat);
                await _categoryService.SaveChangesAsync();
                var oldcategoryDTO = _mapper.Map<CategoryDTO>(oldcategory);
                return new ResultView<CategoryDTO> { Enttiy = oldcategoryDTO, IsSuccess = true, Message = " Hard Delete Successfully" };

            }
            catch (Exception ex)
            {
                return new ResultView<CategoryDTO> { Enttiy = null, IsSuccess = false, Message = ex.Message };

            }

        }

        public async Task<ResultView<CategoryDTO>> SoftDelete(CategoryDTO product)
        {
            try
            {
                //var pro = _mapper.Map<Product>(product);
                //pro.IsDeleted = true;
                //var oldprodcut = _productRepository.UpdateAsync(pro);


                var PRd = _mapper.Map<Product>(product);
                var oldprodcut = (await _categoryService.GetAllAsync()).FirstOrDefault(p => p.Id == product.Id && p.IsDeleted != true);
                oldprodcut.IsDeleted = true;
                await _categoryService.SaveChangesAsync();
                var oldproductDTO = _mapper.Map<CategoryDTO>(oldprodcut);
                return new ResultView<CategoryDTO> { Enttiy = oldproductDTO, IsSuccess = true, Message = " Softe Delete Successfully" };

            }
            catch (Exception ex)
            {
                return new ResultView<CategoryDTO> { Enttiy = null, IsSuccess = false, Message = ex.Message };

            }

        }
        public async Task<ResultView<CategoryDTO>> GetOne(int id)
        {
            var prd = await _categoryService.GetByIdAsync(id);

            if (prd != null)
            {
                var prdDTO = _mapper.Map<CategoryDTO>(prd);
                return new ResultView<CategoryDTO>() { Enttiy = prdDTO, Message = "Done", IsSuccess = true };
            }
            else
            {
                return new ResultView<CategoryDTO>() { Enttiy = null, Message = "Not Found ", IsSuccess = false };
            }
        }





        public async Task<List<CategoryDTO>> GetAllPagination()
        {
            var AllData = (await _categoryService.GetAllAsync());

           
            var Prds = AllData.Select(p => new CategoryDTO()
                                              {
                                                  Id = p.Id,
                                                  Name = p.Name}).ToList();


            return Prds;


        }









    }
}
