# OdataFluentQuery for c#

This lib is a generic fluent OData query generator that only generates the query string.

## Filtering with `FilterBy`
## Simple Filter
```csharp
var odataFilter = new ODataFilter<Product>()
    .FilterBy(p => p.Name == "Table")
    .And()
    .FilterBy(p => p.Price > 100)
    .Or()
    .FilterBy(p => p.Category == "Furniture");

var filterQuery = odataFilter.Build(); // $filter=(Name eq 'Table') and (Price gt 100) or (Category eq 'Furniture')
```

## Group Filter
```csharp
var odataFilter = new ODataFilter<Product>()
    .BeginGroup()
        .FilterBy(p => p.Name == "Table")
        .Or()
        .FilterBy(p => p.Category == "Furniture")
    .EndGroup()
    .And()
    .FilterBy(p => p.Price > 100);

var filterQuery = odataFilter.Build(); // $filter=((Name eq 'Table') or (Category eq 'Furniture')) and (Price gt 100)
```
## Filter by functions
```csharp
var odataFilter = new ODataFilter<Product>()
    .Contains(p => p.Name, "Table")
    .And()
    .StartsWith(p => p.Category, "Furn");

var filterQuery = odataFilter.Build(); // $filter=contains(Name, 'Table') and startsWith(Category, 'Furn')
```

## Filter by Navigation Properties 

```csharp
var odataFilter = new ODataFilter<Student>()
    .FilterBy(s => s.Teacher.FirstName == "Adam");

var filterQuery = odataFilter.Build(); // $filter=Teacher/FirstName eq 'Adam
```csharp
