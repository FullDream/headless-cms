using Microsoft.Extensions.DependencyInjection;

namespace Iam.Presentation;

public static class DependencyInjection
{
	public static IMvcBuilder AddIamPresentation(this IMvcBuilder builder) =>
		builder.AddApplicationPart(typeof(DependencyInjection).Assembly);
}
