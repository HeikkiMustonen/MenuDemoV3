CREATE TABLE [dbo].[RestaurantMenus]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RestaurantId] INT NOT NULL, 
    [MenuId] INT NOT NULL, 
    CONSTRAINT [FK_RestaurantMenus_Restaurants] FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurants]([Id]), 
    CONSTRAINT [FK_RestaurantMenus_Menus] FOREIGN KEY ([MenuId]) REFERENCES [Menus]([Id])
)
