using Chat.Application.Hubs;
using Chat.Common.Repositories.Interfaces;
using Chat.Common.Services;
using Chat.Data;
using Microsoft.EntityFrameworkCore;

namespace Chat.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            builder.Services.AddDbContext<ChatDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
                

            builder.Services.AddScoped<IChatRoomRepository, ChatRoomRepository>();

            builder.Services.AddControllers();
            builder.Services.AddSignalR();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapControllers();
            app.MapHub<ChatRoomHub>("/chatHub");

            app.Run();
        }
    }
}
