import React, { useState } from 'react';
import CreateRectangle from './CreateRectangle';
import RectangleList from './RectangleList';
import './App.css';

const App = () => {
    const [shouldRefresh, setShouldRefresh] = useState(false);

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