To run this you will need:
Microsoft SQL server
Optional SSMS(To view the DB)

First download the secrets.env file as provided and add it to the PROJECT folder (Dotaproject/Dotaproject/secrets.env)

After downloading the code and the related dependencies navigate to Dotaproject/dotaproject and run:
(If theese commands dont work, run "dotnet tool install --global dotnet-ef")

dotnet ef migrations add InitialPlayerMigration --context PlayerDbContext
dotnet ef database update --context PlayerDbContext
dotnet ef migrations add InitialAuthMigration --context AuthDbContext
dotnet ef database update --context AuthDbContext

This sets up the schema for the database, the code will give you 2 default users
EricAdmin//any12345 >> Policy(Role) admin
EricUser//any12345  >> policy(Role) verifiedUser

To run the Scraper
npx ts-node src/index.ts
