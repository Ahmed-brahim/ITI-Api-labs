using Microsoft.EntityFrameworkCore;

namespace lab1._1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer(); // ✅ ضروري
            builder.Services.AddSwaggerGen();           // ✅ ضروري

            builder.Services.AddDbContext<Models.AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();          
                app.UseSwaggerUI();       
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
