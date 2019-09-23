using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;
using EmployeeDataAccess;

namespace EmployeeService.Controllers
{
    [EnableCorsAttribute("*", "*", "*")]
    public class EmployeesController : ApiController
    {


        
        
        //[RequireHttps]
        [BasicAuthentication]

        public HttpResponseMessage Get(string gender = "All")
        {

            string username = Thread.CurrentPrincipal.Identity.Name;

            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                //switch(gender.ToLower())
                switch (username.ToLower())
                {
                    /*case "all":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());
                    */    

                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e=> e.Gender.ToLower() == "male").ToList());

                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender.ToLower() == "female").ToList());

                    default:
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                        //return Request.CreateErrorResponse(HttpStatusCode.BadRequest,"Value for gender must be All, Male or Female. " + 
                        //   gender + " is invalid.");


                }
                
            }
        }

        //public IEnumerable<Employee> Get() { 
        //[HttpGet]
        /*public IEnumerable<Employee> LoadlAllEmployees()
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                return entities.Employees.ToList();
            }
        }
        */
        /*public Employee Get(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                return entities.Employees.FirstOrDefault(e=> e.ID == id);
            }
        }
        */
        [HttpGet]
        public HttpResponseMessage /*Get*/ LoadEmployeeById(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var entity =  entities.Employees.FirstOrDefault(e => e.ID == id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,entity);
                }else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,"Employee with Id = " + id.ToString() + " not found");
                }

            }
        }

        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return message;
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        /*public void Delete(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                entities.Employees.Remove(entities.Employees.FirstOrDefault( e => e.ID == id));
                entities.SaveChanges();
            }
        }
        */
        public HttpResponseMessage Delete(int id)
        {
           try { 
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id = " + id.ToString() + " not found to delete");
                    }else
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);

                    }

                
                }
            }catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }


        }

        /*public void Put(int id, [FromBody] Employee employee)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                entity.FirstName = employee.FirstName;
                entity.LastName = employee.LastName;
                entity.Gender = employee.Gender;
                entity.Salary = employee.Salary;

                entities.SaveChanges();
            }
        }
        */

        //public HttpResponseMessage Put(int id, [FromBody] Employee employee)
        public HttpResponseMessage Put([FromBody]int id, [FromUri] Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id = " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.FirstName = employee.FirstName;
                        entity.LastName = employee.LastName;
                        entity.Gender = employee.Gender;
                        entity.Salary = employee.Salary;

                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, entity);

                    }


                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
