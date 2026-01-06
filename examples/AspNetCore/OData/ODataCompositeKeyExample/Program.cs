using ApiVersioning.Examples;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Options;
using Microsoft.OData;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder( args );

// Add services to the container.

builder.Services.AddControllers().AddOData( options =>
{
    options.UrlKeyDelimiter = ODataUrlKeyDelimiter.Parentheses;
    options.RouteOptions.EnableKeyAsSegment = true;
    options.RouteOptions.EnableKeyInParenthesis = true;

    // INFO: EnableLowerCamelCase() Calling (which is called by default) changes the property name, including the key property
    // Therefore composite key paramters need to adjusted acoordingly. e.g. keyId or keyid
    // If you don't want to change, you can get around that by setting `EnablePropertyNameCaseInsensitive` to true.

    //options.RouteOptions.EnablePropertyNameCaseInsensitive = true;
} );
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning( options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
} ).AddODataApiExplorer(options =>
{
    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
    // note: the specified format code will format the version as "'v'major[.minor][-status]"
    options.GroupNameFormat = "'v'VVV";

    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
    // can also be used to control the format of the API version in route templates
    options.SubstituteApiVersionInUrl = true;
} )
        .AddOData(
            options =>
            {

                // INFO: you do NOT and should NOT use both the query
                // string and url segment methods together. this configuration
                // is merely illustrating that they can coexist and allows you
                // to easily experiment with either configuration. one of these
                // would be removed in a real application.

                // WHEN VERSIONING BY: query string, header, or media type
                options.AddRouteComponents( "api" );

                // WHEN VERSIONING BY: url segment
                options.AddRouteComponents( "api/v{version:apiVersion}" );

            } );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(
    options =>
    {
        // add a custom operation filter which sets default values
        options.OperationFilter<SwaggerDefaultValues>();

        var fileName = typeof( Program ).Assembly.GetName().Name + ".xml";
        var filePath = Path.Combine( AppContext.BaseDirectory, fileName );

        // integrate xml comments
        options.IncludeXmlComments( filePath );
    } );

var app = builder.Build();

// Configure HTTP request pipeline.

if ( app.Environment.IsDevelopment() )
{
    // Access ~/$odata to identify OData endpoints that failed to match a route template.
    app.UseODataRouteDebug();
}

app.UseSwagger();
if ( app.Environment.IsDevelopment() )
{
    app.UseSwaggerUI(
        options =>
        {
            var descriptions = app.DescribeApiVersions();

            // build a swagger endpoint for each discovered API version
            foreach ( var description in descriptions )
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                var name = description.GroupName.ToUpperInvariant();
                options.SwaggerEndpoint( url, name );
            }
        } );
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();