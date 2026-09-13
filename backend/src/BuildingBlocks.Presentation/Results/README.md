# OutcomeResult

`OutcomeResult` is both an MVC `ActionResult` and a Minimal API `IResult`.
`OutcomeResult<T>` inherits both behaviors. Existing controller signatures and implicit conversions from `Result` /
`Result<T>` are unchanged.

Both pipelines share `ResultProblemDetailsMapper`: success is 204 without a payload or 200 with a payload. The default
null-payload behavior remains 204. MVC execution is unchanged. When the host explicitly registers MVC executors, Minimal
endpoints retain the previous MVC formatter, content negotiation,
`Microsoft.AspNetCore.Mvc.JsonOptions`, and custom `ProblemDetailsFactory`
behavior for compatibility. Without MVC services they execute native HTTP results using
`Microsoft.AspNetCore.Http.Json.JsonOptions`. The mapper supplies the same problem fields, status codes, error priority,
and traceId without requiring a ProblemDetailsFactory. Optional `AddProblemDetails` customization is honored. Hosts
migrating serializer customizations away from MVC should configure them with `ConfigureHttpJsonOptions`.

## Registration

MVC integration extends the builder explicitly created by the host. Repeated calls do not register duplicate
conventions. Existing manual MVC convention registrations remain supported.

```csharp
builder.Services.AddControllers().AddOutcomeResults();
builder.Services.AddOpenApi();
var app = builder.Build();
var api = app.MapGroup("/api").WithOutcomeResults();
api.MapGet("/users/{id:guid}", GetById);
app.MapControllers(); // Legacy URLs remain unchanged.
```

Minimal-only hosts require no OutcomeResult service registration, `AddControllers`, or `AddMvcCore`. Attach the metadata
convention to a real API boundary:

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi(); // Only needed to expose an OpenAPI document.
var app = builder.Build();
var api = app.MapGroup("/api").WithOutcomeResults();
var group = api.MapGroup("/users");
group.MapGet("/{id:guid}", GetById);

static async Task<OutcomeResult<UserDto>> GetById(Guid id, ISender sender) =>
    await sender.Send(new GetUserByIdQuery(id));
```

No `.Produces`, `.ProducesProblem`, or per-endpoint metadata extension is needed. Supported return types:
`OutcomeResult`, `OutcomeResult<T>`, and their `Task<>`
and `ValueTask<>` wrappers. Endpoints mapped outside this group still execute OutcomeResult but do not receive the
group's response metadata convention.

Use an existing group prefix when migrating an API; do not introduce `/api` if that would change its public routes. The
current WebApi still has only MVC controllers and a hub, so it maps them directly without an artificial empty group.

## Why a group convention?

ASP.NET Core invokes `IEndpointMetadataProvider` before handler attributes and endpoint conventions such as `.Produces`.
A return-type metadata provider alone cannot see these future declarations and avoid duplicate entries. The public
`IEndpointConventionBuilder.Finally` hook on the API group runs after endpoint conventions, including their `Finally`
hooks. It can inspect the combined route pattern and final HTTP methods and preserve all explicitly declared statuses.
Put any custom group `Finally` metadata changes before `WithOutcomeResults`. There is no reflection or metadata
inspection per request.

MVC and Minimal API use `OutcomeResultResponseMetadata` for the original success, error-schema, HTTP-method, and
collection rules. MVC retains its historical action-selector route interpretation; Minimal API inspects the full route,
including parent group parameters. The original MVC convention namespace remains
`BuildingBlocks.Presentation.Results` for source compatibility.

Existing error metadata schemas are deliberately preserved: 400, 409 and 422 declare `ValidationProblemDetails`; 401,
403 and 404 declare `ProblemDetails`. Runtime still returns plain `ProblemDetails` for BusinessRule/Failure and includes
an `errors` extension for Conflict, exactly as before. This change does not alter the Result invariants or behavior of
malformed failures (such as an empty error array).

Tests: `dotnet test tests/BuildingBlocks.Presentation.Tests`.
