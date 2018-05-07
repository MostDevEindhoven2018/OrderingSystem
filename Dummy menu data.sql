--Sub categories
insert into SubDishTypes(SubType) values ('Cold Beverage') --1
insert into SubDishTypes(SubType) values ('Warm Beverage') --2
insert into SubDishTypes(SubType) values ('Alcohol Beverage') --3
insert into SubDishTypes(SubType) values ('Ice Creams') --4
insert into SubDishTypes(SubType) values ('Salads') --5
insert into SubDishTypes(SubType) values ('Pies')  --6
insert into SubDishTypes(SubType) values ('Sub-Mains') --7
insert into SubDishTypes(SubType) values ('Sub-Starters') --8

--Drinks demo
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (1,1,'Cola',2,'aaa')
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (1,1,'Fanta',2,'aaa')
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (1,2,'Tea',2,'aaa')
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (1,2,'Coffee',2,'aaa')
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (1,3,'Red Wine',2,'aaa')
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (1,3,'Beer',2,'aaa')

--Starters demo
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (2,8,'Carpachio',11,'aaa')
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (2,8,'Tuna Salad',11,'aaa')
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (2,8,'Shrimps',11,'aaa')
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (2,8,'Tomato Soup',11,'aaa')
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (2,8,'Onion Soup',11,'aaa')

--Mains demo
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (3,7,'Steak',10,'aaa')
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (3,7,'Salmon',10,'aaa')
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (3,7,'Chicken breast',10,'aaa')
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (3,7,'Duck',10,'aaa')
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (3,7,'Vega burger',10,'aaa')

--Dessert demo
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (4,4,'Vanilla Ice Cream',10,'aaa')
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (4,4,'Straberry Ice Cream',12,'aaa')
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (4,4,'Pear Ice Cream',15,'aaa')
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (4,5,'Banana split',52,'aaa')
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (4,5,'Fruit salad',52,'aaa')
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (4,6,'Apple pie',02,'aaa')
insert into DishTypes(Course,SubDishTypeID,Name,Price,Recipe) values (4,6,'Chocolate pie',5,'aaa')