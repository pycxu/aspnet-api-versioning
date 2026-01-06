namespace ApiVersioning.Examples.Controllers;

using ApiVersioning.Examples.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

[ApiVersion( 2.0 )]
public class Orders2Controller : ODataController
{
    // GET ~/api/v2/orders
    [EnableQuery]
    public IActionResult Get( ODataQueryOptions<Order> options ) =>
        Ok( new[] { new Order() { Id = 1, Customer = $"Bill Mei" } } );

    // GET ~/api/v2/orders/customer={keycustomer},id={keyid}
    // GET ~/api/v2/orders(customer={keycustomer},id={keyid})
    [EnableQuery]
    // INFO: 
    // Composite key order: 
    // - is alphabetical by default and ASP.NET Core routing requires the URI to match that order.
    // - to change this, set the key `order` in model configuration. e.g. builder.EntityType<Order>().Property(p => p.Id).Order = 1;
    // Composite key parameter:
    // - parameter names must follow the `key + property` convention, adjusted with `EnableLowerCamelCase()`
    // - EnableLowerCamelCase = true -> keyid, keycustomer
    // - EnableLowerCamelCase = false -> keyId, keyCustomer
    // - alternatively, set `RouteOptions.EnablePropertyNameCaseInsensitive = true`
    public IActionResult Get( int keyid, string keycustomer, ODataQueryOptions<Order> options ) =>
        Ok( new Order() { Id = keyid, Customer = $"{keycustomer} (composite key)" } );
}