using ContentTypes.Application.Common.ContentField;
using ContentTypes.Core;
using MediatR;
using SharedKernel.Result;

namespace ContentTypes.Application.UpdateField;

internal sealed class UpdateFieldInContentTypeHandler(IContentTypeRepository repository)
	: IRequestHandler<UpdateFieldInContentTypeCommand, Result<ContentFieldDto>>
{
	public async Task<Result<ContentFieldDto>> Handle(UpdateFieldInContentTypeCommand request,
		CancellationToken cancellationToken)
	{
		var contentType = await repository.FindByIdAsync(request.ContentTypeId, cancellationToken);

		if (contentType is null) return ContentTypeErrors.NotFound(nameof(request.ContentTypeId));


		var fieldRes = contentType.UpdateField(request.ContentFieldId,
			new ContentFieldPatch(
				Name: request.UpdateDto.Name,
				Label: request.UpdateDto.Label,
				Type: request.UpdateDto.Type,
				IsRequired: request.UpdateDto.IsRequired
			)
		);

		if (fieldRes.IsFailure) return fieldRes.Errors;

		await repository.SaveChangesAsync(cancellationToken);

		return fieldRes.Value.ToDto();
	}
}