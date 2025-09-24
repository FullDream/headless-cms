using System.Text.Json;
using BuildingBlocks.Messaging;

namespace ContentEntries.Application.Commands.Create;

public sealed record CreateContentEntryCommand(
	string ContentTypeName,
	Dictionary<string, JsonElement> Fields
) : ContentEntryCommandBase(ContentTypeName, Fields), ICommand<IReadOnlyDictionary<string, object?>>;