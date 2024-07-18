# AmegaGroupTest

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

