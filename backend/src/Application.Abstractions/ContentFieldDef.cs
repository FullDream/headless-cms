using SharedKernel;

namespace Application.Abstractions;

public sealed record ContentFieldDef(
	string Name,
	FieldType Type,
	bool IsRequired,
	int? Order = null
);