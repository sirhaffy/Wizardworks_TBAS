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

# Debug - visa miljövariabler och lista filer
RUN echo "Building with VITE_API_BASE_URL=$VITE_API_BASE_URL"
RUN echo "Building with BUILD_TIME=$BUILD_TIME"

# Bygg applikationen
RUN yarn build
# Verify build output
RUN ls -la dist/
RUN cat dist/index.html

# Produktionsimage
FROM nginx:alpine
# Verifiera att nginx.conf finns
RUN ls -la /etc/nginx/conf.d/
COPY nginx.conf /etc/nginx/conf.d/default.conf
# Verifiera att nginx.conf kopierades
RUN cat /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist /usr/share/nginx/html
# Verifiera att filerna kopierades
RUN ls -la /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]