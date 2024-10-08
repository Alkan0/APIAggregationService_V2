# ASP.NET Core Web API (.NET 8.0) - API Aggregation Service

### This project is an ASP.NET Core Web API application built with .NET 8.0. It integrates with several external services to provide aggregated data through a unified API. The project includes endpoints for fetching cat images, weather data, and Spotify track/album details.
![Screenshot 2024-08-28 103643](https://github.com/user-attachments/assets/e92560e8-1fdd-4aac-816f-49c2f2026051)

## Features

- Cat API Integration:
  - Fetch random cat images.
  - Retrieve a list of cat breeds.
  - Add or remove favorite cat images.

- Weather API Integration:
  - Fetch weather information based on city names using OpenWeatherMap.

- Spotify API Integration:
  - Get track details.
  - Get album details.
  - Retrieve playlist tracks.

## API Endpoints
### Cat Controller

  - Fetch Random Cat Images
    - GET /api/cats/images
      Returns a list of random cat images.

  - Fetch All Breeds
    - GET /api/cats/breeds
      Returns a list of all cat breeds.

  - Add a Cat to Favorites
    - POST /api/cats/favorites?imageId={imageId}&userId={userId}
      Adds a cat image to the user's favorites.

  - Remove a Favorite Cat
    - DELETE /api/cats/favorites/{favoriteId}
      Removes a cat image from the user's favorites.

### OpenWeatherMap Controller

  - Fetch Weather Information
    - GET /api/openweathermap/{city}
      Retrieves weather information for the specified city.

## Spotify Controller
### The base-62 identifier found at the end of the Spotify or an artist, track, album, playlist, etc. 

  - Get Track Details
    - GET /api/spotify/track/{trackId} 
      Retrieves details of a specific track.

  - Get Album Details
    - GET /api/spotify/album/{albumId}
      Retrieves details of a specific album.

  - Get Playlist Tracks
    - GET /api/spotify/playlist/{playlistId}
      Retrieves tracks from a specific playlist.

## Getting Started
### Prerequisites
- .NET 8.0 SDK
- API keys for Spotify, OpenWeatherMap, and any required services for the Cat API

## Installation
### Clone the Repository
```bash
git clone https://github.com/Alkan0/APIAggregationService_V2.git
cd APIAggregationService_V2
```

## Install Dependencies
- Restore the NuGet packages:
```bash
dotnet restore
```
## Configuration
#### Update the Program.cs, appsettings.json and Cat's services files with your API keys.
#### Configure the API keys for Spotify, OpenWeatherMap, and other required services.

## Running the Application locally
```bash
dotnet build
dotnet run
```

### Access the Endpoints

- Cat Images: 
    - GET /api/cats/images

- Breeds: 
    - GET /api/cats/breeds

- Add Favorite: 
    - POST /api/cats/favorites?imageId={imageId}&userId={userId}

- Remove Favorite: 
    - DELETE /api/cats/favorites/{favoriteId}

- Weather Information: 
    - GET /api/openweathermap/{city}

- Track Details: 
    - GET /api/spotify/track/{trackId}

- Album Details: 
    - GET /api/spotify/album/{albumId}

- Playlist Tracks: 
    - GET /api/spotify/playlist/{playlistId}

## Contributing

### Feel free to submit issues or pull requests. 

