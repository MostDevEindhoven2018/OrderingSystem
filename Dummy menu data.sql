--Sub categories
insert into SubDishTypes(SubType) values ('Cold Beverage') --1
insert into SubDishTypes(SubType) values ('Warm Beverage') --2
insert into SubDishTypes(SubType) values ('Alcohol Beverage') --3
insert into SubDishTypes(SubType) values ('Ice Creams') --4
insert into SubDishTypes(SubType) values ('Salads') --5
insert into SubDishTypes(SubType) values ('Pies')  --6

--Drinks demo
insert into DishTypes(Course,SubDishTypeID,Name) values (1,1,'Cola')
insert into DishTypes(Course,SubDishTypeID,Name) values (1,1,'Fanta')
insert into DishTypes(Course,SubDishTypeID,Name) values (1,2,'Tea')
insert into DishTypes(Course,SubDishTypeID,Name) values (1,2,'Coffee')
insert into DishTypes(Course,SubDishTypeID,Name) values (1,3,'Red Wine')
insert into DishTypes(Course,SubDishTypeID,Name) values (1,3,'Beer')

--Starters demo
insert into DishTypes(Course,Name) values (2,'Carpachio')
insert into DishTypes(Course,Name) values (2,'Tuna Salad')
insert into DishTypes(Course,Name) values (2,'Shrimps')
insert into DishTypes(Course,Name) values (2,'Tomato Soup')
insert into DishTypes(Course,Name) values (2,'Onion Soup')

--Mains demo
insert into DishTypes(Course,Name) values (3,'Steak')
insert into DishTypes(Course,Name) values (3,'Salmon')
insert into DishTypes(Course,Name) values (3,'Chicken breast')
insert into DishTypes(Course,Name) values (3,'Duck')
insert into DishTypes(Course,Name) values (3,'Vega burger')

--Dessert demo
insert into DishTypes(Course,SubDishTypeID,Name) values (4,4,'Vanilla Ice Cream')
insert into DishTypes(Course,SubDishTypeID,Name) values (4,4,'Straberry Ice Cream')
insert into DishTypes(Course,SubDishTypeID,Name) values (4,4,'Pear Ice Cream')
insert into DishTypes(Course,SubDishTypeID,Name) values (4,5,'Banana split')
insert into DishTypes(Course,SubDishTypeID,Name) values (4,5,'Fruit salad')
insert into DishTypes(Course,SubDishTypeID,Name) values (4,6,'Apple pie')
insert into DishTypes(Course,SubDishTypeID,Name) values (4,6,'Chocolate pie')