# .NET 10.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that a .NET 10.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 10.0 upgrade.
3. Upgrade GroceryShop.Dominio\GroceryShop.Dominio.csproj
4. Upgrade GroceryShop.Repositorio\GroceryShop.Repositorio.csproj
5. Upgrade GroceryShop.Angular\GroceryShop.Angular.csproj

## Settings

This section contains settings and data used by execution steps.

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name                                      | Current Version | New Version | Description                                          |
|:--------------------------------------------------|:---------------:|:-----------:|:-----------------------------------------------------|
| Microsoft.AspNetCore.Mvc.NewtonsoftJson           | 3.1.1           | 10.0.0      | Recommended for .NET 10.0                            |
| Microsoft.AspNetCore.SpaServices.Extensions       | 3.1.1           | 10.0.0      | Recommended for .NET 10.0                            |
| Microsoft.EntityFrameworkCore.Design              | 3.1.1           | 10.0.0      | Recommended for .NET 10.0                            |
| Microsoft.EntityFrameworkCore.Proxies             | 3.1.1           | 10.0.0      | Recommended for .NET 10.0                            |
| Microsoft.EntityFrameworkCore.Relational          | 3.1.1           | 10.0.0      | Recommended for .NET 10.0                            |
| Microsoft.EntityFrameworkCore.Tools               | 3.1.1           | 10.0.0      | Recommended for .NET 10.0                            |
| Pomelo.EntityFrameworkCore.MySql                  | 3.1.1           |             | Deprecated - Replace with MySql.EntityFrameworkCore or Pomelo.EntityFrameworkCore.MySql 10.0.0+ |
| Swashbuckle.AspNetCore                            | 5.0.0           | 7.2.0       | Recommended for .NET 10.0                            |
| Swashbuckle.AspNetCore.Swagger                    | 5.0.0           | 7.2.0       | Recommended for .NET 10.0                            |
| Swashbuckle.AspNetCore.SwaggerGen                 | 5.0.0           | 7.2.0       | Recommended for .NET 10.0                            |
| Swashbuckle.AspNetCore.SwaggerUi                  | 6.4.0           | 7.2.0       | Recommended for .NET 10.0                            |

### Project upgrade details

This section contains details about each project upgrade and modifications that need to be done in the project.

#### GroceryShop.Dominio\GroceryShop.Dominio.csproj modifications

Project properties changes:
  - Target framework should be changed from `netcoreapp3.1` to `net10.0`

#### GroceryShop.Repositorio\GroceryShop.Repositorio.csproj modifications

Project properties changes:
  - Target framework should be changed from `netcoreapp3.1` to `net10.0`

NuGet packages changes:
  - Microsoft.EntityFrameworkCore.Proxies should be updated from `3.1.1` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.EntityFrameworkCore.Relational should be updated from `3.1.1` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.EntityFrameworkCore.Tools should be updated from `3.1.1` to `10.0.0` (*recommended for .NET 10.0*)
  - Pomelo.EntityFrameworkCore.MySql `3.1.1` is deprecated - needs to be replaced with compatible version for .NET 10.0

#### GroceryShop.Angular\GroceryShop.Angular.csproj modifications

Project properties changes:
  - Target framework should be changed from `netcoreapp3.1` to `net10.0`

NuGet packages changes:
  - Microsoft.AspNetCore.Mvc.NewtonsoftJson should be updated from `3.1.1` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.AspNetCore.SpaServices.Extensions should be updated from `3.1.1` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.EntityFrameworkCore.Design should be updated from `3.1.1` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.EntityFrameworkCore.Relational should be updated from `3.1.1` to `10.0.0` (*recommended for .NET 10.0*)
  - Swashbuckle.AspNetCore should be updated from `5.0.0` to `7.2.0` (*recommended for .NET 10.0*)
  - Swashbuckle.AspNetCore.Swagger should be updated from `5.0.0` to `7.2.0` (*recommended for .NET 10.0*)
  - Swashbuckle.AspNetCore.SwaggerGen should be updated from `5.0.0` to `7.2.0` (*recommended for .NET 10.0*)
  - Swashbuckle.AspNetCore.SwaggerUi should be updated from `6.4.0` to `7.2.0` (*recommended for .NET 10.0*)

Feature upgrades:
  - API behavioral changes and compatibility issues need to be reviewed and addressed
  - Review usage of obsolete or changed APIs from .NET Core 3.1 to .NET 10.0

Other changes:
  - Startup.cs pattern may need updates to use minimal hosting model (Program.cs)
  - Review and update middleware configuration for .NET 10.0 compatibility