FROM node:20-alpine AS build
WORKDIR /app

# Debugutskrift för att se miljövariabler
RUN env | grep VITE || true

# Add CACHE_BUST arg
ARG CACHE_BUST
ENV CACHE_BUST=${CACHE_BUST:-1}

# Rensa gamla builds
RUN rm -rf node_modules .cache dist

# Kopiera package.json först
COPY package.json yarn.lock ./
RUN yarn install --frozen-lockfile

# Kopiera alla filer inklusive .env
COPY . .

# Debugutskrift av .env innehåll
RUN cat .env || true

# Bygg applikationen
RUN echo "Building with CACHE_BUST=${CACHE_BUST}"
RUN yarn build --mode production --force

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]