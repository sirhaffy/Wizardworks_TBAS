import React, {useEffect, useState} from 'react';
import { apiFetch } from './utils/api';
import AnimatedButton from "./components/AnimatedButton.jsx";

const CreateRectangle = ({ onRectangleCreated }) => {
    
    // States for tracking success and error messages
    const [addSuccess, setAddSuccess] = useState(false);
    const [addError, setAddError] = useState('');
    const [clearSuccess, setClearSuccess] = useState(false);
    const [clearError, setClearError] = useState('');

    // Log messages in development mode when states change.
    useEffect(() => {
        if (process.env.NODE_ENV === 'development') {
            if (addSuccess) console.log('New rectangle added successfully!');
            if (clearSuccess) console.log('All rectangles cleared successfully!');
            if (addError) console.error(addError);
            if (clearError) console.error(clearError);
        }
    }, [addSuccess, clearSuccess, addError, clearError]);
    
    // Handle form submission to create a new rectangle.
    const handleSubmit = async (e) => {
        e.preventDefault();

        // Reset all status messages
        setAddSuccess(false);
        setAddError('');
        setClearSuccess(false);
        setClearError('');

        try {
            // Fetch existing rectangles from the API
            const rectangles = await apiFetch('/rectangle');
            let position;

            if (rectangles.length === 0) {
                position = { x: 0, y: 0 }; // Staring position.
            } else {
                let n = Math.floor(Math.sqrt(rectangles.length)); // Determine current square size.
                let step = rectangles.length - n * n; // Find the next position within the square.

                if (step < n) {
                    position = { x: n, y: step }; // Expand to the right.
                } else {
                    position = { x: n - (step - n), y: n }; // Expand downward and to the left.
                }
            }

            const rectangleData = {
                color: '#' + Math.floor(Math.random() * 11184810 + 5592405).toString(16), // Random color, avoid dark.
                x: position.x,
                y: position.y
            };

            // Send the new rectangle data to the API.
            await apiFetch('/rectangle', 'POST', rectangleData);
            onRectangleCreated();
            setAddSuccess(true);
        } catch (err) {
            setAddError('Failed to create rectangle. Please try again.');
        }
    };

    // Handle clearing all rectangles
    const handleClear = async () => {
        // Reset all status messages
        setAddSuccess(false);
        setAddError('');
        setClearSuccess(false);
        setClearError('');

        try {
            await apiFetch('/rectangle', 'DELETE');
            onRectangleCreated();
            setClearSuccess(true);
        } catch (err) {
            setClearError('Failed to clear rectangles. Please try again.'); 
        }
    };

    return (
        <div className="rectangle-controls-container">
            <form onSubmit={handleSubmit} className="rectangle-controls">
                <AnimatedButton type="submit">Add square</AnimatedButton>
                <AnimatedButton type="button" onClick={handleClear}>Clear</AnimatedButton>
            </form>
        </div>
    );
};

export default CreateRectangle;
