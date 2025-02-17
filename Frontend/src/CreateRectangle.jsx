import React, {useEffect, useState} from 'react';
import { apiFetch } from './utils/api';
import AnimatedButton from "./components/AnimatedButton.jsx";

const CreateRectangle = ({ onRectangleCreated }) => {

    const [isProcessing, setIsProcessing] = useState(false);
    const [addSuccess, setAddSuccess] = useState(false);
    const [addError, setAddError] = useState('');
    const [clearSuccess, setClearSuccess] = useState(false);
    const [clearError, setClearError] = useState('');

    // Log messages in development mode when states change.
    useEffect(() => {
        if (process.env.NODE_ENV === 'development') {
            // Debug: Log success and error messages
            // if (addSuccess) console.log('New rectangle added successfully!');
            // if (clearSuccess) console.log('All rectangles cleared successfully!');

            if (addError) console.error(addError);
            if (clearError) console.error(clearError);
        }
    }, [addSuccess, clearSuccess, addError, clearError]);

    // Handle form submission to create a new rectangle.
    const handleSubmit = async (e) => {
        e.preventDefault();

        if (isProcessing) return; // Prevent multiple submissions

        setIsProcessing(true);
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
                let n = Math.floor(Math.sqrt(rectangles.length));
                let step = rectangles.length - n * n;

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

            // Wait for the new rectangle to be created.
            onRectangleCreated();

            // Reset status messages
            setAddSuccess(true);

        } catch (err) {
            setAddError('Failed to create rectangle. Please try again.');
        } finally {
            setIsProcessing(false);
        }
    };

    // Handle clearing all rectangles
    const handleClear = async () => {
        // Prevent multiple submissions
        if (isProcessing) return;
        setIsProcessing(true);

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
        } finally {
            setIsProcessing(false);
        }
    };

    return (
        <div className="rectangle-controls-container">
            <form onSubmit={handleSubmit} className="rectangle-controls">
                <AnimatedButton type="submit" disabled={isProcessing}>
                    {isProcessing ? 'Processing...' : 'Add square'}
                </AnimatedButton>
                <AnimatedButton type="button" onClick={handleClear} disabled={isProcessing}>
                    {isProcessing ? 'Processing...' : 'Clear'}
                </AnimatedButton>
            </form>
            {addError && <p className="error-message">{addError}</p>}
            {clearError && <p className="error-message">{clearError}</p>}
        </div>
    );
};

export default CreateRectangle;
