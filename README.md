# signalRTest
test usage signalR + PostgreSQL + ASP.NET core

1. Create Database on PostgreSQL 13 server
2. use /DB/db.sql for build and fill tables
3. Setup connection at appsettings.Development.json /  appsettings.Production.json
4. Build server application & start from Visual Studio
5. Or start server application manually 
	Server.exe
6. Build agent application
7. Start agent application with server url as parameter
	for example: 
	agent.exe http://localhost:30809/agent
