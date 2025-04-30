using Orm;
using Orm.Type;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
//Get List<Temp>
app.MapGet("/weatherforecast", () => {
    
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

//Get List<User>
app.MapGet("/api/users", ()=> {
    using (Orm.ExceCommand comm = new Orm.ExceCommand()) 
    {
        return comm.SelectFrom<Orm.Type.User>("users");
    }
});

//Get FIND USER FROM ID
app.MapGet("/api/users/{id}", (int id)=> {
    using (Orm.ExceCommand comm = new Orm.ExceCommand()) 
    {
        var lits_user = comm.SelectFrom<User>("users");
        
        User? user = lits_user?.FirstOrDefault(p=> p.id == id);
        
        if (user == null) return Results.NotFound(new {message = "Пользователь не найден"});
        
        return Results.Json(user);
    }
});

app.MapPut("/api/users/", (User user)=> {
    using (Orm.ExceCommand comm = new Orm.ExceCommand()) {
        bool state = comm.Update("users",
        //SET
        "first_name = @first_name, " + 
        "last_name = @last_name, " +
        "email = @email",
        //WHERE 
        "id = @id", 
        user);

        if(state)   return Results.Json(new {message = "success uodates user"});
        else return Results.Json(new {message = "failed update user"});
        
    }
});
app.MapDelete("/api/users/{id}", (int id)=> {
    using (Orm.ExceCommand comm = new ExceCommand()) {
        bool state = comm.Drop("users", 
        "id = @id", new User() { id = id } );
        if(state)   return Results.Json(new {message = "success deleted user"});
        else return Results.Json(new {message = "failed delete user"});
    } 
});


///Message



app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
