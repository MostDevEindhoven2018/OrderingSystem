using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class DishType
    {
        public MainType Type { get; private set; }
        /// <summary>
        /// Returns all dishes known to the system
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DishType> GetAll()
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<DishType> getAllDrinks()
        {
            return GetAll().Where((dt) => { return (dt.Type == MainType.DRINK); });
        }

        public static IEnumerable<DishType> getAllStarters()
        {
            return GetAll().Where((dt) => { return (dt.Type == MainType.STARTER); });
        }

        public static IEnumerable<DishType> getAllMains()
        {
            return GetAll().Where((dt) => { return (dt.Type == MainType.MAINCOURSE); });
        }

        public static IEnumerable<DishType> getAllDesserts()
        {
            return GetAll().Where((dt) => { return (dt.Type == MainType.DESSERT); });
        }
    }

    public enum MainType
    {
        INVALID,
        DRINK,
        STARTER,
        MAINCOURSE,
        DESSERT
    }
}
