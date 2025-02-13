import React, { useRef, useCallback } from 'react';

const AnimatedButton = ({ children, onClick, ...props }) => {
    const buttonRef = useRef(null);

    const handleClick = useCallback((event) => {
        const button = buttonRef.current;
        if (button) {
            button.classList.add('animate-press');

            const handleAnimationEnd = () => {
                button.classList.remove('animate-press');
                button.removeEventListener('animationend', handleAnimationEnd);
            };

            button.addEventListener('animationend', handleAnimationEnd);
        }

        // Call the original onClick handler if provided
        if (onClick) {
            onClick(event);
        }
    }, [onClick]);

    return (
        <button ref={buttonRef} onClick={handleClick} {...props}>
            {children}
        </button>
    );
};

export default AnimatedButton;
