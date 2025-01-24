import React, { useState } from 'react';
import { apiFetch } from './utils/api';

const CreateRectangle = ({ onRectangleCreated  }) => {
    const [success, setSuccess] = useState(false);
    const [error, setError] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        setSuccess(false);
        setError('');

        const randomColor = '#' + Math.floor(Math.random() * 16777215).toString(16);

        try {
            await apiFetch('/rectangle', 'POST', { color: randomColor });
            onRectangleCreated(); // Anropa callback
            setSuccess(true);
        } catch (err) {
            setError('Failed to create rectangle. Please try again.');
        }
    };

    const handleClear = async () => {
        try {
            await apiFetch('/rectangle', 'DELETE');
            onRectangleCreated(); // Anropa callback
            setSuccess(true);
            setError('');
        } catch (err) {
            setError('Failed to clear rectangles. Please try again.');
        }
    };

    return (
        <div className="create-rectangle">
            <h2>Create Rectangle</h2>
            <form onSubmit={handleSubmit}>
                <button type="submit">Create Random Rectangle</button>
                <button type="button" onClick={handleClear}>Clear All Rectangles</button>
            </form>
            {success && <p className="success-message">Rectangle created successfully!</p>}
            {error && <p className="error-message">{error}</p>}
        </div>
    );
};

export default CreateRectangle;