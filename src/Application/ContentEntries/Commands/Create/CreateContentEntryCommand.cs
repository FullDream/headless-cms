using System.Text.Json;
using MediatR;

namespace Application.ContentEntries.Commands.Create;

public sealed record CreateContentEntryCommand(
	string ContentTypeName,
	Dictionary<string, JsonElement> Fields
) : ContentEntryCommandBase(ContentTypeName, Fields), IRequest<IReadOnlyDictionary<string, object?>>;