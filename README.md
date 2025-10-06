C# Trading system in terminal v. 1.0

Trading system är ett enkel konsolapplikation byggt i C# och .NET 8.

Applikationen kan laddas ner från Github:

https://github.com/DusanTodo/trading_system


Efter nedladdningen, placera alla filer i ett mapp. 

Startas från terminal/powershell/git bash med kommandot "dotnet run". Krävs Visual Studio eller eller VS Code med C# stöd. 


Fungerar på Windows & Mac OS. 

Funktioner V.1.0:

-Logga in/ut.
-Registrera konto.
-Ladda upp en vara (namn + beskrivning).
-Bläddra bland andra användares varor.
-Skicka bytesförfrågningar.
-Acceptera eller neka inkommande förfrågningar.
-Bläddra bland avslutade förfrågningar (godkända / nekade).
-Radera konto, extra funktion, ej krav i uppgiften.
-Grundläggande spar och laddnings funktion med 3st .txt filer som uppdateras vid varje program avslut.


Implementation:

Klasser: user, item, trade och saveload. 

SaveLoad klassen tar hand om logiken hur filerna(3st .txt filer) fylls med data, läses in och sparas automatisk vid ändringar när program avslutas.

Uppbyggnad: Komposition, klasser är var för sig men samarbetar, trade klassen använder user och item klasser för att
komplettera trade information. Jag valde komposition eftersom mina klasser inte har en ärvrelation.
En trade är inte en user och inte ett item, utan den använder information från båda.
Därför är det mer logiskt att låta klasserna samarbeta (komposition) istället för att ärva.




