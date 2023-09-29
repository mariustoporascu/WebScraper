## Dependencies
dotnet sdk 8
docker
docker-compose

## Build

Run `dotnet build` to build the solution.

## Run

To run the web application:

```bash
cd .\docker\
docker-compose up
```

## Code Styles & Formatting

The template includes [EditorConfig](https://editorconfig.org/) support to help maintain consistent coding styles for multiple developers working on the same project across various editors and IDEs. The **.editorconfig** file defines the coding styles applicable to this solution.

## Test

To run the tests:
```bash
dotnet test
```

## Benchmarks

To run the benchmarks:
```bash
cd .\benchmarks\Benchmarks\
dotnet run -c Release
```

## API Documentation
IF you run using docker-compose
Api URL: http://localhost:8080
UI URL: http://localhost:3000
RabbitMQ: http://localhost:15672
Selenium Hub: http://localhost:4444