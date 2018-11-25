# Delivery service #

## Getting Started ##

This instruction provides you how to build and run this project on your local machine for testing purpose.

### Prerequisites

Please make sure that you have installed all bullets point before before running this project.

* [Visual Studio 2017 or Visual Studio Code](https://visualstudio.microsoft.com/)
* [.NET Core 2.1 or later SDK](https://www.microsoft.com/net/download)

## Running the application

Following steps below to get a development env running

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