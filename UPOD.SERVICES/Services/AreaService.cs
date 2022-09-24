using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.SERVICES.Helpers;

namespace UPOD.SERVICES.Services
{

    public interface IAreaService
    {
        Task<ResponseModel<AreaResponse>> GetListAreas(PaginationRequest model);
        Task<ObjectModelResponse> CreateArea(AreaRequest model);
        Task<ObjectModelResponse> UpdateArea(Guid id, AreaRequest model);
        Task<ObjectModelResponse> DisableArea(Guid id);
    }

    public class AreaService : IAreaService
    {
        private readonly Database_UPODContext _context;
        public AreaService(Database_UPODContext context)
        {
            _context = context;
        }


        public async Task<ResponseModel<AreaResponse>> GetListAreas(PaginationRequest model)
        {
            var areas = await _context.Areas.Where(a => a.IsDelete == false).Select(a => new AreaResponse
            {
                id = a.Id,
                code = a.Code,
                area_name = a.AreaName,
                description = a.Description,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate

            }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<AreaResponse>(areas)
            {
                Total = areas.Count,
                Type = "Areas"
            };
        }

        public async Task<ObjectModelResponse> CreateArea(AreaRequest model)
        {
            var code_number = await GetLastCode();
            var code = CodeHelper.GeneratorCode("AR", code_number + 1);
            var area = new Area
            {
                Id = Guid.NewGuid(),
                Code = code,
                Description = model.description,
                AreaName = model.area_name,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now

            };
            var data = new AreaResponse();
            var message = "blank";
            var status = 500;
            var area_id = await _context.Areas.Where(x => x.Id.Equals(area.Id)).FirstOrDefaultAsync();
            if (area_id != null)
            {
                status = 400;
                message = "AreaId is already exists!";
            }
            else
            {
                message = "Successfully";
                status = 201;
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

            }

            return new ObjectModelResponse(data)
            {
                Message = message,
                Status = status,
                Type = "Area"
            };
        }


        public async Task<ObjectModelResponse> DisableArea(Guid id)
        {
            var area = await _context.Areas.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            area!.IsDelete = true;
            area.UpdateDate = DateTime.Now;
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
                UpdateDate = DateTime.Now
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
