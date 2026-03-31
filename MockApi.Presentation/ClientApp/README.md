
# ClientApp

React frontend for the mock JSON generator.

## Running the code

1. Install dependencies:
   `npm install`
2. Create a local env file:
   copy `.env.example` to `.env`
3. Start the backend:
   `dotnet run --launch-profile http --project ..\MockApi.Presentation.csproj`
4. Start the frontend:
   `npm run dev`

## API URL

The frontend reads backend base URL from `VITE_API_BASE_URL`.

Default local value:
`http://localhost:5255`

If `VITE_API_BASE_URL` is empty, frontend uses the current origin or Vite proxy.
  
