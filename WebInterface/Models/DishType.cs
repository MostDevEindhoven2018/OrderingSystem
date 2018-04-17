using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class DishType
    {
        private static List<DishType> allDishes;
        public CourseType Type { get; private set; }
        public string Name { get; private set; }
        public List<Ingredient> DefaultIngredients { get; private set; } = new List<Ingredient>();

        /// <summary>
        /// Returns all dishes known to the system
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DishType> GetAll()
        {
            if(allDishes == null)
            {
                List<DishType> l = new List<DishType>();

                l.Add(new DishType() { Type = CourseType.DRINK, Name = "Cola"});
                l.Add(new DishType() { Type = CourseType.DRINK, Name = "Fanta" });
                l.Add(new DishType() { Type = CourseType.DRINK, Name = "Wine" });
                l.Add(new DishType() { Type = CourseType.DRINK, Name = "Beer" });
                l.Add(new DishType() { Type = CourseType.DRINK, Name = "Coffee" });
                l.Add(new DishType() { Type = CourseType.DRINK, Name = "Tea" });

                l.Add(new DishType() { Type = CourseType.STARTER, Name = "Carpachio" });
                l.Add(new DishType() { Type = CourseType.STARTER, Name = "Tuna Salad" });
                l.Add(new DishType() { Type = CourseType.STARTER, Name = "Shrimps" });
                l.Add(new DishType() { Type = CourseType.STARTER, Name = "Tomato Soup" });
                l.Add(new DishType() { Type = CourseType.STARTER, Name = "Onion Soup" });

                l.Add(new DishType() { Type = CourseType.MAINCOURSE, Name = "Steak" });
                l.Add(new DishType() { Type = CourseType.MAINCOURSE, Name = "Salmon" });
                l.Add(new DishType() { Type = CourseType.MAINCOURSE, Name = "Chicken breast" });
                l.Add(new DishType() { Type = CourseType.MAINCOURSE, Name = "Duck" });
                l.Add(new DishType() { Type = CourseType.MAINCOURSE, Name = "Vega burger" });

                l.Add(new DishType() { Type = CourseType.DESSERT, Name = "Vanilla icecream" });
                l.Add(new DishType() { Type = CourseType.DESSERT, Name = "Banana split" });
                l.Add(new DishType() { Type = CourseType.DESSERT, Name = "Apple pie with wipped cream" });
                l.Add(new DishType() { Type = CourseType.DESSERT, Name = "Fruit salad" });

                allDishes = l;
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
