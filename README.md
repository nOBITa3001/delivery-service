# Delivery service

## Getting Started

This instruction provides you how to build and run this project on your local machine for testing purpose.

### Prerequisites

Please make sure that you have installed all bullets point before before running this project.

* [Visual Studio 2017 or Visual Studio Code](https://visualstudio.microsoft.com/)
* [.NET Core 2.1 or later SDK](https://www.microsoft.com/net/download)

## Running the application

Following steps below to get a development env running

### Cloning repository

```
λ git clone https://github.com/nOBITa3001/delivery-service.git
```

### Using Visual Studio 2017

```
> Go to delivery-service/src folder
> Open DS.sln file
> Set DS.API as startup project, if it is not
> Press F5 or CTRL F5 (:
```

### Using Visual Studio Code

```
> Open command prompt or terminal
λ cd delivery-service/src/DS.API/
λ dotnet run
> Open web browser and go to http://localhost:5000/swagger or https://localhost:5001/swagger
```

### Note: On the swagger page, you will see a required field for a version, please fill a number, i.e. 1

## Running automated tests

### Using Visual Studio 2017

```
> Run all test by clicking "Run All" link on Test Explorer window or CTRL + R, A
```

### Using Visual Studio Code

```
> Open command prompt or terminal
λ cd delivery-service/src/
λ dotnet test
```

## Project details

Here is the application’s layered architecture:

![application's structure](https://github.com/nOBITa3001/delivery-service/blob/master/application-structure.png)

- DS.API - provides a way to communicate with clients
- DS.Handlers - validates, processes and controls workflow for all requests from the API. It is a business layer
- DS.DomainModel - contains all business domains and provides factories for creating validated domains
- DS.DataAccess - is a layer for providing how to communicate with persistent storage
- DS.Infrastructure/DA.Infrastructure.web - provide fundamental functions for others
- DS.Contract - is a main place if you are looking for an interface among projects
- DS.Dtos – provides all data transfer objects!