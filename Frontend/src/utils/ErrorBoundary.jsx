import React from 'react';

class ErrorBoundary extends React.Component {
    constructor(props) {
        super(props);
        this.state = { hasError: false, error: null };
    }

    static getDerivedStateFromError(error) {
        return { hasError: true, error };
    }

    componentDidCatch(error, errorInfo) {
        console.error('Error caught by boundary:', error, errorInfo);
    }

    render() {
        if (this.state.hasError) {
            return (
                <div className="p-4 mb-4 text-red-700 bg-red-100 rounded-lg" role="alert">
                    <h2 className="text-lg font-semibold mb-2">Something went wrong</h2>
                    <p className="text-sm">
                        {this.state.error?.message || 'An unexpected error occurred'}
                    </p>
                </div>
            );
        }

        return this.props.children;
    }
}

export { ErrorBoundary };