﻿using System.Text.RegularExpressions;
using CTSCore.Models;
using Reflection;
using Shouldly;
using Xunit;

namespace ModelBuilder.Tests;

public class ModelSourceProviderTests
{
    // This should be defined here in the tests because the actual refelctor should never have a fixed asm path.
    private const string DbContextAsmPath = @"C:\Users\brady\projects\ApiGen\Library\CTSCore.dll";

    [Fact]
    public void PropertTypeDeclarationsAreCorrect()
    {
        var sourceProvider = new ModelSourceProvider();
        var entityType = typeof(CourseTemplate);
        var dbRef = new DbContextReflector();
        var pRef = new PropertyReflector();
        
        var expectedModels = dbRef.GetEntityProperties(entityType)
            .Select(p => pRef.GetPropertyModel(p))
            .ToList();
        var expectedDecs = expectedModels.Select(m => $"public {m.BuildPropertyDeclaration()}").ToList();
        
        var actualCode = sourceProvider.BuildDtoForEntity(entityType);

        var actualDecs = GetPropertyDecsFromType(actualCode).ToList();
        //actualDecs.ShouldBe(expectedDecs, ignoreOrder: true);
    }

    private List<PropertyModel> GetPropertyDecsFromType(string modelSource)
    {
        // TASKT: Get fancy and use RegEx to pull the properties.

        var rx = new Regex(@"(?'access'\w+) (?'type')\w+ (?'name'\w+)");
        var decs = rx.Match(modelSource);

        return new List<PropertyModel>();
    }
}