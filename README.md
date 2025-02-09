# CMAP Assessment By Aliya Aziz

# EF initial create (creates the migrations)
dotnet ef migrations add InitialCreate -o Migrations -s ../TimesheetApp

# Build the solution
dotnet build

# Run the .NET MVC app
dotnet run --project .\TimesheetApp

# Run the tests
dotnet test

# URL of application running locally
http://localhost:3000/