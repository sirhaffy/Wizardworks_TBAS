// Import environment variables for API base URL and API key
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;
const API_KEY = import.meta.env.VITE_API_KEY;

// TODO: Add a check to see if anything is recieved from the API, when docker is not started there should be an error.
// TODO: Add an <ErrorBoundary> component to catch errors in the API request and display a user-friendly error message,
// TODO: and wrap the <App> component in it to handle any errors that occur during rendering.

// Define an asynchronous function for making API requests
export const apiFetch = async (endpoint, method = "GET", body = null) => {
    try {
        // Check if API configuration is available
        if (!API_BASE_URL || !API_KEY) {
            throw new Error('Missing API configuration');
        }

        // Set up request options
        const options = {
            method,
            headers: {
                "Content-Type": "application/json",
                "X-API-Key": API_KEY
            }
        };

        // Add request body if provided
        if (body) {
            options.body = JSON.stringify(body);
        }

        // Send the API request
        const response = await fetch(`${API_BASE_URL}${endpoint}`, options);

        // Get the response text
        const text = await response.text();
        
        // TODO: Remove, just for testing.
        console.log("response", response);
        
        // Check if the response is not OK (status code outside 200-299 range)
        if (!response.ok) {
            throw new Error(`API request failed: ${response.status} ${response.statusText}`);
        }

        // Handle empty responses (status 204 or empty text)
        if (response.status === 204 || !text) {
            return null;
        }

        // Parse and return the JSON response
        return JSON.parse(text);
    } catch (error) {
        // Log any errors that occur during the API request
        console.error('API request error:', error);

        // Detect if the backend is unreachable
        if (error.message.includes('Failed to fetch') || error.message.includes('NetworkError')) {
            throw new Error('Cannot connect to the server. The backend or database may be down.');
        }

        // Re-throw the error if it's not a network-related error
        throw error;
    }
};
