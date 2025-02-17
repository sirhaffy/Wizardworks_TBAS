import React, { useEffect, useState } from "react";
import { apiFetch } from "./utils/api";

const RectangleList = ({ refresh }) => {
    const [rectangles, setRectangles] = useState([]);
    const [error, setError] = useState('');

    // Fetch rectangles when the component mounts or when 'refresh' changes.
    useEffect(() => {
        const fetchRectangles = async () => {
            try {
                const data = await apiFetch('/rectangle');
                const sortedData = data.sort((a, b) => (a.x !== b.x ? a.x - b.x : a.y - b.y));
                setRectangles(sortedData);
            } catch (error) {
                console.error('Error fetching rectangles:', error);
                setError(error.message.includes('Cannot connect to the server')
                    ? 'The database is unreachable. Please check if the server is running.'
                    : 'Failed to fetch rectangles. Please try again later.');
            }
        };

        fetchRectangles();
    }, [refresh]);

    // Group rectangles by their x-coordinate (column-based grouping).
    const groupedRectangles = rectangles.reduce((acc, rect) => {
        acc[rect.x] = acc[rect.x] || [];
        acc[rect.x].push(rect);
        return acc;
    }, {});

    return (
        <div className="rectangle-container">


            <div className="rectangle-columns">
                {Object.entries(groupedRectangles).map(([columnIndex, columnRectangles]) => (
                    <div
                        key={columnIndex}
                        className="rectangle-column"
                        >
                        {columnRectangles.map((rect, index) => (
                            <div
                            key={rect.id || index}
                            className="rectangle rectangle-effect"
                            style={{
                                backgroundColor: rect.color
                            }}
                            />
                        ))}
                    </div>
                ))}
            </div>
                {error && <p className="error-message">{error}</p>}
                {!error && rectangles.length === 0 && <p>No squares to display.</p>}
        </div>
    );
};

export default RectangleList;
