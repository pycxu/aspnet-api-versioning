namespace ApiVersioning.Examples.Controllers;

using ApiVersioning.Examples.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

[ApiVersion( 2.0 )]
[ControllerName( "People" )]
public class People2Controller : ODataController
{
    // GET ~/api/people?api-version=2.0
    [EnableQuery]
    public IActionResult Get( ODataQueryOptions<Person> options ) =>
        Ok( new Person[]
        {
            new()
            {
                Id = 1,
                FirstName = "Bill",
                LastName = "Mei",
                Email = $"bill.mei@somewhere.com",
                Phone = "555-555-5555",
            },
        } );

    // GET ~/api/people/email={keyemail},id={keyid}?api-version=2.0
    // GET ~/api/people(email={keyemail},id={keyid})?api-version=2.0 // TODO: does not work
    [EnableQuery]
    public IActionResult Get( int keyid, string keyemail, ODataQueryOptions<Person> options ) =>
        Ok( new Person()
        {
            Id = keyid,
            FirstName = "Bill",
            LastName = "Mei",
            Email = $"{keyemail} (composite key)",
            Phone = "555-555-5555",
        } );
}