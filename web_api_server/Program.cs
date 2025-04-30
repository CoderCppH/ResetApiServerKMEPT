using System.Text;
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
app.MapGet("/api/users/", ()=> {
    using (Orm.ExceCommand comm = new Orm.ExceCommand()) 
    {
        return comm.SelectFrom<Orm.Type.User>("users");
    }
});
app.MapPost("/api/find_user/", 
    (User user)=> {
        using(ExceCommand comm = new ExceCommand()) {
            return Results.Ok(comm?.SelectFrom<Orm.Type.User>("users")?.Find(item=> item.email == user.email));
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

//POST REGISTER USER
app.MapPost("/api/users/",
    (User user)=> {
        using(Orm.ExceCommand comm = new ExceCommand()  ) {
            bool state = comm.Insert(
                "users", 
                "first_name, last_name, email", 
                "@first_name, @last_name, @email", 
            user);

            if(state)   return Results.Json(new {message = "success inserts user"});
            else return Results.Json(new {message = "failed insert user"});
        }
    });

//UPDATE USER FROM ID
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

        if(state)   return Results.Json(new {message = "success updates user"});
        else return Results.Json(new {message = "failed update user"});
        
    }
});

//DELETE USER
app.MapDelete("/api/users/{id}", (int id)=> {
    using (Orm.ExceCommand comm = new ExceCommand()) {
        bool state = comm.Drop("users", 
        "id = @id", new User() { id = id } );
        if(state)   return Results.Json(new {message = "success deleted user"});
        else return Results.Json(new {message = "failed delete user"});
    } 
});


///Message
app.MapGet("/api/messagers/",
    ()=>{
        using (Orm.ExceCommand comm = new ExceCommand()) {
            System.Console.WriteLine("Get_messagers");
            return comm.SelectFrom<Orm.Type.Message>("messagers");
        }
    });


//INSERT MESSAGE
app.MapPost("/api/messagers/",
    (Message message)=> {
        using (Orm.ExceCommand comm = new ExceCommand()) {
            bool state = comm.Insert(
                "messagers",
                "user_from_id, user_to_id, message",
                "@user_from_id, @user_to_id, @message",
                message
            );

            if(state)   return Results.Json(new {message = "success inserts messagers"});
            else return Results.Json(new {message = "failed insert messagers"});
        }
    });
app.MapGet("/api/messagers/{id_user}", 
    (int id_user)=>{
        using (Orm.ExceCommand comm = new ExceCommand()) {
            var list_message = comm.SelectFrom<Orm.Type.Message>("messagers");
            return Results.Ok(list_message?.Where(item=> item.user_from_id == id_user).ToArray());
        }
    });
app.MapGet("/api/messagers/{id_user_from}/{id_user_to}", 
    (int id_user_from, int id_user_to) => {
        using (Orm.ExceCommand comm = new ExceCommand()) 
        {
            
            // Получаем все сообщения из базы данных
            var list_message = comm.SelectFrom<Orm.Type.Message>("messagers");
            // Фильтруем сообщения: сначала от отправителя, затем от получателя
            var filteredMessages = list_message
                ?.Where(item => (item.user_from_id == id_user_from || item.user_from_id == id_user_to ) && (item.user_to_id == id_user_to || item.user_to_id ==id_user_from))
                .OrderBy(item => item.timestamp)
                .ToArray();
            System.Console.WriteLine($"From_id: {id_user_from}; To_id: {id_user_to}; list_size: {list_message?.Count}; filter_size: {filteredMessages?.Count()}");
            return Results.Ok(filteredMessages);
        }
    });


app.Run();
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
