CREATE TABLE [dbo].[Dishes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [Description ] VARCHAR(400) NULL , 
    [Price] DECIMAL(18, 2) NULL DEFAULT 0, 
    [DishType] INT NULL, 
    CONSTRAINT [FK_Dish_ToDishType] FOREIGN KEY ([DishType]) REFERENCES [DishType]([id])
)
