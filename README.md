# AmegaGroupTest

## Running and stopping the app
This implemented test task for AmegaGroup by Aleksey Mudla

To run the application 
1. Set tiingo token as user secret via command
```
dotnet user-secrets set "Token" <YOUR_TIINGO_TOKEN> --project "./FinancialInstruments/FinancialInstruments.Api/FinancialInstruments.Api.csproj"
```

2. Run the app
```
dotnet run --project  ./FinancialInstruments\FinancialInstruments.Api/FinancialInstruments.Api.csproj --propertty WarningLevel=3
```

To stop the app push Ctrl + C

## Description

The app containes two parts:
- REST-service, described by swagger https://localhost:5008/swagger/index.html
There are 2 endpoints here:
```
/api/Quotes
```
Provides 3 hardcoded tickers one can use 

```
/api/Quotes/{ticker}
```
Provides quote data (price) for ticker provided

- WebSocket-service
One can connect to web socket using link
```
ws://localhost:5008/ws
```

To subscribe to certain ticket data stream, send message
```
{
    "eventName" : "subscribe",
    "ticker" : <ticker>
}
```

eventName must be "subscribe"

ticker schould be one from /api/Quotes

## TODOs

- Unsubscription
