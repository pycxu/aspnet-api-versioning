namespace ApiVersioning.Examples.Controllers;

using ApiVersioning.Examples.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

[ApiVersion( 1.0 )]
public class PeopleController : ODataController
{
    // GET ~/api/people?api-version=1.0
    [EnableQuery]
    public IActionResult Get( ODataQueryOptions<Person> options ) =>
        Ok( new Person[]
        {
            new()
            {
                Id = 1,
                FirstName = "Bill",
                LastName = "Mei",
                Email = "bill.mei@somewhere.com",
                Phone = "555-555-5555",
            },
        } );

    // GET ~/api/people/{key}?api-version=1.0
    // GET ~/api/people({key})?api-version=1.0 // TODO: does not work
    [EnableQuery]
    public IActionResult Get( int key, ODataQueryOptions<Person> options ) =>
        Ok( new Person()
        {
            Id = key,
            FirstName = "Bill",
            LastName = "Mei",
            Email = "bill.mei@somewhere.com",
            Phone = "555-555-5555",
        } );
}