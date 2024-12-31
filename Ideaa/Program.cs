////using Ideas.Data;
////using Microsoft.AspNetCore.Http.Features;
////using Microsoft.EntityFrameworkCore;
////using Microsoft.OpenApi.Models;
////using Microsoft.AspNetCore.Authentication.JwtBearer;
////using Microsoft.IdentityModel.Tokens;
////using System.Text;
////using Ideas.Data.Models;
////using Microsoft.AspNetCore.Identity;
////using Ideaa.Extensions;
////using FluentAssertions.Common;

////var builder = WebApplication.CreateBuilder(args);

////// التأكد من تحميل ملف الإعدادات
////builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
////builder.Configuration.AddEnvironmentVariables(); // إذا كنت تستخدم بيئات مختلفة

////// إضافة Identity مع التخصيصات المتعلقة بكلمة المرور
////builder.Services.AddIdentity<User, IdentityRole>()
////    .AddEntityFrameworkStores<AppDbContext>()
////    .AddDefaultTokenProviders();

////builder.Services.AddControllers()
////        .AddNewtonsoftJson(options =>
////            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); // تجاهل الحلقات المرجعية




////// إضافة التخصيصات الخاصة بكلمة المرور
////builder.Services.Configure<IdentityOptions>(options =>
////{
////    options.Password.RequireDigit = true;
////    options.Password.RequiredLength = 6;  // الحد الأدنى للطول
////    options.Password.RequireNonAlphanumeric = false;  // لا يشترط وجود رموز خاصة
////    options.Password.RequireUppercase = true;
////    options.Password.RequireLowercase = true;

////});

////// إضافة خدمات التحكم بالمصادر (Controllers)
////builder.Services.AddControllers().AddNewtonsoftJson();

////// إعداد قاعدة البيانات باستخدام اتصال موجود في ملف الإعدادات
////builder.Services.AddDbContext<AppDbContext>(options =>
////    options.UseSqlServer(builder.Configuration.GetConnectionString("IdeaDB")));


////// التحقق من تحميل القيم من التكوين بشكل صحيح
////var secretKey = builder.Configuration["Jwt:SecretKey"];
////var issuer = builder.Configuration["Jwt:Issuer"];
////var audience = builder.Configuration["Jwt:Audience"];

////if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
////{
////    throw new ArgumentNullException("JWT configuration values are missing.");
////}


////// تكوين CORS للسماح بأي مصدر، أي رأس، وأي طريقة
////builder.Services.AddCors(options =>
////{
////    options.AddPolicy("CorsPolicy", policy =>
////    {
////        policy.AllowAnyOrigin()
////              .AllowAnyHeader()
////              .AllowAnyMethod();
////    });
////});

////// إضافة خدمة Swagger لتوثيق API
////builder.Services.AddEndpointsApiExplorer();

////builder.Services.AddCustomJwtAuth(builder.Configuration);
////builder.Services.AddSwaggerGenJwtAuth();

////// تحديد حجم الملفات في الطلبات
////builder.Services.Configure<FormOptions>(options =>
////{
////    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // الحد الأقصى لحجم الملفات 10 ميغابايت
////});

////var app = builder.Build();

////// تكوين مسار الطلبات HTTP
////if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
////{
////    app.UseDeveloperExceptionPage();

////    // تمكين Swagger UI
////    app.UseSwagger();
////    app.UseSwaggerUI(c =>
////    {
////        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
////        c.RoutePrefix = string.Empty; // يجعل Swagger الصفحة الافتراضية
////    });
////}
////else
////{
////    app.UseExceptionHandler("/Home/Error");
////    app.UseHsts();
////}



////// إضافة دعم CORS
////app.UseCors("CorsPolicy");

////// إعادة التوجيه لـ HTTPS
////app.UseHttpsRedirection();

////// استخدام الملفات الثابتة
////app.UseStaticFiles();

////// تفعيل المصادقة قبل التصريح

////app.UseAuthentication();

////// تفعيل التصريح
////app.UseAuthorization();

////// ربط المسارات بالمتحكمات
////app.MapControllers();

////// تشغيل التطبيق
////app.Run();

//using Ideas.Data;
//using Microsoft.AspNetCore.Http.Features;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.OpenApi.Models;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using Ideas.Data.Models;
//using Microsoft.AspNetCore.Identity;
//using Ideaa.HubS;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddSignalR();


//// التأكد من تحميل ملف الإعدادات
//builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
//builder.Configuration.AddEnvironmentVariables(); // إذا كنت تستخدم بيئات مختلفة

//// إضافة Identity مع التخصيصات المتعلقة بكلمة المرور
//builder.Services.AddIdentity<User, IdentityRole>()
//    .AddEntityFrameworkStores<AppDbContext>()
//    .AddDefaultTokenProviders();

//builder.Services.AddControllers()
//        .AddNewtonsoftJson(options =>
//            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); // تجاهل الحلقات المرجعية

