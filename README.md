# CompanyManager
## Summary
Application designed to help manage beauty salon. It consists of calendar (like Google Calendar) where user can create, edit, delete visits. 
UI is in Polish language because it will be used by my friend when finished.

In future there will be also full treatment history for each client with photos documentation (stored on Blob Storage). 

Project is still in progress. Currently only calendar is available with partialy implementented UnitTests and IntegrationTests. 

Calendar view Sample: 
![image](https://user-images.githubusercontent.com/27851909/164258293-b47807d6-1b51-470e-ba37-1f5ae8c715e5.png)

Create visit form sample (explanation for each section below):
![image](https://user-images.githubusercontent.com/27851909/164260216-168dd243-33b6-49f3-bc07-a9fe6e141ccb.png)
1. Basic information like date, hour, client name (field with autocomplete to help search for already existing clients in database) etc.
2. All provided services with filter in top section.
3. Summary of selected services with possibility to change it's default price and duration.
4. Summary of total price and time range.


Stack: 
* .NET 6
* Blazor WASM
* EF Core
* MudBlazor 
* SQL Server 
* Azure Blob Storage (not implemented yet)
* Identity Server
* XUnit
* AutoFixture
* FluentAssertions
* Moq

## Run it locally
If you want to run the app on your machine remember to register and login because every endpoint needs authorization.
In post registration page rember to confirm account: ![image](https://user-images.githubusercontent.com/27851909/164262666-565a2dc2-a2fb-4f53-b8c2-d533f909722c.png)
It is easy to miss :)
    
