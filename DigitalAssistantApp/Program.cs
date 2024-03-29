using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DigitalAssistantApp;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddDbContext<DadContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); //����������� � ��
var app = builder.Build();

using (DadContext tc = new DadContext()) //�������� ����������� � ��
{
    if (tc.Database.CanConnect())
    {
        app.Logger.LogInformation("����������� � �� ������������");
    }
    else
    {
        app.Logger.LogInformation("�� ������� ������������ � ��");
    }
}
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;       //noncommercial-��� ������������������ ��������
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
