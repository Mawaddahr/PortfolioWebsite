@echo off
echo Starting both frontend and backend development servers...
start cmd /k "cd UI && cd src && npm run dev"
start cmd /k "dotnet run"