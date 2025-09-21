using SharedKernel.Result;

namespace ContentTypes.Core;

public static class ContentTypeErrors
{
	public static Error NotFound(string property) =>
		new Error("ContentType.NotFound", "ContentType is not found", property, ErrorType.NotFound);

	public static Error AlreadyExist(string name) =>
		new("ContentType.AlreadyExist",
			$"ContentType '{name}' already exists.",
			nameof(ContentType.Name),
			ErrorType.Conflict);

	public static Error ContentFieldNotFound(Guid id) =>
		new("ContentType.ContentFieldNotFound",
			$"Field with id {id} not found.",
			nameof(ContentField.Id),
			ErrorType.NotFound);

	public static Error ContentFieldNameIsUnique(string name) =>
		new("ContentType.ContentFieldIsUnique",
			$"Field name '{name}' must be unique.",
			nameof(ContentField.Name),
			ErrorType.Conflict);
}