//// إضافة التخصيصات الخاصة بكلمة المرور
//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.Password.RequireDigit = true;
//    options.Password.RequiredLength = 6;  // الحد الأدنى للطول
//    options.Password.RequireNonAlphanumeric = false;  // لا يشترط وجود رموز خاصة
//    options.Password.RequireUppercase = true;
//    options.Password.RequireLowercase = true;
//});

//// إضافة خدمات التحكم بالمصادر (Controllers)
//builder.Services.AddControllers().AddNewtonsoftJson();

//// إعداد قاعدة البيانات باستخدام اتصال موجود في ملف الإعدادات
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("IdeaDB")));

//// التحقق من تحميل القيم من التكوين بشكل صحيح
//var secretKey = builder.Configuration["Jwt:SecretKey"];
//var issuer = builder.Configuration["Jwt:Issuer"];
//var audience = builder.Configuration["Jwt:Audience"];

//if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
//{
//    throw new ArgumentNullException("JWT configuration values are missing.");
//}

//// تكوين CORS للسماح بأي مصدر، أي رأس، وأي طريقة
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("CorsPolicy", policy =>
//    {
//        policy.AllowAnyOrigin()
//              .AllowAnyHeader()
//              .AllowAnyMethod();
//    });
//});

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.RequireHttpsMetadata = false;
//    options.SaveToken = true;
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"], // التأكد من أن القيمة موجودة في الإعدادات
//        ValidateAudience = true,  // تم تفعيل التحقق من Audience
//        ValidateLifetime = true,
//        ValidAudience = builder.Configuration["Jwt:Audience"], // إضافة Audience
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])) // التأكد من أن SecretKey صحيح
//    };
//});

//// إضافة خدمة Swagger لتوثيق API
//builder.Services.AddEndpointsApiExplorer();

//// إضافة Swagger
//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Description = "Enter the JWT token in the format 'Bearer {your token}'"
//    });

//    options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                },
//                Scheme = "oauth2",
//                Name = "Bearer",
//                In = ParameterLocation.Header
//            },
//            new List<string>()
//        }
//    });
//});

//// تحديد حجم الملفات في الطلبات
//builder.Services.Configure<FormOptions>(options =>
//{
//    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // الحد الأقصى لحجم الملفات 10 ميغابايت
//});

//var app = builder.Build();



//// تكوين مسار الطلبات HTTP
//if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
//{
//    app.UseDeveloperExceptionPage();

//    // تمكين Swagger UI
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
//        c.RoutePrefix = string.Empty; // يجعل Swagger الصفحة الافتراضية
//    });
//}
//else
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//// إضافة دعم CORS
//app.UseCors("CorsPolicy");

//// إعادة التوجيه لـ HTTPS
//app.UseHttpsRedirection();

//// استخدام الملفات الثابتة
//app.UseStaticFiles();

//// تفعيل المصادقة قبل التصريح
//app.UseAuthentication();


//// تفعيل التصريح
//app.UseAuthorization();

//// ربط المسارات بالمتحكمات
//app.MapControllers();

//app.MapHub<NotificationHub>("/notificationHub");
//// تشغيل التطبيق
//app.Run();
using Ideas.Data;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ideas.Data.Models;
using Microsoft.AspNetCore.Identity;
using Ideaa.HubS;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

// التأكد من تحميل ملف الإعدادات
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables(); // إذا كنت تستخدم بيئات مختلفة

// إضافة Identity مع التخصيصات المتعلقة بكلمة المرور
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); // تجاهل الحلقات المرجعية


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// إضافة التخصيصات الخاصة بكلمة المرور
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;  // الحد الأدنى للطول
    options.Password.RequireNonAlphanumeric = false;  // لا يشترط وجود رموز خاصة
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
});

// إضافة خدمات التحكم بالمصادر (Controllers)
builder.Services.AddControllers().AddNewtonsoftJson();

// إعداد قاعدة البيانات باستخدام اتصال موجود في ملف الإعدادات
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdeaDB")));

// التحقق من تحميل القيم من التكوين بشكل صحيح
var secretKey = builder.Configuration["Jwt:SecretKey"];
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
{
    throw new ArgumentNullException("JWT configuration values are missing.");
}

// تكوين CORS للسماح بأي مصدر، أي رأس، وأي طريقة
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
    };
});

// إضافة خدمة Swagger لتوثيق API
builder.Services.AddEndpointsApiExplorer();

// إضافة Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the JWT token in the format 'Bearer {your token}'"
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
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// تحديد حجم الملفات في الطلبات
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // الحد الأقصى لحجم الملفات 10 ميغابايت
});

var app = builder.Build();

// تكوين مسار الطلبات HTTP
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();

    // تمكين Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // يجعل Swagger الصفحة الافتراضية
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// إضافة دعم CORS
app.UseCors("CorsPolicy");

// إعادة التوجيه لـ HTTPS
app.UseHttpsRedirection();

// استخدام الملفات الثابتة
app.UseStaticFiles();

// تفعيل المصادقة قبل التصريح
app.UseAuthentication();

// تفعيل التصريح
app.UseAuthorization();

// ربط المسارات بالمتحكمات
app.MapControllers();


// تكوين مسار Hub
app.MapHub<NotificationHub>("/notificationHub");



// تشغيل التطبيق
app.Run();
