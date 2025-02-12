import React, { useState } from 'react';
import CreateRectangle from './CreateRectangle';
import RectangleList from './RectangleList';
import './styling/main.css';

const App = () => {
    const [shouldRefresh, setShouldRefresh] = useState(false);

    // Toggle refresh state to trigger RectangleList update
    const handleRectangleCreated = () => {
        setShouldRefresh(prev => !prev);
    };

    return (
        <div className="app-container">
            <h1>Rectangle Manager</h1>
            <CreateRectangle onRectangleCreated={handleRectangleCreated} />
            <RectangleList refresh={shouldRefresh} />
        </div>
    );
};

export default App;