# AGS
## Introduction
AGS is a solution that consists of several components and each component has its own purpose:
- AGS Web: It is a web application that users will interact with. It calls several other compoenents for functionalities. 
- AGS Identity: It is a Rest API that works as a identity server for other several compoenents. 
- AGS Document: [Coming soon...]It is a GraphQL service that works as a document management service and uses AGS Identity as an authentication and authorization service.
- AGS Flow: [Coming soon...]
- AGS Notification: [Coming soon...]

You can find the demo website here: https://ags.azurewebsites.net/ (Free tier so it takes around a min to initialize). 


The technologies that are used in each component are as below:
- All: Azure Pipelines, Azure App Service, Azure Blob Storage
- AGS Web: NextJS, ReactJS, Reactstrap
- AGS Identity: ASP.NET Core Web API, Identity Server 4, EF Core 
- AGS Document: ASP.NET Core Web API, GraphQL .net