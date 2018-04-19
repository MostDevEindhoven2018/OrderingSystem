using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class DishType
    {
        private static List<DishType> allDishes;

        public int DishTypeID { get; set; }
        public virtual CourseType Type { get; set; }
        public string Name { get; set; }

        public ICollection<Ingredient> DefaultIngredients { get; private set; }

        /// <summary>
        /// Returns all dishes known to the system
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DishType> GetAll()
        {
            if(allDishes == null)
            {
                List<DishType> list = new List<DishType>();

                list.Add(new DishType() { Type = CourseType.DRINK, Name = "Cola"});
                list.Add(new DishType() { Type = CourseType.DRINK, Name = "Fanta" });
                list.Add(new DishType() { Type = CourseType.DRINK, Name = "Wine" });
                list.Add(new DishType() { Type = CourseType.DRINK, Name = "Beer" });
                list.Add(new DishType() { Type = CourseType.DRINK, Name = "Coffee" });
                list.Add(new DishType() { Type = CourseType.DRINK, Name = "Tea" });

                list.Add(new DishType() { Type = CourseType.STARTER, Name = "Carpachio" });
                list.Add(new DishType() { Type = CourseType.STARTER, Name = "Tuna Salad" });
                list.Add(new DishType() { Type = CourseType.STARTER, Name = "Shrimps" });
                list.Add(new DishType() { Type = CourseType.STARTER, Name = "Tomato Soup" });
                list.Add(new DishType() { Type = CourseType.STARTER, Name = "Onion Soup" });

                list.Add(new DishType() { Type = CourseType.MAINCOURSE, Name = "Steak" });
                list.Add(new DishType() { Type = CourseType.MAINCOURSE, Name = "Salmon" });
                list.Add(new DishType() { Type = CourseType.MAINCOURSE, Name = "Chicken breast" });
                list.Add(new DishType() { Type = CourseType.MAINCOURSE, Name = "Duck" });
                list.Add(new DishType() { Type = CourseType.MAINCOURSE, Name = "Vega burger" });

                list.Add(new DishType() { Type = CourseType.DESSERT, Name = "Vanilla icecream" });
                list.Add(new DishType() { Type = CourseType.DESSERT, Name = "Banana split" });
                list.Add(new DishType() { Type = CourseType.DESSERT, Name = "Apple pie with wipped cream" });
                list.Add(new DishType() { Type = CourseType.DESSERT, Name = "Fruit salad" });

                allDishes = list;
            }
            return allDishes;
        }

        /// <summary>
        /// Returns all DishTypes with DRINK as CourseType
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DishType> getAllDrinks()
        {
            return GetAll().Where((dt) => { return (dt.Type == CourseType.DRINK); });
        }

        /// <summary>
        /// Returns all DishTypes with STARTER as CourseType
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DishType> getAllStarters()
        {
            return GetAll().Where((dt) => { return (dt.Type == CourseType.STARTER); });
        }

        /// <summary>
        /// Returns all DishTypes with MAINCOURSE as CourseType
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DishType> getAllMains()
        {
            return GetAll().Where((dt) => { return (dt.Type == CourseType.MAINCOURSE); });
        }

        /// <summary>
        /// Returns all DishTypes with DESSERT as CourseType
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DishType> getAllDesserts()
        {
            return GetAll().Where((dt) => { return (dt.Type == CourseType.DESSERT); });
        }
    }

    public enum CourseType
    {
        INVALID,
        DRINK,
        STARTER,
        MAINCOURSE,
        DESSERT
    }
}
