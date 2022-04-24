using Microsoft.EntityFrameworkCore;
using MinimalShoppingListAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseInMemoryDatabase("ShoppingListApi"));

var app = builder.Build();

//This is the method that we can use to create and end point for the HTTP type get
app.MapGet("/shoppinglist", async (ApiDbContext db) => await db.Groceries.ToListAsync());

//Get by Id
app.MapGet("/shoppinglist/{id}", async (int id, ApiDbContext db) =>
{
    var grocery = db.Groceries.FindAsync(id);
    return grocery != null ? Results.Ok(grocery) : Results.NotFound();
});

//Delete
app.MapDelete("/shoppinglist/{id}", async (int id, ApiDbContext db) =>
{
    var grocery = await db.Groceries.FindAsync(id);

    if (grocery != null)
    {
        db.Groceries.Remove(grocery);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    //else
    return Results.NotFound();
});

//Update
//Put is for update an existing item
app.MapPut("/shoppinglist/{id}", async (int id, Grocery grocery, ApiDbContext db) =>
{
    var groceryInDb = await db.Groceries.FindAsync(id);

    if (groceryInDb != null)
    {
        groceryInDb.Name = grocery.Name;
        groceryInDb.Purchased = grocery.Purchased;

        await db.SaveChangesAsync();
        return Results.Ok(groceryInDb);
    }
    //else
    return Results.NotFound();
});

//Create endpoint, add new grocery items
app.MapPost("/shoppinglist", async (Grocery grocery, ApiDbContext db) =>
{
    db.Groceries.Add(grocery);
    await db.SaveChangesAsync();
    return Results.Created($"/shoppinglist/{grocery.Id}", grocery);
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();