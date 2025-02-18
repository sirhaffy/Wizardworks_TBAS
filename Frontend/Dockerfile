FROM node:20-alpine AS build
WORKDIR /app/Frontend

# Build arguments
ARG VITE_API_KEY
ARG VITE_BACKEND_URL

# Miljövariabler för build
ENV VITE_API_KEY=$VITE_API_KEY
ENV VITE_BACKEND_URL=$VITE_BACKEND_URL

# Copy package.json and yarn.lock
COPY Frontend/package.json Frontend/yarn.lock ./

RUN yarn install --frozen-lockfile

# Copy the rest of the frontend files
COPY Frontend ./

# Build the application with environment variables
RUN yarn build

# Production image
FROM nginx:alpine

# Copy the built files
COPY --from=build /app/Frontend/dist /usr/share/nginx/html

# Copy nginx config
COPY Frontend/nginx.conf.template /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]