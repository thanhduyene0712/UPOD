using Microsoft.EntityFrameworkCore;
using UPOD.REPOSITORIES.Models;
using UPOD.REPOSITORIES.RequestModels;
using UPOD.REPOSITORIES.ResponeModels;
using UPOD.REPOSITORIES.ResponseViewModel;
using UPOD.SERVICES.Helpers;

namespace UPOD.SERVICES.Services
{

    public interface IGuidelineService
    {
        Task<ResponseModel<GuidelineResponse>> GetListGuidelines(PaginationRequest model);
        Task<ObjectModelResponse> CreateGuideline(GuidelineRequest model);
        Task<ObjectModelResponse> UpdateGuideline(Guid id, GuidelineRequest model);
        Task<ObjectModelResponse> DisableGuideline(Guid id);
    }

    public class GuidelineService : IGuidelineService
    {
        private readonly Database_UPODContext _context;
        public GuidelineService(Database_UPODContext context)
        {
            _context = context;
        }


        public async Task<ResponseModel<GuidelineResponse>> GetListGuidelines(PaginationRequest model)
        {
            var guidelines = await _context.Guidelines.Where(a => a.IsDelete == false).Select(a => new GuidelineResponse
            {
                id = a.Id,
                code = a.Code,
                service = new ServiceViewResponse
                {
                    id = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Id).FirstOrDefault(),
                    code = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                    service_name = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                    description = _context.Services.Where(x => x.Id.Equals(a.ServiceId)).Select(a => a.Description).FirstOrDefault(),
                },
                guideline_des = a.Guideline1,
                guideline_name = a.GuidelineName,
                is_delete = a.IsDelete,
                create_date = a.CreateDate,
                update_date = a.UpdateDate

            }).OrderByDescending(x => x.update_date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new ResponseModel<GuidelineResponse>(guidelines)
            {
                Total = guidelines.Count,
                Type = "Guidelines"
            };
        }
        private async Task<int> GetLastCode()
        {
            var guideline = await _context.Guidelines.OrderBy(x => x.Code).LastOrDefaultAsync();
            return CodeHelper.StringToInt(guideline!.Code!);
        }
        public async Task<ObjectModelResponse> CreateGuideline(GuidelineRequest model)
        {
            var guideline_id = Guid.NewGuid();
            while (true)
            {
                var guideline_dup = await _context.Guidelines.Where(x => x.Id.Equals(guideline_id)).FirstOrDefaultAsync();
                if (guideline_dup == null)
                {
                    break;
                }
                else
                {
                    guideline_id = Guid.NewGuid();
                }
            }
            var num = await GetLastCode();
            var code = CodeHelper.GeneratorCode("GL", num + 1);
            var guideline = new Guideline
            {
                Id = guideline_id,
                Code = code,
                ServiceId = model.service_id,
                GuidelineName = model.guideline_name,
                Guideline1 = model.guideline_des,
                IsDelete = false,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now

            };
            var data = new GuidelineResponse();

                await _context.Guidelines.AddAsync(guideline);
                var rs = await _context.SaveChangesAsync();
                if (rs > 0)
                {
                    data = new GuidelineResponse
                    {
                        id = guideline.Id,
                        code = guideline.Code,
                        service = new ServiceViewResponse
                        {
                            id = _context.Services.Where(x => x.Id.Equals(guideline.ServiceId)).Select(a => a.Id).FirstOrDefault(),
                            code = _context.Services.Where(x => x.Id.Equals(guideline.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                            service_name = _context.Services.Where(x => x.Id.Equals(guideline.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                            description = _context.Services.Where(x => x.Id.Equals(guideline.ServiceId)).Select(a => a.Description).FirstOrDefault(),
                        },
                        guideline_des = guideline.Guideline1,
                        guideline_name = guideline.GuidelineName,
                        is_delete = guideline.IsDelete,
                        create_date = guideline.CreateDate,
                        update_date = guideline.UpdateDate
                    };
                }

            

            return new ObjectModelResponse(data)
            {

                Type = "Guideline"
            };
        }


        public async Task<ObjectModelResponse> DisableGuideline(Guid id)
        {
            var guideline = await _context.Guidelines.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            guideline!.IsDelete = true;
            guideline.UpdateDate = DateTime.Now;
            var data = new GuidelineResponse();
            _context.Guidelines.Update(guideline);
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new GuidelineResponse
                {
                    id = guideline.Id,
                    code = guideline.Code,
                    service = new ServiceViewResponse
                    {
                        id = _context.Services.Where(x => x.Id.Equals(guideline.ServiceId)).Select(a => a.Id).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(guideline.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                        service_name = _context.Services.Where(x => x.Id.Equals(guideline.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                        description = _context.Services.Where(x => x.Id.Equals(guideline.ServiceId)).Select(a => a.Description).FirstOrDefault(),
                    },
                    guideline_des = guideline.Guideline1,
                    guideline_name = guideline.GuidelineName,
                    is_delete = guideline.IsDelete,
                    create_date = guideline.CreateDate,
                    update_date = guideline.UpdateDate
                };

            }
            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Guideline"
            };

        }
        public async Task<ObjectModelResponse> UpdateGuideline(Guid id, GuidelineRequest model)
        {
            var guideline = await _context.Guidelines.Where(a => a.Id.Equals(id)).Select(x => new Guideline
            {
                Id = id,
                Code = x.Code,
                ServiceId = model.service_id,
                GuidelineName = model.guideline_name,
                Guideline1 = model.guideline_des,
                IsDelete = x.IsDelete,
                CreateDate = x.CreateDate,
                UpdateDate = DateTime.Now
            }).FirstOrDefaultAsync();
            var data = new GuidelineResponse();

            _context.Guidelines.Update(guideline!);
            var rs = await _context.SaveChangesAsync();
            if (rs > 0)
            {
                data = new GuidelineResponse
                {
                    id = guideline!.Id,
                    code = guideline.Code,
                    service = new ServiceViewResponse
                    {
                        id = _context.Services.Where(x => x.Id.Equals(guideline.ServiceId)).Select(a => a.Id).FirstOrDefault(),
                        code = _context.Services.Where(x => x.Id.Equals(guideline.ServiceId)).Select(a => a.Code).FirstOrDefault(),
                        service_name = _context.Services.Where(x => x.Id.Equals(guideline.ServiceId)).Select(a => a.ServiceName).FirstOrDefault(),
                        description = _context.Services.Where(x => x.Id.Equals(guideline.ServiceId)).Select(a => a.Description).FirstOrDefault(),
                    },
                    guideline_des = guideline.Guideline1,
                    guideline_name = guideline.GuidelineName,
                    is_delete = guideline.IsDelete,
                    create_date = guideline.CreateDate,
                    update_date = guideline.UpdateDate
                };
            }

            return new ObjectModelResponse(data)
            {
                Status = 201,
                Type = "Guideline"
            };
        }

    }
}
