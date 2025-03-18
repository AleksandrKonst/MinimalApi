var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

var promotions = new List<Promotion>
{
    new() { Id = 1, Name = "New Year Sale", Discount = 20 },
    new() { Id = 2, Name = "Black Friday Deal", Discount = 50 }
};


app.MapGet("/promotions", () => promotions);


app.MapGet("/promotions/{id}", (int id) =>
{
    var promotion = promotions.Find(p => p.Id == id);
    return promotion is not null ? Results.Ok(promotion) : Results.NotFound();
});


app.MapPost("/promotions", (Promotion newPromotion) =>
{
    if (newPromotion.Id == 0 || string.IsNullOrEmpty(newPromotion.Name))
        return Results.BadRequest("Invalid data.");

    promotions.Add(newPromotion);
    return Results.Created($"/promotions/{newPromotion.Id}", newPromotion);
});

app.MapPut("/promotions/{id}", (int id, Promotion updatedPromotion) =>
{
    var promotion = promotions.Find(p => p.Id == id);

    if (promotion is null)
        return Results.NotFound();

    promotion.Name = updatedPromotion.Name;
    promotion.Discount = updatedPromotion.Discount;

    return Results.NoContent();
});

app.MapDelete("/promotions/{id}", (int id) =>
{
    var promotion = promotions.Find(p => p.Id == id);
    if (promotion is not null)
    {
        promotions.Remove(promotion);
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();

public class Promotion
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Discount { get; set; }
}