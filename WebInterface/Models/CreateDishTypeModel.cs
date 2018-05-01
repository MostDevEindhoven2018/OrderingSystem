using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class CreateDishTypeModel
    {
        public virtual CourseType Course { get; set; }
        public string Name { get; set; }
        public int SubTypeID { get; set; }
        public List<int> SubTypeList { get; set; }
    }
}
