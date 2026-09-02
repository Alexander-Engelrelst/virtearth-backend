# VirtEarth backend

## Overview
This repository contains the C# backend for the VirtEarth POC. It was written for .NET 8.

* **Frontend repository**: [VirtEarth frontend](https://github.com/Alexander-Engelrelst/virtearth-frontend)
* **Live Demo**: [Watch a demo of the game](https://youtu.be/sPvokRURzJg)

## Credits
I was the main developer of the backend

The setup of everything docker related was done by **Kobe Vandenberghe**

The startercode mainly including boilerplate was provided by **Matthias Blomme**

## Usage
Before starting this server docker must be running and .NET 8 must be installed.

Since this is a simple POC and was never supposed to run in production and only locally mock credentials are used 
and provided to allow seamless usage. In a real production environment these would have been replaced with real 
credentials and proper environment variables.

```bash
docker compose -f config/docker/docker-compose.yml up -d
dotnet run --project src/Adria.Main
```

## Key Features
* **Game Cleanup**: The backend automatically cleans up old games that have been inactive for a certain period of 
  time as to avoid filling up memory with stale game sessions. ([ActiveGamesCleanupService.cs](./src/Adria.Infrastructure/BackgroundServices/ActiveGamesCleanupService.cs))
* **Custom maze algorithm**: The backend  uses a backtracking algorithm to generate a maze for every new game 
  session. ([MazeGenerator.cs](./src/Adria.Domain/games/MazeGenerator.cs))