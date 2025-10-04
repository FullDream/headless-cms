using MediatR;
using SharedKernel;

namespace Contracts.Notifications;

public record ContentFieldTypeChangedNotification(string FieldName, FieldType Type) : INotification;