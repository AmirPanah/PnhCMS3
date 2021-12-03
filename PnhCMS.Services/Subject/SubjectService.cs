using PnhCMS.Services;
using PnhCMS.Repository.Common;
using PnhCMS.Repository;
using PnhCMS.Services.Subject.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnhCMS.Services.Subject
{
    public class SubjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PnhCMS.Repository.AppContext _appContext;
        public SubjectService(IUnitOfWork unitOfWork, PnhCMS.Repository.AppContext appContext)
        {
            _unitOfWork = unitOfWork;
            _appContext = appContext;
        }

        public async Task<List<SubjectVM>> GetAll()
        {
           

            var list = await _unitOfWork.Repository<PnhCMS.Repository.AppModel.Subject>().GetAll().Include(x => x.Courses).Where(x => x.Deleted == false)
                   .Select(x => new SubjectVM
                   {
                       SubjectId = x.SubjectId,
                       SubjectCode = x.SubjectCode,
                       SubjectName = x.SubjectName,
                       SubjectCredit = x.SubjectCredit,
                       CourseId = x.CourseId,
                       CourseName = x.Courses.CourseName,
                   }).OrderByDescending(x => x.SubjectId).ToListAsync();

            return list;
        }

        public async Task<List<SubjectVM>> GetSubjectById(string searchBy)
        {
            var list = _unitOfWork.Repository<PnhCMS.Repository.AppModel.Subject>().GetAll().Include(x => x.Courses)
                  .Select(x => new SubjectVM
                  {
                      SubjectId = x.SubjectId,
                      SubjectCode = x.SubjectCode,
                      SubjectName = x.SubjectName,
                      SubjectCredit = x.SubjectCredit,
                      CourseId = x.CourseId,
                      CourseName = x.Courses.CourseName,
                  }).OrderByDescending(x => x.SubjectId).AsQueryable();

            if (!string.IsNullOrEmpty(searchBy))
            {
                list = list.Where(x => x.SubjectName.ToLower().Contains(searchBy.ToLower()) || x.SubjectCode.ToLower().Contains(searchBy.ToLower()) || x.CourseName.ToLower().Contains(searchBy.ToLower())).AsQueryable();
            }
            return await list.ToListAsync();
        }

        public List<SubjectVM> GetSubjectByCourseId(int id)
        {
            var list = _unitOfWork.Repository<PnhCMS.Repository.AppModel.Subject>().GetAll()
                   .Where(x => x.Deleted == false && x.CourseId == id)
                   .Select(x => new SubjectVM
                   {
                       SubjectId = x.SubjectId,
                       SubjectName = x.SubjectName
                   }).OrderBy(x=>x.SubjectName).ToList();

            return list;
        }


        public async Task<List<SubjectListVM>> GetSubjectDetails()
        {
           



            var list = await (from c in _appContext.Subjects
                              join te in _appContext.TeacherEnrollments on c.SubjectId equals te.SubjectId
                              join se in _appContext.StudentEnrollments on c.SubjectId equals se.SubjectId
                              join ge in _appContext.Grades on se.GradeId equals ge.GradeId
                              join tee in _appContext.Teachers on te.TeacherId equals tee.TeacherId

                              where c.Deleted == false
                              group new { c, te, se, ge , tee} by c.SubjectId into cts
                              select new SubjectListVM
                              {
                                  SubjectId = cts.Key,
                                  SubjectName = cts.Select(x => x.c.SubjectName).FirstOrDefault(),
                                  SubjectCode = cts.Select(x => x.c.SubjectCode).FirstOrDefault(),
                                    TeacherName = cts.Select(x => x.tee.TeacherName).FirstOrDefault(),
                                  NoOfStudent = cts.Select(x => x.se.StudentId).Distinct().Count(),
                                  AvgOfGrades = cts.Select(x => x.ge.GradePoint).Average(),
                              }).ToListAsync();

            return list;



        }


        public async Task<PnhCMS.Repository.AppModel.Subject> GetById(int id)
        {
            var entity = await _unitOfWork.Repository<PnhCMS.Repository.AppModel.Subject>().FindAsync(x => x.SubjectId == id);
            return entity;
        }
        public async Task<SubjectVM> GetSubjectById(int id)
        {
            var entity = await _unitOfWork.Repository<PnhCMS.Repository.AppModel.Subject>().FindAsync(x => x.SubjectId == id);
            if (entity != null)
            {
                var data = new SubjectVM
                {
                    SubjectId = entity.SubjectId,
                    SubjectCode = entity.SubjectCode,
                    SubjectName = entity.SubjectName,
                    SubjectCredit =entity.SubjectCredit,
                    CourseId = entity.CourseId
                };
                return data;
            }
            else
            {
                var data = new SubjectVM();
                return data;
            }


        }

        public PnhCMS.Repository.AppModel.Subject GetByName(string name)
        {
            return _unitOfWork.Repository<PnhCMS.Repository.AppModel.Subject>().GetAll().Where(x => x.SubjectName.Trim().Equals(name.Trim())).SingleOrDefault();
        }
        public PnhCMS.Repository.AppModel.Subject GetByCode(string name)
        {
            return _unitOfWork.Repository<PnhCMS.Repository.AppModel.Subject>().GetAll().Where(x => x.SubjectCode.Trim().Equals(name.Trim())).SingleOrDefault();
        }

        public async Task<ResultModel> Insert(SubjectVM model)
        {
            try
            {
                if (GetByName(model.SubjectName) != null)
                    return new ResultModel { Result = false, Message = NotificationMessageConfig.ExistMessage(model.SubjectName) };
                if (GetByName(model.SubjectCode) != null)
                    return new ResultModel { Result = false, Message = NotificationMessageConfig.ExistMessage(model.SubjectCode) };

                //var entity = new PnhCMS.Repository.AppModel.Subject();
                //Mapper.Map(entity, model);

                var entity = new PnhCMS.Repository.AppModel.Subject
                {
                    SubjectCode = model.SubjectCode,
                    SubjectName = model.SubjectName,
                    CourseId = model.CourseId,
                    SubjectCredit = model.SubjectCredit,
                    CreateDate = DateTime.Now
                };

                await _unitOfWork.Repository<PnhCMS.Repository.AppModel.Subject>().InsertAsync(entity);

                return new ResultModel { Id = entity.SubjectId.ToString(), Result = true, Message = NotificationMessageConfig.InsertSuccessMessage(model.SubjectName) };
            }
            catch (Exception)
            {
                return new ResultModel { Result = false, Message = NotificationMessageConfig.InsertErrorMessage(model.SubjectName) };
            }
        }

        public async Task<ResultModel> Update(SubjectVM model)
        {
            try
            {
                var entity = await GetById(model.SubjectId);
                entity.SubjectName = model.SubjectName;
                entity.SubjectCode = model.SubjectCode;
                entity.CourseId = model.CourseId;
                entity.SubjectCredit = model.SubjectCredit;
                entity.UpdateDate = DateTime.Now;
                await _unitOfWork.CommitAsync();

                return new ResultModel { Id = entity.SubjectId.ToString(), Result = true, Message = NotificationMessageConfig.UpdateSuccessMessage(model.SubjectName) };
            }
            catch (Exception)
            {
                return new ResultModel { Id = model.SubjectId.ToString(), Result = false, Message = NotificationMessageConfig.UpdateErrorMessage(model.SubjectName) };
            }
        }

        public async Task<ResultModel> Delete(int id)
        {
            var model = await GetById(id);
            try
            {
                if (model != null)
                {
                    model.Deleted = true;
                    model.DeleteDate = DateTime.Now;

                    _unitOfWork.Commit();
                    return new ResultModel { Result = true, Message = NotificationMessageConfig.DeleteSuccessMessage(model.SubjectName), Id = model.SubjectId.ToString() };
                }
                else
                {
                    return new ResultModel { Result = false, Message = NotificationMessageConfig.NotFoundError };
                }

            }
            catch (Exception)
            {
                return new ResultModel { Result = false, Message = NotificationMessageConfig.DeleteErrorMessage(model.SubjectName) };
            }
        }

    }
}
