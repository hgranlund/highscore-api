# Highscore API

[![Build status](https://ci.appveyor.com/api/projects/status/7satk7891o0q8641/branch/master?svg=true)](https://ci.appveyor.com/project/hgranlund/highscore-api/branch/master) [![codecov](https://codecov.io/gh/hgranlund/highscore-api/branch/master/graph/badge.svg)](https://codecov.io/gh/hgranlund/highscore-api)

## Using

1. Get .NET Core 2.1 or later:

This is best done by following the instructions found [here](https://www.microsoft.com/net/download/dotnet-core/2.1).

2. Clone:

```
git clone https://github.com/hgranlund/highscore-api
```

3. Build:

```
dotnet restore
dotnet build
```

4. Run:

```
cd HighscoreApi
dotnet run
```

5. Test:

```
cd HighscoreApi.Test
dotnet test
```

## Database

To scaffold a migration and create the initial set of tables for the model, run:

    dotnet ef migrations add MigrationName

To apply the new migration to the database, run:

    dotnet ef database update

```

```
