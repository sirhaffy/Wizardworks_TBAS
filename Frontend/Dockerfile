FROM node:20-alpine AS build
WORKDIR /app

# Kopiera package.json först
COPY package.json yarn.lock ./
RUN yarn install --frozen-lockfile

# Kopiera alla filer inklusive .env
COPY . .

# Tvinga ombyggnation av cache vid varje bygge
ARG CACHE_BUST=$(date +%s)
RUN echo "Cache bust: $CACHE_BUST"

# Bygg applikationen
RUN yarn build

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]