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
        public virtual CourseType Course { get; set; }
        public string Name { get; set; }
        public SubDishType SubType { get; set; }
        public int Price { get; set; }

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

                list.Add(new DishType() { Course = CourseType.Drink, SubType = new SubDishType { SubType = "ColdBeverage" }, Name = "Cola"});
                list.Add(new DishType() { Course = CourseType.Drink, SubType = new SubDishType { SubType = "ColdBeverage" }, Name = "Fanta" });
                list.Add(new DishType() { Course = CourseType.Drink, SubType = new SubDishType { SubType = "HardDrink" }, Name = "Wine" });
                list.Add(new DishType() { Course = CourseType.Drink, SubType = new SubDishType { SubType = "HardDrink" }, Name = "Beer" });
                list.Add(new DishType() { Course = CourseType.Drink, SubType = new SubDishType { SubType = "HotBeverage" }, Name = "Coffee" });
                list.Add(new DishType() { Course = CourseType.Drink, SubType = new SubDishType { SubType = "HotBeverage" }, Name = "Tea" });

                list.Add(new DishType() { Course = CourseType.Starter, Name = "Carpachio" });
                list.Add(new DishType() { Course = CourseType.Starter, Name = "Tuna Salad" });
                list.Add(new DishType() { Course = CourseType.Starter, Name = "Shrimps" });
                list.Add(new DishType() { Course = CourseType.Starter, Name = "Tomato Soup" });
                list.Add(new DishType() { Course = CourseType.Starter, Name = "Onion Soup" });

                list.Add(new DishType() { Course = CourseType.MainCourse, Name = "Steak" });
                list.Add(new DishType() { Course = CourseType.MainCourse, Name = "Salmon" });
                list.Add(new DishType() { Course = CourseType.MainCourse, Name = "Chicken breast" });
                list.Add(new DishType() { Course = CourseType.MainCourse, Name = "Duck" });
                list.Add(new DishType() { Course = CourseType.MainCourse, Name = "Vega burger" });

                list.Add(new DishType() { Course = CourseType.Dessert, SubType = new SubDishType { SubType = "IceCreams" }, Name = "Vanilla Ice Cream" });
                list.Add(new DishType() { Course = CourseType.Dessert, SubType = new SubDishType { SubType = "Salads" }, Name = "Banana split" });
                list.Add(new DishType() { Course = CourseType.Dessert, SubType = new SubDishType { SubType = "Pies" }, Name = "Apple pie with whipped cream" });
                list.Add(new DishType() { Course = CourseType.Dessert, SubType = new SubDishType { SubType = "Salads" }, Name = "Fruit salad" });

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
            return GetAll().Where((dt) => { return (dt.Course == CourseType.Drink); });
        }

        public static IEnumerable<DishType> getAllHotBeverages()
        {
            return GetAll().Where((dt) => { return (dt.Course == CourseType.Drink && dt.SubType.SubType == "HotBeverage"); });
        }

        public static IEnumerable<DishType> getAllColdBeverages()
        {
            return GetAll().Where((dt) => { return (dt.Course == CourseType.Drink && dt.SubType.SubType == "ColdBeverage"); });
        }

        public static IEnumerable<DishType> getAllHardDrinks()
        {
            return GetAll().Where((dt) => { return (dt.Course == CourseType.Drink && dt.SubType.SubType == "HardDrink"); });
        }

        /// <summary>
        /// Returns all DishTypes with STARTER as CourseType
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DishType> getAllStarters()
        {
            return GetAll().Where((dt) => { return (dt.Course == CourseType.Starter); });
        }

        /// <summary>
        /// Returns all DishTypes with MAINCOURSE as CourseType
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DishType> getAllMains()
        {
            return GetAll().Where((dt) => { return (dt.Course == CourseType.MainCourse); });
        }

        /// <summary>
        /// Returns all DishTypes with DESSERT as CourseType
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DishType> getAllDesserts()
        {
            return GetAll().Where((dt) => { return (dt.Course == CourseType.Dessert); });
        }
        public static IEnumerable<DishType> getAllPies()
        {
            return GetAll().Where((dt) => { return (dt.Course == CourseType.Dessert && dt.SubType.SubType == "Pies"); });
        }
        public static IEnumerable<DishType> getAllIceCreams()
        {
            return GetAll().Where((dt) => { return (dt.Course == CourseType.Dessert && dt.SubType.SubType == "IceCreams"); });
        }
        public static IEnumerable<DishType> getAllSalads()
        {
            return GetAll().Where((dt) => { return (dt.Course == CourseType.Dessert && dt.SubType.SubType == "Salads"); });
        }

    }

    public enum CourseType
    {
        Unavailable,
        Drink,
        Starter,
        MainCourse,
        Dessert
    }

    public enum DrinkType
    {
        ColdBeverage,
        HotBeverage,
        SoftDrink
    }
}
