using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DoAnChuyenNganh.Contract.Repositories.Entity;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Repositories.Context;
using DoAnChuyenNganh.Repositories.Entity;
using DoAnChuyenNganh.Services;
using DoAnChuyenNganh.Services.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using DoAnChuyenNganh.Services.Mapping;
using DoAnChuyenNganh.Services.EmailSettings;
using DoAnChuyenNganh.Services.Configs;

namespace DoAnChuyenNganhBE.API
{
    public static class DependencyInjection
    {
        public static void AddConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCorsConfig();
            services.ConfigRoute();
            services.AddConfigTimeToken();
            services.AddSwaggerUIAuthentication();
            services.AddMemoryCache();
            services.AddDatabase(configuration);
            services.AddIdentity();
            services.AddInfrastructure(configuration);
            services.AddServices();
            services.AddAuthenticationBearer(configuration);
            services.AddAutoMapperConfig();
            services.AddEmailConfig(configuration);
        }
        public static void AddAuthenticationBearer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new Exception("JWT_ISSUER is not set"),
                        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new Exception("JWT_AUDIENCE is not set"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new Exception("JWT_KEY is not set")))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chatHub"))
                            {
                                if (accessToken.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                                {
                                    context.Token = accessToken.ToString().AsSpan(7).Trim().ToString();
                                }
                                else
                                {
                                    context.Token = accessToken;
                                }
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

        }
        public static void ConfigRoute(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });
        }
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ?? throw new Exception("DATABASE_CONNECTION_STRING is not set"));
            });
        }

        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
            })
             .AddEntityFrameworkStores<DatabaseContext>()
             .AddDefaultTokenProviders();
        }
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ILecturerService, LecturerService>();
            services.AddScoped<ILecturerPlanService, LecturerPlanService>();
            services.AddScoped<ILecturerActivitiesService, LecturerActivitiesService>();
            services.AddScoped<IActivitiesService, ActivitiesService>();
            services.AddScoped<ITeachingScheduleService, TeachingScheduleService>();
            services.AddScoped<IBusinessService, BusinessService>();
            services.AddScoped<IBusinessCollaborationService, BusinessCollaborationService>();
            services.AddScoped<IBusinessActivitesService, BusinessActivitiesService>();
            services.AddScoped<IInternshipManagementService, InternshipManagementService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IExtracurricularActivitiesService, ExtracurricularActivitiesService>();
            services.AddScoped<IStudentExpectationsService, StudentExpectationsService>();
            services.AddScoped<IOutgoingDocumentService, OutgoingDocumentService>();
            services.AddHttpContextAccessor();
            services.AddScoped<DepartmentManager>();
            services.AddScoped<LecturerManager>();
            services.AddScoped<IIncomingDocumentService, IncomingDocumentService>();
            services.AddScoped<IAlumniService, AlumniService>();
            services.AddScoped<IAlumniActivitiesService, AlumniActivitiesService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IAlumniCompanyService, AlumniCompanyService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();

        }
        public static void AddAutoMapperConfig(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
        }
        public static void AddEmailConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        }
        public static void AddSwaggerUIAuthentication(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "QuanLyHocVu.API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }
        public static void AddCorsConfig(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.WithOrigins(Environment.GetEnvironmentVariable("CLIENT_DOMAIN") ?? throw new Exception("CLIENT_DOMAIN is not set"))
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                });
            });
        }
        public static void AddConfigTimeToken(this IServiceCollection services)
        {
            services.Configure<DataProtectionTokenProviderOptions>(options =>
                    options.TokenLifespan = TimeSpan.FromMinutes(30));
        }
    }
}
