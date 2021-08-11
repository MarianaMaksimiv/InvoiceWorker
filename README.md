# InvoiceWorker

The application consumes an event feed which Xero provides and creates a custom text file for each newly created invoice.
It is fault resistant, follows all of the best practices and is a maintainable codebase that will last.


## Local Setup

1. Download and install the `.NET 5` SDK. Link [here](https://dotnet.microsoft.com/download)
2. Clone this repository.
3. Use the following commands to build and run the app.
    ```bash
    cd InvoiceWorker

    # to build the app
    dotnet build

    # to run the app
    dotnet run --project InvoiceWorker

    # to run tests
    dotnet build
    ```