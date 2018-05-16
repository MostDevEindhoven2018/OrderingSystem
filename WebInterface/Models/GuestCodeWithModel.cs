using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    /// <summary>
    /// This class is used by the MenuCard Layout to maintain a session state by embedding
    /// the guestCode into the html. The layout does this automatically, but therefore
    /// requires a ViewModel of this type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
