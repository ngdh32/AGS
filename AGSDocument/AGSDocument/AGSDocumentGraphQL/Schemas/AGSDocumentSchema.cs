using System;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

public class AGSDocumentGraphQLSchema : Schema
{
    public AGSDocumentGraphQLSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetRequiredService<AGSDocumentQueryType>();
        // Mutation = provider.GetRequiredService<AGSDocumentMutationType>();
    }
}
