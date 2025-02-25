import React, { useState } from 'react';
import CreateRectangle from './CreateRectangle';
import RectangleList from './RectangleList';
import { ErrorBoundary } from './utils/ErrorBoundary';
import './styling/main.css';

const App = () => {
    const [shouldRefresh, setShouldRefresh] = useState(false);

    const handleRectangleCreated = async () => {

        return new Promise((resolve) => {
            setShouldRefresh(prev => {
                resolve();
                return prev + 1;
            });
        });
    };

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