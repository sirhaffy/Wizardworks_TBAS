const API_KEY =  import.meta.env.VITE_API_KEY;
const BASE_URL = import.meta.env.VITE_AZURE_VM_IP;

class APIError extends Error {
    constructor(message, status = null) {
        super(message);
        this.name = 'APIError';
        this.status = status;
    }
}

const checkEnvironmentVariables = () => {
    const missing = [];
    if (!API_KEY) missing.push('API_KEY');

    if (missing.length > 0) {
        console.error('Missing environment variables:', missing);
        console.log('Current environment:', {
            NODE_ENV: import.meta.env.MODE,
            BASE_URL: BASE_URL,
            API_KEY: API_KEY ? '[SET]' : '[MISSING]'
        });
        return false;
    }
    return true;
};


const checkBackendStatus = async () => {
    try {
        const response = await fetch(`${BASE_URL}/api/rectangle`, {
            mode: 'cors',
            headers: {
                'Accept': 'application/json',
                'X-API-Key': API_KEY
            }
        });
        return response.ok;
    } catch (error) {
        console.error('Backend status check failed:', error);
        return false;
    }
};

export const apiFetch = async (endpoint, method = "GET", body = null) => {
    try {
        if (!checkEnvironmentVariables()) {
            throw new APIError('Missing API configuration. Please check your environment variables.');
        }

        const isBackendUp = await checkBackendStatus();
        if (!isBackendUp) {
            throw new APIError('Cannot connect to the server. The backend or database may be down.');
        }

        const options = {
            method,
            mode: 'cors',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'X-API-Key': API_KEY
            }
        };

        if (body) {
            options.body = JSON.stringify(body);
        }

        const response = await fetch(`${BASE_URL}${endpoint}`, options);
        const text = await response.text();

        if (!response.ok) {
            throw new APIError(text || `Request failed with status ${response.status}`, response.status);
        }

        if (response.status === 204 || !text) {
            return null;
        }

        try {
            return JSON.parse(text);
        } catch (error) {
            console.error('JSON parsing error:', error);
            throw new APIError('Invalid JSON response from server');
        }
    } catch (error) {
        console.error('API request error:', error);
        if (error instanceof APIError) throw error;
        throw new APIError(error.message || 'An unexpected error occurred');
    }
};

export { APIError };