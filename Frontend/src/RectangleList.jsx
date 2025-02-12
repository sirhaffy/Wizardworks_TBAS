import React, { useEffect, useState } from "react";
import { apiFetch } from "./utils/api";

const RectangleList = ({ refresh }) => {
    const [rectangles, setRectangles] = useState([]);
    const [error, setError] = useState('');

    // Fetch rectangles when the component mounts or when 'refresh' changes.
    useEffect(() => {
        fetchRectangles().then(r => r);
    }, [refresh]);

    // Fetch rectangles from the API and sort them.
    const fetchRectangles = async () => {
        try {
            const data = await apiFetch('/rectangle');
            const sortedData = data.sort((a, b) => (a.x !== b.x ? a.x - b.x : a.y - b.y));
            setRectangles(sortedData);
        } catch (error) {
            console.error('Error fetching rectangles:', error);

            if (error.message.includes('Cannot connect to the server')) {
                setError('The database is unreachable. Please check if the server is running.');
            } else {
                setError('Failed to fetch rectangles. Please try again later.');
            }
        }
    };
    
    // Group rectangles by their x-coordinate (column-based grouping).
    const groupedRectangles = rectangles.reduce((acc, rect) => {
        acc[rect.x] = acc[rect.x] || [];
        acc[rect.x].push(rect);
        return acc;
    }, {});

    return (
        <div className="rectangle-container">
            
            {/* Display an error message if fetching fails. */}
            {error && <p className="error-message">{error}</p>}

            {/* Display a message if there are no squares to display. */}
            {rectangles.length === 0 && <p>No squares to display.</p>}
            
            <div className="rectangle-columns">
                {Object.entries(groupedRectangles).map(([columnIndex, columnRectangles]) => (
                    <div
                        key={columnIndex}
                        className="rectangle-column"
                    >
                        {columnRectangles.map((rect, index) => (
                            <div
                                key={rect.id || index}
                                className="rectangle"
                                style={{
                                    backgroundColor: rect.color // Set the rectangle color.
                                }}
                            />
                        ))}
                    </div>
                ))}
            </div>
        </div>
    );
};

export default RectangleList;
