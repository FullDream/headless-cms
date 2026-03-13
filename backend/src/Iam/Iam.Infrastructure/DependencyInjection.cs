using Iam.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iam.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddIamInfrastructure(
		this IServiceCollection services,
		IConfiguration config)
	{
		var connectionString = config.GetConnectionString("DefaultConnection");

		services.AddDbContext<IamDbContext>(options =>
			options.UseSqlite(connectionString)
				.UseSnakeCaseNamingConvention());

		services
			.AddAuthentication(IdentityConstants.ApplicationScheme)
			.AddCookie(IdentityConstants.ApplicationScheme,
				options =>
				{
					options.Events.OnRedirectToLogin = ctx =>
					{
						ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
						return Task.CompletedTask;
					};

					options.Events.OnRedirectToAccessDenied = ctx =>
					{
						ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
						return Task.CompletedTask;
					};
				});

		services.AddAuthorizationCore();
		services.AddIdentityCore<IamUser>()
			.AddEntityFrameworkStores<IamDbContext>()
			.AddSignInManager();

		services.AddScoped<IAuthService, AuthService>();

		return services;
	}
}