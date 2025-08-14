using System.Text.Json;
using Application.Common.Messaging;

namespace Application.ContentEntries.Commands.Create;

public sealed record CreateContentEntryCommand(
	string ContentTypeName,
	Dictionary<string, JsonElement> Fields
) : ContentEntryCommandBase(ContentTypeName, Fields), ICommand<IReadOnlyDictionary<string, object?>>;