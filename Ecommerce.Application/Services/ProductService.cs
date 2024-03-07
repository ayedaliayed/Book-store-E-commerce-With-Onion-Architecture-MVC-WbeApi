using AutoMapper;
using Ecommerce.Application.Contract;
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
    public class ProductService:IProductService
    {
        
        
            private IProductRepository _productRepository;
            private IMapper _mapper;

            public ProductService(IProductRepository productRepository, IMapper mapper)
            {
                _productRepository = productRepository;
                _mapper = mapper;

            }
            public async Task<ResultView<CreateOrUpdateProductDTO>> Create(CreateOrUpdateProductDTO product)
            {

                var oldProduct = (await _productRepository.GetAllAsync()).Where(p => p.Name == product.Name).FirstOrDefault();
                if (oldProduct != null)
                {
                    return new ResultView<CreateOrUpdateProductDTO> { Enttiy = null, IsSuccess = false, Message = "Already Exist" };
                }
                else
                {
                    var prd = _mapper.Map<Product>(product);
                    var NewProduct = await _productRepository.CreateAsync(prd);
                    await _productRepository.SaveChangesAsync();
                    var newProductDTO = _mapper.Map<CreateOrUpdateProductDTO>(NewProduct);
                    return new ResultView<CreateOrUpdateProductDTO> { Enttiy = newProductDTO, IsSuccess = true, Message = "Created Successfully" };

                }

            }

            public async Task<ResultView<CreateOrUpdateProductDTO>> Update(CreateOrUpdateProductDTO product)
            {

                var oldProduct = (await _productRepository.GetAllAsync()).Where(p => p.Name == product.Name && p.Id != product.Id).FirstOrDefault();
                if (oldProduct != null)
                {
                    return new ResultView<CreateOrUpdateProductDTO> { Enttiy = null, IsSuccess = false, Message = "Already Exist" };
                }
                else
                {
                    var prd = _mapper.Map<Product>(product);
                    var NewProduct = await _productRepository.UpdateAsync(prd);
                    await _productRepository.SaveChangesAsync();
                    var newProductDTO = _mapper.Map<CreateOrUpdateProductDTO>(NewProduct);
                    return new ResultView<CreateOrUpdateProductDTO> { Enttiy = newProductDTO, IsSuccess = true, Message = "Update Successfully" };

                }

            }

            public async Task<ResultView<CreateOrUpdateProductDTO>> HardDelete(CreateOrUpdateProductDTO product)
            {
                try
                {
                    var pro = _mapper.Map<Product>(product);
                    var oldprodcut = _productRepository.DeleteAsync(pro);
                    await _productRepository.SaveChangesAsync();
                    var oldproductDTO = _mapper.Map<CreateOrUpdateProductDTO>(oldprodcut);
                    return new ResultView<CreateOrUpdateProductDTO> { Enttiy = oldproductDTO, IsSuccess = true, Message = " Hard Delete Successfully" };

                }
                catch (Exception ex)
                {
                    return new ResultView<CreateOrUpdateProductDTO> { Enttiy = null, IsSuccess = false, Message = ex.Message };

                }

            }

            public async Task<ResultView<CreateOrUpdateProductDTO>> SoftDelete(CreateOrUpdateProductDTO product)
            {
                try
                {
                    //var pro = _mapper.Map<Product>(product);
                    //pro.IsDeleted = true;
                    //var oldprodcut = _productRepository.UpdateAsync(pro);


                    var PRd = _mapper.Map<Product>(product);
                    var oldprodcut = (await _productRepository.GetAllAsync()).FirstOrDefault(p => p.Id == product.Id && p.IsDeleted != true);
                    oldprodcut.IsDeleted = true;
                    await _productRepository.SaveChangesAsync();
                    var oldproductDTO = _mapper.Map<CreateOrUpdateProductDTO>(oldprodcut);
                    return new ResultView<CreateOrUpdateProductDTO> { Enttiy = oldproductDTO, IsSuccess = true, Message = " Softe Delete Successfully" };

                }
                catch (Exception ex)
                {
                    return new ResultView<CreateOrUpdateProductDTO> { Enttiy = null, IsSuccess = false, Message = ex.Message };

                }

            }

            public async Task<ResultDataList<GetAllProductDTO>> GetAllPagination(int items, int pagenumber)
            {
                var AllData = (await _productRepository.GetAllAsync());

                //var prds=AllData.Skip(items*(pagenumber-1)).Take(items).ToList();
                //var prdsdto=_mapper.Map<GetAllProductDTO>(prds);

                var Prds = AllData.Skip(items * (pagenumber - 1)).Take(items).Where(p=>p.IsDeleted==false)
                                                  .Select(p => new GetAllProductDTO()
                                                  {
                                                      Id = p.Id,
                                                      Name = p.Name,
                                                      Price = p.Price,
                                                      CategoryName = p.Category.Name
                                                  }).ToList();

                ResultDataList<GetAllProductDTO> resultDataList = new ResultDataList<GetAllProductDTO>();
                resultDataList.Count = Prds.Count();
                resultDataList.Entities = Prds;

                return resultDataList;


            }

            public async Task<ResultView<CreateOrUpdateProductDTO>> GetOne(int id)
            {
                var prd = await _productRepository.GetByIdAsync(id);

                if (prd != null)
                {
                    var prdDTO = _mapper.Map<CreateOrUpdateProductDTO>(prd);
                    return new ResultView<CreateOrUpdateProductDTO>() { Enttiy = prdDTO, Message = "Done", IsSuccess = true };
                }
                else
                {
                    return new ResultView<CreateOrUpdateProductDTO>() { Enttiy = null, Message = "Not Found ", IsSuccess = false };
                }
            }


        }
    }
