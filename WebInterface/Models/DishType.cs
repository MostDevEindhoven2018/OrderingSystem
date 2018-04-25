using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Models
{
    public class DishType : SubDishType
    {
        private static List<DishType> allDishes;

        public int DishTypeID { get; set; }
        public virtual CourseType Course { get; set; }
        public string Name { get; set; }

        public ICollection<Ingredient> DefaultIngredients { get; /*private*/ set; }

        /// <summary>
        /// Returns all dishes known to the system
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DishType> GetAll()
        {
            if(allDishes == null)
            {
                List<DishType> list = new List<DishType>();

                list.Add(new DishType() { Course = CourseType.DRINK, SubType = "ColdBeverage", Name = "Cola"});
                list.Add(new DishType() { Course = CourseType.DRINK, SubType = "ColdBeverage", Name = "Fanta" });
                list.Add(new DishType() { Course = CourseType.DRINK, SubType = "HardDrink", Name = "Wine" });
                list.Add(new DishType() { Course = CourseType.DRINK, SubType = "HardDrink", Name = "Beer" });
                list.Add(new DishType() { Course = CourseType.DRINK, SubType = "HotBeverage", Name = "Coffee" });
                list.Add(new DishType() { Course = CourseType.DRINK, SubType = "HotBeverage", Name = "Tea" });

                list.Add(new DishType() { Course = CourseType.STARTER, Name = "Carpachio" });
                list.Add(new DishType() { Course = CourseType.STARTER, Name = "Tuna Salad" });
                list.Add(new DishType() { Course = CourseType.STARTER, Name = "Shrimps" });
                list.Add(new DishType() { Course = CourseType.STARTER, Name = "Tomato Soup" });
                list.Add(new DishType() { Course = CourseType.STARTER, Name = "Onion Soup" });

                list.Add(new DishType() { Course = CourseType.MAINCOURSE, Name = "Steak" });
                list.Add(new DishType() { Course = CourseType.MAINCOURSE, Name = "Salmon" });
                list.Add(new DishType() { Course = CourseType.MAINCOURSE, Name = "Chicken breast" });
                list.Add(new DishType() { Course = CourseType.MAINCOURSE, Name = "Duck" });
                list.Add(new DishType() { Course = CourseType.MAINCOURSE, Name = "Vega burger" });

                list.Add(new DishType() { Course = CourseType.DESSERT, SubType = "IceCreams", Name = "Vanilla Ice Cream" });
                list.Add(new DishType() { Course = CourseType.DESSERT, SubType = "Salads", Name = "Banana split" });
                list.Add(new DishType() { Course = CourseType.DESSERT, SubType = "Pies", Name = "Apple pie with whipped cream" });
                list.Add(new DishType() { Course = CourseType.DESSERT, SubType = "Salads", Name = "Fruit salad" });

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
            return GetAll().Where((dt) => { return (dt.Course == CourseType.DRINK); });
        }

        public static IEnumerable<DishType> getAllHotBeverages()
        {
            return GetAll().Where((dt) => { return (dt.Type == CourseType.DRINK && dt.SubType == "HotBeverage"); });
        }

        public static IEnumerable<DishType> getAllColdBeverages()
        {
            return GetAll().Where((dt) => { return (dt.Type == CourseType.DRINK && dt.SubType == "ColdBeverage"); });
        }

        public static IEnumerable<DishType> getAllHardDrinks()
        {
            return GetAll().Where((dt) => { return (dt.Type == CourseType.DRINK && dt.SubType == "HardDrink"); });
        }

        /// <summary>
        /// Returns all DishTypes with STARTER as CourseType
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DishType> getAllStarters()
        {
            return GetAll().Where((dt) => { return (dt.Course == CourseType.STARTER); });
        }

        /// <summary>
        /// Returns all DishTypes with MAINCOURSE as CourseType
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DishType> getAllMains()
        {
            return GetAll().Where((dt) => { return (dt.Course == CourseType.MAINCOURSE); });
        }

        /// <summary>
        /// Returns all DishTypes with DESSERT as CourseType
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DishType> getAllDesserts()
        {
            return GetAll().Where((dt) => { return (dt.Course == CourseType.DESSERT); });
        }
        public static IEnumerable<DishType> getAllPies()
        {
            return GetAll().Where((dt) => { return (dt.Type == CourseType.DESSERT && dt.SubType == "Pies"); });
        }
        public static IEnumerable<DishType> getAllIceCreams()
        {
            return GetAll().Where((dt) => { return (dt.Type == CourseType.DESSERT && dt.SubType == "IceCreams"); });
        }
        public static IEnumerable<DishType> getAllSalads()
        {
            return GetAll().Where((dt) => { return (dt.Type == CourseType.DESSERT && dt.SubType == "Salads"); });
        }

    }

    public enum CourseType
    {
        UNAVAILABLE,
        DRINK,
        STARTER,
        MAINCOURSE,
        DESSERT
    }

    public enum DrinkType
    {
        ColdBeverage,
        HotBeverage,
        SoftDrink
    }
}
