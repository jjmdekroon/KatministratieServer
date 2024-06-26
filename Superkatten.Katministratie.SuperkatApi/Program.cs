using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Superkatten.Katministratie.Application;
using Superkatten.Katministratie.Application.Authenticate.Middleware;
using Superkatten.Katministratie.Application.Configuration;
using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Infrastructure;
using System.Text;

const string SWAGGER_DOC_VERSION = "v1";
//const string CORS_POLICY_NAME = "DefaultCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

//----------------------------------------------------------------------------------------------------------
builder.Configuration.AddEnvironmentVariables();

{
    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc(SWAGGER_DOC_VERSION, new OpenApiInfo { Title = "Superkatten", Version = SWAGGER_DOC_VERSION });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorisation header using the bearer scheme. \r\n\r\n"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        { 
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });
    builder.Services.AddApplicationServices(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddAuthentication(options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options => {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true, //+
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidAudience = null,
            ValidIssuer = null,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(UserAuthorisationConfiguration.Secret)),
            ClockSkew = TimeSpan.Zero
        };
    });

    builder.Services.AddAuthorization(options =>
    {
        options.DefaultPolicy = new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();

        options.AddPolicy(SuperkattenPolicies.POLICY_ADMINISTRATOR, policy =>
        {
            var allPermissionValues = (PermissionEnum[])Enum.GetValues(typeof(PermissionEnum));
            var array = allPermissionValues
                .Select(value => value.ToString())
                .ToArray();
            foreach (var item in array)
            {
                policy.RequireRole(item);
            }
        });

        options.AddPolicy(SuperkattenPolicies.POLICY_VIEW_ONLY, policy =>
            policy.RequireRole(
                PermissionEnum.Viewer.ToString()
            ));

        options.AddPolicy(SuperkattenPolicies.POLICY_GASTGEZIN, policy =>
            policy.RequireRole(
                PermissionEnum.Viewer.ToString(),
                PermissionEnum.Gastgezin.ToString()
            ));

        options.AddPolicy(SuperkattenPolicies.POLICY_COORDINATOR, policy =>
            policy.RequireRole(
                PermissionEnum.Viewer.ToString(),
                PermissionEnum.Coordinator.ToString()
            ));
    });

/*    builder.Services.AddCors(options =>
    {
        options.AddPolicy(
            name: CORS_POLICY_NAME, 
            builder => 
            {
                builder.WithOrigins(
                    "https://localhost:7292"
                );
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });
    });*/

    builder.Services.AddLogging();
}

//----------------------------------------------------------------------------------------------------------
var app = builder.Build();

app.UseSwagger();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Superkatten Api v1"));
}
else
{
    app.UseSwaggerUI();
    app.UseExceptionHandler("/Error");
}

// For routing and sequence
// see: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-6.0#middleware-order
//

app.UseHttpsRedirection();
app.UseRouting();
app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();
//app.UseCors(CORS_POLICY_NAME);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
