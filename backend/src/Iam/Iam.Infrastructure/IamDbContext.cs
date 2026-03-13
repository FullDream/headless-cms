using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Iam.Infrastructure;

public sealed class IamDbContext(DbContextOptions<IamDbContext> options) : IdentityDbContext<IamUser>(options);