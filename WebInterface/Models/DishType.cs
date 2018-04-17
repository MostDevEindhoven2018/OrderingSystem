using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class DishType
    {
        private static List<DishType> allDishes;
        public MainType Type { get; private set; }
        public string Name { get; private set; }

        /// <summary>
        /// Returns all dishes known to the system
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DishType> GetAll()
        {
            if(allDishes == null)
            {
                List<DishType> l = new List<DishType>();

                l.Add(new DishType() { Type = MainType.DRINK, Name = "Cola"});
                l.Add(new DishType() { Type = MainType.DRINK, Name = "Fanta" });
                l.Add(new DishType() { Type = MainType.DRINK, Name = "Wine" });
                l.Add(new DishType() { Type = MainType.DRINK, Name = "Beer" });
                l.Add(new DishType() { Type = MainType.DRINK, Name = "Coffee" });
                l.Add(new DishType() { Type = MainType.DRINK, Name = "Tea" });

                l.Add(new DishType() { Type = MainType.STARTER, Name = "Carpachio" });
                l.Add(new DishType() { Type = MainType.STARTER, Name = "Tuna Salad" });
                l.Add(new DishType() { Type = MainType.STARTER, Name = "Shrimps" });
                l.Add(new DishType() { Type = MainType.STARTER, Name = "Tomato Soup" });
                l.Add(new DishType() { Type = MainType.STARTER, Name = "Onion Soup" });

                l.Add(new DishType() { Type = MainType.MAINCOURSE, Name = "Steak" });
                l.Add(new DishType() { Type = MainType.MAINCOURSE, Name = "Salmon" });
                l.Add(new DishType() { Type = MainType.MAINCOURSE, Name = "Chicken breast" });
                l.Add(new DishType() { Type = MainType.MAINCOURSE, Name = "Duck" });
                l.Add(new DishType() { Type = MainType.MAINCOURSE, Name = "Vega burger" });

                l.Add(new DishType() { Type = MainType.DESSERT, Name = "Vanilla icecream" });
                l.Add(new DishType() { Type = MainType.DESSERT, Name = "Banana split" });
                l.Add(new DishType() { Type = MainType.DESSERT, Name = "Apple pie with wipped cream" });
                l.Add(new DishType() { Type = MainType.DESSERT, Name = "Fruit salad" });

                allDishes = l;
            }
            return allDishes;
        }

        /// <summary>
        /// Returns all DishTypes with DRINK as MainType
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DishType> getAllDrinks()
        {
            return GetAll().Where((dt) => { return (dt.Type == MainType.DRINK); });
        }

        /// <summary>
        /// Returns all DishTypes with STARTER as MainType
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DishType> getAllStarters()
        {
            return GetAll().Where((dt) => { return (dt.Type == MainType.STARTER); });
        }

        /// <summary>
        /// Returns all DishTypes with MAINCOURSE as MainType
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DishType> getAllMains()
        {
            return GetAll().Where((dt) => { return (dt.Type == MainType.MAINCOURSE); });
        }

        /// <summary>
        /// Returns all DishTypes with DESSERT as MainType
        /// </summary>
        /// <returns></returns>
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
