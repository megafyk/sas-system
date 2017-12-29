using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class CoporationMasterContext
    {
        /// <summary>
        /// Get all coporation in Db
        /// </summary>
        /// <returns>List of coporations</returns>
        public List<Coporation_Master> GetAll()
        {
            using (var ctx = new SASDBEntities())
            {
                return ctx.Coporation_Master.ToList();
            }
        }
    }
}
