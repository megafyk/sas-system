using Data;
using SAS.Web.Infrastructure.Core;
using SAS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SAS.Web.Controllers
{
    [RoutePrefix("api/students")]
    public class StudentController : ApiControllerBase
    {   
        /// <summary>
        /// Get all Grades in db
        /// </summary>
        /// <param name="request">client's request header</param>
        /// <returns>json data include grades</returns>
        [HttpGet]
        [Route("grades")]
        public HttpResponseMessage GetGradeFilter(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                try
                {
                    StudentMasterContext smc = new StudentMasterContext();
                    List<string> grades = smc.GetAllGrades();
                    response = request.CreateResponse(HttpStatusCode.OK, grades);
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });
                }
                return response;
            });
        }
        /// <summary>
        /// Get all Classes
        /// </summary>
        /// <param name="request">client's request header</param>
        /// <returns>json data include classes</returns>
        [HttpGet]
        [Route("classes")]
        public HttpResponseMessage GetClassFilter(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                try
                {
                    StudentMasterContext smc = new StudentMasterContext();
                    List<string> classes = smc.GetAllClasses();
                    response = request.CreateResponse(HttpStatusCode.OK, classes);
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });
                }
                return response;
            });
        }
        /// <summary>
        /// Get students by page in db
        /// </summary>
        /// <param name="request">client's request header</param>
        /// <param name="page">page's number</param>
        /// <param name="pageSize">page's size</param>
        /// <param name="filter">filter data</param>
        /// <param name="type">type of filter</param>
        /// <returns>json data include students by page</returns>
        [HttpGet]
        [Route("{page:int=0}/{pageSize=7}/{filter?}/{type?}")]
        public HttpResponseMessage GetStudents(HttpRequestMessage request, int? page, int? pageSize, string filter=null, string type = null)
        {
            int currentPage = (page == null) ? 0 : page.Value;
            int currentPageSize = pageSize.Value;

            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                StudentMasterContext smc = new StudentMasterContext();
                List<Student_Master> students = null;
                List<StudentViewModel> studentsVM = new List<StudentViewModel>();
                int totalRecords = new int();
                try
                {
                    if (!string.IsNullOrEmpty(type))
                    {
                        if (type.Equals("grade"))
                        {
                            students = smc
                                .FindBy(s => s.GRADE.ToLower().Contains(filter.ToLower().Trim()))
                                .ToList();
                            totalRecords = smc
                                .FindBy(s => s.GRADE.ToLower().Contains(filter.ToLower().Trim())).Count;
                        }
                        else if (type.Equals("class"))
                        {
                            students = smc
                                .FindBy(s => s.CLASS.ToLower().Contains(filter.ToLower().Trim()))
                                .ToList();
                            totalRecords = smc
                                .FindBy(s => s.CLASS.ToLower().Contains(filter.ToLower().Trim())).Count;
                        }
                    }
                    else
                    {
                        students = smc
                            .GetAll()
                            .Skip(currentPage * currentPageSize)
                            .Take(currentPageSize)
                            .ToList();
                        totalRecords = smc.GetAll().Count;
                    }
                    foreach (Student_Master s_m in students)
                    {
                        studentsVM.Add(new StudentViewModel()
                        {
                            CoporationCode = s_m.COPORATION_CODE,
                            StudentId = s_m.STUDENT_ID,
                            Name = s_m.NAME,
                            Grade = s_m.GRADE,
                            Class = s_m.CLASS,
                            Status = s_m.STATUS,
                            UserId = s_m.USER_ID
                        });
                    }
                    PaginationSet<StudentViewModel> pagedSet = new PaginationSet<StudentViewModel>()
                    {
                        Page = currentPage,
                        TotalCount = totalRecords,
                        TotalPages = (int)Math.Ceiling((decimal)totalRecords / currentPageSize),
                        Items = studentsVM
                    };
                    response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });
                }
                return response;
            });
        }
    }
}
