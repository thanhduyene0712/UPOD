using Microsoft.EntityFrameworkCore;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponseModels;
using UPOD.REPOSITORIES.ResponseViewModel;
using UPOD.SERVICES.Helpers;

namespace UPOD.SERVICES.Services
{

    public interface IAreaService
    {
        Task<ResponseModel<AreaResponse>> GetListAreas(PaginationRequest model, SearchRequest value);
        Task<ObjectModelResponse> CreateArea(AreaRequest model);
        Task<ObjectModelResponse> UpdateArea(Guid id, AreaRequest model);
        Task<ObjectModelResponse> DisableArea(Guid id);
        Task<ObjectModelResponse> GetDetailsArea(Guid id);
        Task<ResponseModel<TechnicianViewResponse>> GetListTechniciansByAreaId(PaginationRequest model, Guid id);
    }

    public class AreaServices : IAreaService
    {
        private readonly Database_UPODContext _context;
        public AreaServices(Database_UPODContext context)
        {
            _context = context;
        }


        public async Task<ResponseModel<AreaResponse>> GetListAreas(PaginationRequest model, SearchRequest value)
        {
            var total = await _context.Areas.Where(a => a.IsDelete == false).ToListAsync();
            var areas = new List<AreaResponse>();
            if (value.search == null)
            {
                total = await _context.Areas.Where(a => a.IsDelete == false).ToListAsync();
                areas = await _context.Areas.Where(a => a.IsDelete == false).Select(a => new AreaResponse
                {
                    id = a.Id,
                    code = a.Code,
                    area_name = a.AreaName,
                    description = a.Description,
                    is_delete = a.IsDelete,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate

                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }
            else
            {
                total = await _context.Areas.Where(a => a.IsDelete == false
                && (a.Code!.Contains(value.search.Trim())
                || a.AreaName!.Contains(value.search.Trim())
                || a.Description!.Contains(value.search.Trim()))).ToListAsync();
                areas = await _context.Areas.Where(a => a.IsDelete == false
                && (a.Code!.Contains(value.search.Trim())
                || a.AreaName!.Contains(value.search.Trim())
                || a.Description!.Contains(value.search.Trim()))).Select(a => new AreaResponse
                {
                    id = a.Id,
                    code = a.Code,
                    area_name = a.AreaName,
                    description = a.Description,
                    is_delete = a.IsDelete,
                    create_date = a.CreateDate,
                    update_date = a.UpdateDate

                }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            }

            return new ResponseModel<AreaResponse>(areas)
            {
                Total = total.Count,
                Type = "Areas"
            };
        }
        public async Task<ObjectModelResponse> GetDetailsArea(Guid id)
        {
            var areas = await _context.Areas.Where(a => a.IsDelete == false && a.Id.Equals(id)).Select(a => new AreaResponse
            {
                id = a.Id,
                code = a.Code,
                area_name = a.AreaName,
                description = a.Description,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate

            }).FirstOrDefaultAsync();
            return new ObjectModelResponse(areas!)
            {
                Type = "Area"
            };
        }
        public async Task<ResponseModel<TechnicianViewResponse>> GetListTechniciansByAreaId(PaginationRequest model, Guid id)
        {
            var total = await _context.Technicians.Where(a => a.IsDelete == false && a.AreaId.Equals(id)).ToListAsync();
            var technician = await _context.Technicians.Where(a => a.IsDelete == false && a.AreaId.Equals(id)).Select(a => new TechnicianViewResponse
            {
                id = a.Id,
                code = a.Code,
                tech_name = a.TechnicianName,
                phone = a.Telephone,
                email = a.Email,
            }).OrderBy(x => x.code).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<TechnicianViewResponse>(technician)
            {
                Total = total.Count,
                Type = "Technicians"
            };
        }

        public async Task<ObjectModelResponse> CreateArea(AreaRequest model)
        {
            var area_id = Guid.NewGuid();
            while (true)
            {
                var area_dup = await _context.Areas.Where(x => x.Id.Equals(area_id)).FirstOrDefaultAsync();
                if (area_dup == null)
                {
                    break;
                }
                else
                {
                    area_id = Guid.NewGuid();
                }
            }
            var code_number = await GetLastCode();
            var code = CodeHelper.GeneratorCode("AR", code_number + 1);
            while (true)
            {
                var code_dup = await _context.Areas.Where(a => a.Code.Equals(code)).FirstOrDefaultAsync();
                if (code_dup == null)
                {
                    break;
                }
                else
                {
                    code = "AR-" + code_number++.ToString();
                }
            }
            var area = new Area
            {
                Id = area_id,
                Code = code,
                Description = model.description,
                AreaName = model.area_name,
                IsDelete = false,
                CreateDate = DateTime.UtcNow.AddHours(7),
                UpdateDate = DateTime.UtcNow.AddHours(7)

            };
            var data = new AreaResponse();
            await _context.Areas.AddAsync(area);
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new AreaResponse
                {
                    id = area.Id,
                    code = area.Code,
                    area_name = area.AreaName,
                    description = area.Description,
                    is_delete = area.IsDelete,
                    create_date = area.CreateDate,
                    update_date = area.UpdateDate
                };
            }



            return new ObjectModelResponse(data)
            {

                Type = "Area"
            };
        }


        public async Task<ObjectModelResponse> DisableArea(Guid id)
        {
            var area = await _context.Areas.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            area!.IsDelete = true;
            area.UpdateDate = DateTime.UtcNow.AddHours(7);
            _context.Areas.Update(area);
            var data = new AreaResponse();
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {

            }
            data = new AreaResponse
            {
                id = id,
                code = area.Code,
                description = area.Description,
                area_name = area.AreaName,
                is_delete = area.IsDelete,
                create_date = area.CreateDate,
                update_date = area.UpdateDate
            };
            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Area"
            };
        }
        public async Task<ObjectModelResponse> UpdateArea(Guid id, AreaRequest model)
        {
            var area = await _context.Areas.Where(a => a.Id.Equals(id)).Select(x => new Area
            {
                Id = id,
                Code = x.Code,
                AreaName = model.area_name,
                Description = model.description,
                IsDelete = x.IsDelete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.UtcNow.AddHours(7)
            }).FirstOrDefaultAsync();
            _context.Areas.Update(area!);
            var data = new AreaResponse();
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new AreaResponse
                {
                    id = area!.Id,
                    code = area.Code,
                    area_name = area.AreaName,
                    description = area.Description,
                    is_delete = area.IsDelete,
                    create_date = area.CreateDate,
                    update_date = area.UpdateDate
                };
            }

            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Area"
            };
        }
        private async Task<int> GetLastCode()
        {
            var area = await _context.Areas.OrderBy(x => x.Code).LastOrDefaultAsync();
            return CodeHelper.StringToInt(area!.Code!);
        }
    }
}
