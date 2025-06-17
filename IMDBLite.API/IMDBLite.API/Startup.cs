using System.Text;
using System.Text.Json.Serialization;
using Dapper;
using IMDBLite.API.Converters;
using IMDBLite.API.Middlewares;
using IMDBLite.API.Repository;
using IMDBLite.API.Repository.Interfaces;
using IMDBLite.API.Services;
using IMDBLite.API.Services.Interfaces;
using IMDBLite.API.Settings;
using IMDBLite.API.Validations;
using IMDBLite.API.Validations.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Supabase;

namespace IMDBLite.API;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.Converters.Add(new GenderEnumConverter());
            });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                return new BadRequestObjectResult(new
                {
                    Message = "Validation Failed",
                    Errors = errors
                });
            };
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.FullName);

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Enter JWT Bearer token only",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        services.Configure<ConnectionString>(_configuration.GetSection("ConnectionStrings"));

        services.Configure<SupabaseSettings>(_configuration.GetSection("Supabase"));

        services.AddSingleton(provider =>
        {
            var settings = provider.GetRequiredService<IOptions<SupabaseSettings>>().Value;
            return new Client(settings.SupabaseUrl, settings.SupabaseKey);
        });
        services.AddScoped<ISupabaseService, SupabaseService>();

        services.AddAutoMapper(typeof(Startup).Assembly);
        services.AddScoped<IActorRepository, ActorRepository>();
        services.AddScoped<IProducerRepository, ProducerRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IProducerService, ProducerService>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IActorService, ActorService>();
        services.AddScoped<IGenreService, GenreService>();


        services.AddScoped<IActorValidator, ActorValidator>();
        services.AddScoped<IProducerValidator, ProducerValidator>();
        services.AddScoped<IGenreValidator, GenreValidator>();
        services.AddScoped<IMovieValidator, MovieValidator>();
        services.AddScoped<IAuthValidator, AuthValidator>();
        services.AddScoped<IReviewValidator, ReviewValidator>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
        });

        var key = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration["JWT:ValidIssuer"],
                    ValidAudience = _configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionHandling>();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}