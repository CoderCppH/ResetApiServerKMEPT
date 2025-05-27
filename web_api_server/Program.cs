using System.Collections;
using System.Text;
using Orm;
using Orm.Type;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
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
            var found_user = (comm?.SelectFrom<Orm.Type.User>("users")?.Find(item=> item.email == user.email));
            if (found_user == null)
                return Results.NotFound(new {message = "user not found or null"});
            return Results.Json(found_user);
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
        bool state = comm.Delete("users", 
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
            ?.Where(item => 
            // Диалог между двумя пользователями в любом направлении
                (item.user_from_id == id_user_from && item.user_to_id == id_user_to) ||
                (item.user_from_id == id_user_to && item.user_to_id == id_user_from)
            )
            .OrderBy(item => item.timestamp)
            .ToArray();
            System.Console.WriteLine($"From_id: {id_user_from}; To_id: {id_user_to}; list_size: {list_message?.Count}; filter_size: {filteredMessages?.Count()}");
            return Results.Ok(filteredMessages);
        }
    });
//Lenta 
app.MapGet("/api/lenta/",
    () =>{
        using (Orm.ExceCommand comm = new ExceCommand())
        {
            var list = comm.SelectFrom<Orm.Type.Lenta>("lenta_tables");
            list.ForEach(x => x.image_post = Pack.ImagePk.ImageCompressor.CompressBase64Image(x.image_post));
            return Results.Ok(list);
        }
    });
app.MapPost("/api/lenta/", 
    (Lenta lenta) => {
        using (ExceCommand comm = new ExceCommand())
        {
            if (lenta.image_post.Equals("1"))
            {
                var img_array_bytes = File.ReadAllBytes("./src/resource/imgs/empty.jpg");
                var base_64_image = System.Convert.ToBase64String(img_array_bytes);
                lenta.image_post = base_64_image;
            }

            bool state = comm.Insert(
                "lenta_tables",
                "name_post, description_post, image_post, id_user",
                "@name_post, @description_post, @image_post, @id_user",
                lenta);

            if (state) return Results.Json(new { message = "success inserts lenta" });
            else return Results.Json(new { message = "failed insert lenta" });
        }
    });
app.MapGet("/api/lenta/{id_lenta}",
    (int id_lenta) =>
    { 
        using (Orm.ExceCommand comm = new ExceCommand()) {
                var list_lenta = comm.SelectFrom<Orm.Type.Lenta>("lenta_tables");
                list_lenta.ForEach(x => x.image_post = Pack.ImagePk.ImageCompressor.CompressBase64Image(x.image_post));
                return Results.Ok(list_lenta?.FirstOrDefault(item => item.id == id_lenta));
            }
    });
app.MapPut("/api/lenta/",
    (Lenta lenta)=>
    {
        using (ExceCommand comm = new ExceCommand())
        {
            bool state = false;
            if (lenta.image_post.Equals("1"))
            {
                state = comm.Update(
                "lenta_tables",
                "name_post = @name_post, " +
                "description_post = @description_post",
                "id = @id",
                lenta);
            }
            else
            {
                state = comm.Update(
                "lenta_tables",
                "name_post = @name_post, " +
                "description_post = @description_post, " +
                "image_post = @image_post",
                "id = @id",
                lenta);
            }
            

            if(state)   return Results.Json(new {message = "success updates lenta"});
            else return Results.Json(new {message = "failed update lenta"});
        }
    });
app.MapDelete("/api/lenta/{id_lenta}",
    (int id_lenta) =>
    {
        using (ExceCommand comm = new ExceCommand())
        {
            bool state = comm.Delete(
                "lenta_tables",
                "id = @id",
                new Lenta() { id = id_lenta, description_post = "", name_post = "", id_user = 0, image_post = "" });

            if(state)   return Results.Json(new {message = "success deleted lenta"});
            else return Results.Json(new {message = "failed delete lenta"});
        }
    });

app.Run();
