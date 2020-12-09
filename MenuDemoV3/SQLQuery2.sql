SELECT d.name, d.Price, d.Description , al.name FROM Dishes as d 
INNER JOIN AllergensInDishes as ad ON d.Id=ad.DishId 
INNER JOIN Allergens as al ON al.Id = ad.Id where d.id = 2 