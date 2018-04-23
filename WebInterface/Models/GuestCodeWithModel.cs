using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public struct GuestCodeWithModel<T>
    {
        public T Model;
        public string GuestCode;

        public GuestCodeWithModel(T model, string guestCode)
        {
            Model = model;
            GuestCode = guestCode;
        }
    }
}
