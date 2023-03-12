# {{upper||plural}} SERVICE #

--- TO DELETE ---
This is a template for a Mazecare microservice.

This are all the things to replace:
- {{pascal||plural}}
- {{camel||plural}}
- {{upper||plural}}
- {{pascal}}
- {{camel}}
- {{upper}}
--- TO DELETE

This microservice deals with everything related with {{camel||plural}}.

Its role is to be able to check if:

- create, update, read, delete {{pascal||plural}}

This service use the adapter pattern. That means that, depending on the tenant, the targetted "state" or "store" or "database" will be different. 

This service is dockerized.
This service is currently plugged into a Jenkins CI/CD pipeline.

## Environments & Multi-Tenancy ##

### Environments

An `env` represents where and how 
The exact same docker image is to be shared by every `env`.
This only thing that differs between a running image in one `env` to another image in another `env` are `env variables`.

### Multi-Tenancy ###

Every `env` is multi-tenant.
For every tenant, the store can differ and the store address can differ.

The `store`s could be:
- any database (postgresql, mongodb, ...)
- any cache (redis, ...)
- any file systems (s3, ...)
- any API connection

The `store address`es are:
- connection string for a database or a cache
- urls of file systems & credentials
- url of APIs & credentials

This structure allows everything.

## Software Architecture ##

This service currently use Domain Driven Design pattern which is the most accepted design for microservices and anything in general.

There are two main folder as this is typical to all code these days:

- `src`
- `tests`

The `tests` forlder is self-explanatory.

Under `src`, you will find:

- `Program.cs` file: Describes how the service is started and how services are configured. It also responsible for health check urls and swagger file generation & swagger UI;
- `Controllers` folder: Contains the `API`s and the Swagger examples automatically generated
- `Utils` folder: Contains classes that can be shared between microservices in the form of a NuGet package.
- `Domain` folder: Contains the core business logic of the microservice.

Under `Domain` folder, you can find:

- `Handlers` folder: Contains the commands and handler directly dispatched by the controllers
- `Models` folder: Contains the core models of this microservice

## Build ##

You can pick the IDE of your choice.
The best for C# is usually Microsoft Visual Studio, Microsoft Visual Studio Code or Jetbrains Rider.

Use your IDE to build, or use `dotnet build` in the command line.

## Run ##

Use your IDE to run, or use `dotnet run` in the command line.

To check all the APIs visually, go to <{HOST}:{PORT}/swagger.index.html>

## Test ##

Use your IDE to test, or use `dotnet test` in the command line.

## CI / CD (Jenkins) ##

This service is currently on a CI/CD Pipeline on Jenkins.

You can find the link here:
<https://34.126.154.253/blue/organizations/jenkins/{{camel}}-service/activity/>

Please check the Jenkinsfile for more information about the pipeline.


## Folder Structure ##
Wati.Template.Service
* src
  * Wati.Template.Api
    - should contain all web-api related stuff (controller, authorization, validators, exceptions etc)
  * Wati.Template.Common
    - common extension, utils, helpers, domain-models, request/response dtos used across the solution
    - should **not** have any project reference
  * Wati.Template.Orchestrator
      - orchestrator is used to convert input API request DTO to domain-models for business operations
      - this module should **not** contain business logic
      - multiple services can be DI in this to get data from different data context
  * Wati.Template.Service
      - service layer will always work on domain-models, input and output will always be domain-models
      - should not run business logic on dtos/entities
      - each service will only DI one domain data repository, if need data from another domain use orchestrator layer along with builder pattern
  * Wati.Template.Repository
      - repository layer will always work on entities, input and output will always be entities
      - repository layer is responsible to return data either from DatabaseContext or external ThirdParty API data context
  * Wati.Template.Data
      - data layer is only for Database context to connect, migrate and configure based on database entities
* tests
    * integration
      * Wati.Template.Service.IntegrationTests
          - should contain all e2e api endpoint test with and without data response
          - should run an in-memory/file-based stub database to impersonate and execute all endpoints
    * unit (every project should have an equivalent unit test project matching the exact folder structure)
        * Wati.Template.Api.UnitTests
            - should contain all edge cases under unit test with >90% code coverage



## Environments
| Environments        | URL                                                                                                                |
|------------|--------------------------------------------------------------------------------------------------------------------|
| Dev:local        | http://localhost:8088/                                                                   |
| Dev:master        |                                                                    |
| UAT |                                   |
| Production |                                   |