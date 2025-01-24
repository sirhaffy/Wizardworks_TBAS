import React, { useEffect, useState } from "react";
import { apiFetch } from "./utils/api";

const RectangleList = ({ refresh }) => {
    const [rectangles, setRectangles] = useState([]);
    const [error, setError] = useState('');

    useEffect(() => {
        fetchRectangles();
    }, [refresh]);

    const fetchRectangles = async () => {
        try {
            // TODO : FIX THIS !!
            const data = await apiFetch('/rectangle');
            setRectangles(data);
        } catch (error) {
            console.error('Error fetching rectangles:', error);
            setError('Failed to fetch rectangles. Please try again later.');
        }
    };

    return (
        <div className="rectangle-list">
            <h2>Rectangles</h2>
            {error && <p className="error-message">{error}</p>}
            <div className="rectangle-grid">
                {rectangles.map((rect, index) => (
                    <div key={index} className="rectangle" style={{ backgroundColor: rect.color }}></div>
                ))}
            </div>
        </div>
    );
};

export default RectangleList;