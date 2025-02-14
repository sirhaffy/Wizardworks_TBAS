import React, { useState } from 'react';
import CreateRectangle from './CreateRectangle';
import RectangleList from './RectangleList';
import { ErrorBoundary } from './utils/ErrorBoundary';
import { API_BASE_URL, API_KEY } from './config';
import './styling/main.css';

const App = () => {
    const [shouldRefresh, setShouldRefresh] = useState(false);

    const handleRectangleCreated = () => {
        setShouldRefresh(prev => !prev);
    };

    console.log("Using configuration:", process.env.NODE_ENV);
    console.log("API Base URL:", API_BASE_URL);
    console.log("API Key:", API_KEY);

    return (
        <ErrorBoundary>
            <div className="app-container">
                <h1>Rectangle Manager</h1>
                <CreateRectangle onRectangleCreated={handleRectangleCreated} />
                <RectangleList refresh={shouldRefresh} />
            </div>
        </ErrorBoundary>
    );
};

export default App;