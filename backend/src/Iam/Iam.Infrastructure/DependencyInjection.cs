using BuildingBlocks.Authorization;
using Iam.Application.Abstractions.Authentication;
using Iam.Domain.AccessResource;
using Iam.Domain.Roles;
using Iam.Infrastructure.AccessResources;
using Iam.Infrastructure.Authentication;
using Iam.Infrastructure.Authorization;
using Iam.Infrastructure.Authorization.AccessResources;
using Iam.Infrastructure.Initialization;
using Iam.Infrastructure.Persistence;
using Iam.Infrastructure.Roles;
using Iam.Infrastructure.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iam.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddIamInfrastructure(this IServiceCollection services, IConfiguration config)
	{
		var connectionString = config.GetConnectionString("DefaultConnection");

		services.AddDbContext<IamDbContext>(options =>
			options.UseSqlite(connectionString).UseSnakeCaseNamingConvention());

		services.AddHybridCache();

		services.AddAuthentication(IdentityConstants.ApplicationScheme)
			.AddCookie(
				IdentityConstants.ApplicationScheme,
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

		services.AddIdentityCore<PersistenceUser>()
			.AddRoles<PersistenceRole>()
			.AddEntityFrameworkStores<IamDbContext>()
			.AddSignInManager()
			.AddClaimsPrincipalFactory<ClaimsPrincipalFactory>();

		services.AddScoped<DatabaseInitializer>();
		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IRoleRepository, RoleRepository>();
		services.AddScoped<IAccessResourceRepository, AccessResourceRepository>();
		services.AddScoped<IPermissionChecker, PermissionChecker>();
		services.AddScoped<RoleSnapshotProvider>();
		services.AddScoped<UserRoleProvider>();
		services.AddScoped<AccessResourceProvider>();

		return services;
	}

	public static async Task InitializeIamInfrastructureAsync(
		this IServiceProvider serviceProvider,
		CancellationToken cancellationToken = default)
	{
		await using var scope = serviceProvider.CreateAsyncScope();

		var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();

		await initializer.InitializeAsync(cancellationToken);
	}
}
