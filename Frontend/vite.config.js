import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    watch: {
      usePolling: true // Required for WSL2 and Docker.
    },
    cors: true // Required for WSL2 and Docker.
  },
  envPrefix: 'VITE_', // Environment variables prefix for Vite.
})