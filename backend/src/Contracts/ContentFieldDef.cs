using SharedKernel;

namespace Contracts;

public sealed record ContentFieldDef(
	string Name,
	FieldType Type,
	bool IsRequired,
	int? Order = null
);