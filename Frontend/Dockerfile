FROM node:20-alpine AS build
WORKDIR /app

ARG BUILD_TIME
ENV BUILD_TIME=${BUILD_TIME:-unknown}

# Kopiera package.json först för bättre cache
COPY package.json yarn.lock ./
RUN yarn install --frozen-lockfile

# Kopiera resten av filerna
COPY . .
ARG VITE_API_BASE_URL
ARG VITE_API_KEY
ENV VITE_API_BASE_URL=${VITE_API_BASE_URL}
ENV VITE_API_KEY=${VITE_API_KEY}

# Debug - visa miljövariabler
RUN echo "Building with VITE_API_BASE_URL=$VITE_API_BASE_URL"

# Bygg applikationen
RUN yarn build

# Produktionsimage
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]