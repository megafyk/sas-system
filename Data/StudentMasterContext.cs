using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Data
{
    public class StudentMasterContext
    {
        /// <summary>
        /// Get all students in DB
        /// </summary>
        /// <returns>List of all Students</returns>
        public List<Student_Master> GetAll()
        {
            using (var ctx = new SASDBEntities())
            {
                return ctx.Student_Master.ToList();
            }
        }
        /// <summary>
        /// Get all grades in DB
        /// </summary>
        /// <returns>List of all Grades</returns>
        public List<string> GetAllGrades()
        {
            using (var ctx = new SASDBEntities())
            {
                return ctx.Student_Master.Select(g => g.GRADE).Distinct().ToList();
            }
        }
        /// <summary>
        /// Get all classes in DB
        /// </summary>
        /// <returns>List of all Classes</returns>
        public List<string> GetAllClasses()
        {
            using (var ctx = new SASDBEntities())
            {
                return ctx.Student_Master.Select(c => c.CLASS).Distinct().ToList();
            }
        }
        /// <summary>
        /// Find students by lambda condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>List of student which satisfy condition</returns>
        public List<Student_Master> FindBy(Expression<Func<Student_Master, bool>> predicate)
        {
            using (var ctx = new SASDBEntities())
            {
                return ctx.Student_Master.Where(predicate).ToList();
            }
        }
    }
}
