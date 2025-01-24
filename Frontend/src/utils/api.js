const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export const apiFetch = async (endpoint, method = "GET", body = null) => {
    const url = `${API_BASE_URL}${endpoint}`;
    const options = {
        method,
        headers: {
            "Content-Type": "application/json",
            "X-API-Key": import.meta.env.VITE_API_KEY
        }
    };

    if (body) {
        options.body = JSON.stringify(body);
    }

    const response = await fetch(url, options);
    const text = await response.text();
    
    // TESTS
    // console.log('API URL:', url);
    // console.log('Response status:', response.status);
    // console.log('Response text:', text);

    if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
    }
    return response.status === 204 ? null : JSON.parse(text);
};