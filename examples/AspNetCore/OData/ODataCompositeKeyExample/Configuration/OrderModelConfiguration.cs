namespace ApiVersioning.Examples.Configuration;

using ApiVersioning.Examples.Models;
using Asp.Versioning;
using Asp.Versioning.OData;
using Microsoft.OData.ModelBuilder;

public class OrderModelConfiguration : IModelConfiguration
{
    private static void ConfigureV1( ODataModelBuilder builder ) => ConfigureCurrent( builder );

    private static void ConfigureV2( ODataModelBuilder builder )
    {
        var order = ConfigureCurrent( builder );

        // INFO: keys in routing URI is alphabetical by default
        // api/v{version:apiVersion}/Orders(customer={keycustomer},id={keyid})
        order.HasKey( p => new { p.Id, p.Customer } );

        // If you want, you can set the order explicitly: api/v{version:apiVersion}/Orders(id={keyid},customer={keycustomer})
        // var orderType = builder.EntityType<Order>();
        // orderType.Property( p => p.Id ).Order = 1;
        // orderType.Property( p => p.Customer ).Order = 2;
    }

    private static EntityTypeConfiguration<Order> ConfigureCurrent( ODataModelBuilder builder )
    {
        var order = builder.EntitySet<Order>( "Orders" ).EntityType;

        order.HasKey( p => p.Id );

        return order;
    }

    public void Apply( ODataModelBuilder builder, ApiVersion apiVersion, string routePrefix )
    {
        if ( routePrefix != "api/v{version:apiVersion}" )
        {
            return;
        }

        switch ( apiVersion.MajorVersion )
        {
            case 1:
                ConfigureV1( builder );
                break;
            case 2:
                ConfigureV2( builder );
                break;
            default:
                ConfigureCurrent( builder );
                break;
        }
    }
}